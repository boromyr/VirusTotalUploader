using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DarkUI.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace uploader
{
    public partial class UploadForm : ModernForm
    {
        // Limits of the public VirusTotal API v3: files up to 32 MB go straight to /files,
        // bigger ones (up to 650 MB) through a one-time URL from /files/upload_url.
        private const long MaxDirectUploadSize = 32L * 1024 * 1024;
        private const long MaxUploadSize = 650L * 1024 * 1024;
        private const int RateLimitRetries = 8;
        private static readonly TimeSpan RateLimitDelay = TimeSpan.FromSeconds(15);
        private static readonly TimeSpan UploadTimeout = TimeSpan.FromHours(2);

        // Pause between the end of the upload and opening the result page
        private static readonly TimeSpan ResultsDelay = TimeSpan.FromSeconds(2);

        // Folder uploads ask before opening more browser tabs than this.
        private const int MaxTabsWithoutAsking = 5;

        private readonly bool _reopen;
        private readonly string _path;
        private readonly MainForm _mainForm;
        private readonly Settings _settings;
        private readonly bool _isFolder;
        private readonly RestClient _client = new RestClient(new RestClientOptions("https://www.virustotal.com")
        {
            Timeout = TimeSpan.FromMinutes(2)
        });

        private CancellationTokenSource _cts;
        private string _sha256; // Hash of _path computed on load, reused for the lookup

        private static LocalizationBase Lang => LocalizationHelper.Base;

        public UploadForm(MainForm mainForm, Settings settings, bool reopen, string path) {
            _mainForm = mainForm;
            _settings = settings;
            _reopen   = reopen;

            // Gestisci selezione multipla: "a|b|c" apre una finestra per ogni file
            if (path.Contains("|")) {
                var files = path.Split('|').Select(p => p.Trim()).Where(p => p.Length > 0).ToArray();

                for (var i = 1; i < files.Length; i++)
                    new UploadForm(mainForm, settings, reopen, files[i]).Show();

                // Questa finestra gestisce il primo file
                _path = files.Length > 0 ? files[0] : path;
            }
            else {
                _path = path;
            }

            _isFolder = Directory.Exists(_path);
            InitializeComponent();
        }

        private sealed class UploadException : Exception
        {
            public UploadException(string message) : base(message) { }
        }

        private static string Fmt(string template, params object[] args) {
            // Translations may contain broken placeholders; never let that crash an upload.
            try {
                return string.Format(template, args);
            }
            catch (FormatException) {
                return template + " " + string.Join(" ", args);
            }
        }

        private static string FileLink(string sha256) {
            return $"https://www.virustotal.com/gui/file/{sha256}";
        }

        private void ChangeStatus(string text) {
            if (!IsDisposed)
                statusLabel.Text = text;
        }

        private void DisplayError(string error) {
            using (var messageBox = new DarkMessageBox(error, Lang.UploadForm_Error, DarkMessageBoxIcon.Error, DarkDialogButton.Ok))
                messageBox.ShowDialog(this);
        }

        private bool ValidateApiKey() {
            if (string.IsNullOrEmpty(_settings.ApiKey)) {
                MessageBox.Show(this, Lang.UploadForm_NoApiKey, Lang.UploadForm_InvalidKey, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (_settings.ApiKey.Length != 64) {
                MessageBox.Show(this, Lang.UploadForm_InvalidLength, Lang.UploadForm_InvalidKey, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Starts the upload, or cancels it when one is already running.
        /// Runs on the UI thread; network and disk work is awaited, so no UI call ever happens off-thread.
        /// </summary>
        private async void StartUpload() {
            if (_cts != null) {
                _cts.Cancel();
                return;
            }

            if (!ValidateApiKey())
                return;

            _cts = new CancellationTokenSource();
            uploadButton.Text = Lang.UploadForm_Cancel;
            ChangeStatus(Lang.Message_Init);

            var links     = new List<string>();
            var errors    = new List<string>();
            var cancelled = false;

            try {
                await UploadAsync(links, errors, _cts.Token);
            }
            catch (OperationCanceledException) {
                cancelled = true;
            }
            finally {
                _cts.Dispose();
                _cts = null;
            }

            // The window was closed while uploading
            if (IsDisposed)
                return;

            uploadButton.Text = Lang.UploadForm_Upload;

            if (cancelled) {
                ChangeStatus(Lang.Message_Cancelled);
                return;
            }

            if (links.Count > 0) {
                // Give VirusTotal a moment before the result page is opened
                ChangeStatus(Lang.Message_OpeningResults);
                uploadButton.Enabled = false;
                await Task.Delay(ResultsDelay);

                // Closing the window during the wait counts as "don't open"
                if (IsDisposed)
                    return;

                uploadButton.Enabled = true;
                OpenResults(links);
            }

            if (errors.Count > 0) {
                ChangeStatus(Lang.Message_DoneWithErrors);
                DisplayError(string.Join(Environment.NewLine + Environment.NewLine, errors));
                return;
            }

            ChangeStatus(Lang.Message_Idle);

            // Chiudi finestra dopo completamento upload
            await Task.Delay(500);
            if (!IsDisposed)
                Close();
        }

        private async Task UploadAsync(List<string> links, List<string> errors, CancellationToken token) {
            List<string> files;
            if (_isFolder)
                files = await Task.Run(() => EnumerateFiles(_path, errors), token);
            else
                files = new List<string> { _path };

            for (var i = 0; i < files.Count; i++) {
                token.ThrowIfCancellationRequested();

                var file   = files[i];
                var prefix = files.Count > 1 ? $"[{i + 1}/{files.Count}] " : "";

                try {
                    links.Add(await ProcessFileAsync(file, prefix, token));
                }
                catch (OperationCanceledException) {
                    throw;
                }
                catch (UploadException ex) {
                    errors.Add($"{Path.GetFileName(file)}: {ex.Message}");
                }
                catch (Exception ex) {
                    // Anything unexpected (I/O, malformed JSON...) is reported per file instead of killing the app
                    errors.Add($"{Path.GetFileName(file)}: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Lists every file under <paramref name="root"/>, skipping (and reporting) folders that cannot be read.
        /// </summary>
        private static List<string> EnumerateFiles(string root, List<string> errors) {
            var files   = new List<string>();
            var pending = new Stack<string>();
            pending.Push(root);

            while (pending.Count > 0) {
                var dir = pending.Pop();
                try {
                    files.AddRange(Directory.GetFiles(dir));
                    foreach (var sub in Directory.GetDirectories(dir))
                        pending.Push(sub);
                }
                catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException) {
                    errors.Add(Fmt(Lang.Error_FolderRead, dir, ex.Message));
                }
            }

            files.Sort(StringComparer.OrdinalIgnoreCase);
            return files;
        }

        /// <summary>
        /// Looks the file up by hash and uploads it only when VirusTotal does not know it yet.
        /// Returns the link to the result page.
        /// </summary>
        private async Task<string> ProcessFileAsync(string file, string prefix, CancellationToken token) {
            var fileName = Path.GetFileName(file);
            if (!File.Exists(file))
                throw new UploadException(Fmt(Lang.Error_FileMissing, file));

            ChangeStatus(prefix + Fmt(Lang.Message_CheckFile, fileName));

            var sha256 = (file == _path && _sha256 != null
                ? _sha256
                : await Task.Run(() => Utils.GetSHA256(file), token)).ToLowerInvariant();

            // 200 = already known (no need to upload it again), 404 = never seen
            var report = await CallApiAsync(() => ApiRequest($"api/v3/files/{sha256}", Method.Get), token, allowNotFound: true);
            if (report != null)
                return FileLink(sha256);

            var size = new FileInfo(file).Length;
            if (size > MaxUploadSize)
                throw new UploadException(Lang.Error_TooLarge);

            var uploadUrl = "api/v3/files";
            var retryUpload = true;
            if (size > MaxDirectUploadSize) {
                var urlResponse = await CallApiAsync(() => ApiRequest("api/v3/files/upload_url", Method.Get), token);
                uploadUrl = (string)urlResponse["data"];
                if (!Uri.IsWellFormedUriString(uploadUrl, UriKind.Absolute))
                    throw new UploadException(Lang.Error_InvalidResponse);

                // The upload URL can be used only once, so a rate-limited attempt cannot be repeated with it
                retryUpload = false;
            }

            ChangeStatus(prefix + Fmt(Lang.Message_UploadFile, fileName));

            var analysis = await CallApiAsync(() => {
                var request = uploadUrl.StartsWith("http", StringComparison.OrdinalIgnoreCase)
                    ? new RestRequest(new Uri(uploadUrl), Method.Post)
                    : new RestRequest(uploadUrl, Method.Post);
                request.AddHeader("x-apikey", _settings.ApiKey);
                request.AddFile("file", file);
                request.Timeout = UploadTimeout;
                return request;
            }, token, retryRateLimit: retryUpload);

            // The analysis is queued: the file page shows its progress and results
            if (analysis["data"]?["id"] == null)
                throw new UploadException(Lang.Error_InvalidResponse);

            return FileLink(sha256);
        }

        private RestRequest ApiRequest(string resource, Method method) {
            var request = new RestRequest(resource, method);
            request.AddHeader("x-apikey", _settings.ApiKey);
            return request;
        }

        /// <summary>
        /// Executes a request, waiting and retrying while the API quota is exceeded,
        /// and turns every kind of failure into an <see cref="UploadException"/>.
        /// With <paramref name="allowNotFound"/>, a 404 returns null instead of failing.
        /// </summary>
        private async Task<JObject> CallApiAsync(Func<RestRequest> createRequest, CancellationToken token,
                                                 bool allowNotFound = false, bool retryRateLimit = true) {
            for (var attempt = 0; ; attempt++) {
                var response = await _client.ExecuteAsync(createRequest(), token);
                token.ThrowIfCancellationRequested();

                var status = (int)response.StatusCode;
                var json   = ParseObject(response.Content);

                // QuotaExceededError: the public API allows 4 requests per minute
                if (status == 429) {
                    if (!retryRateLimit || attempt >= RateLimitRetries)
                        throw new UploadException(Lang.Error_RateLimit);

                    var previous = statusLabel.Text;
                    ChangeStatus(Fmt(Lang.Message_RateLimit, (int)RateLimitDelay.TotalSeconds));
                    await Task.Delay(RateLimitDelay, token);
                    ChangeStatus(previous);
                    continue;
                }

                if (status == 404 && allowNotFound)
                    return null;
                if (status == 0)
                    throw new UploadException(Fmt(Lang.Error_Network, response.ErrorMessage ?? response.ResponseStatus.ToString()));
                if (status == 401 || status == 403)
                    throw new UploadException(Lang.Error_Forbidden);
                if (status == 413)
                    throw new UploadException(Lang.Error_TooLarge);
                if (!response.IsSuccessful) {
                    // v3 errors look like {"error": {"code": "...", "message": "..."}}
                    var detail = (string)json?["error"]?["message"] ?? response.StatusDescription;
                    throw new UploadException(Fmt(Lang.Error_Http, $"{status} {detail}"));
                }

                if (json == null)
                    throw new UploadException(Lang.Error_InvalidResponse);

                return json;
            }
        }

        private static JObject ParseObject(string content) {
            if (string.IsNullOrWhiteSpace(content))
                return null;

            try {
                return JToken.Parse(content) as JObject;
            }
            catch (JsonReaderException) {
                return null;
            }
        }

        private void OpenResults(List<string> links) {
            if (links.Count == 0)
                return;

            if (links.Count > MaxTabsWithoutAsking) {
                var answer = MessageBox.Show(this, Fmt(Lang.UploadForm_OpenResults, links.Count), Text,
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (answer != DialogResult.Yes)
                    return;
            }

            foreach (var link in links) {
                try {
                    Process.Start(link);
                }
                catch (Win32Exception ex) {
                    DisplayError(ex.Message);
                    return;
                }
            }
        }

        private async void UploadForm_Load(object sender, EventArgs e) {
            infoLabel.Text    = Lang.UploadForm_Info;
            uploadButton.Text = Lang.UploadForm_Upload;
            statusLabel.Text  = Lang.Message_Idle;
            ActiveControl     = uploadButton;

            if (_isFolder) {
                mdTextbox.Text   = "N/A (Folder)";
                shaTextbox.Text  = "N/A (Folder)";
                sha2Textbox.Text = "N/A (Folder)";
            }
            else {
                // Hashing large files takes a while: keep the window responsive
                try {
                    var hashes = await Task.Run(() => new[] { Utils.GetMD5(_path), Utils.GetSHA1(_path), Utils.GetSHA256(_path) });
                    if (IsDisposed)
                        return;

                    mdTextbox.Text   = hashes[0];
                    shaTextbox.Text  = hashes[1];
                    sha2Textbox.Text = hashes[2];
                    _sha256          = hashes[2];
                }
                catch (Exception ex) {
                    if (IsDisposed)
                        return;

                    mdTextbox.Text   = "ERROR";
                    shaTextbox.Text  = "ERROR";
                    sha2Textbox.Text = "ERROR";
                    DisplayError(Fmt(Lang.UploadForm_HashError, ex.Message));
                }
            }

            if (_settings.DirectUpload)
                StartUpload();
        }

        private void uploadButton_Click(object sender, EventArgs e) {
            StartUpload();
        }

        private void UploadForm_FormClosing(object sender, FormClosingEventArgs e) {
            // Stop a running upload; its continuation sees IsDisposed and exits quietly
            _cts?.Cancel();
        }

        private void UploadForm_FormClosed(object sender, FormClosedEventArgs e) {
            _client.Dispose();

            if (_mainForm.IsDisposed || _mainForm.Disposing)
                return;

            // With several upload windows open, only the last one decides what happens to the main window
            var othersOpen = Application.OpenForms.OfType<UploadForm>().Any(f => f != this && !f.IsDisposed);
            if (othersOpen)
                return;

            if (_reopen)
                _mainForm.Show();
            else
                _mainForm.Close();
        }
    }
}

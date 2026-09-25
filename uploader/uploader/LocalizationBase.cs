using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uploader
{
    class LocalizationBase
    {
        public string MainForm_DragFile = "Drag file here";
        public string MainForm_More = "More";

        public string SettingsForm_Title = "Settings";
        public string SettingsForm_General = "General settings";
        public string SettingsForm_Key = "API key";
        public string SettingsForm_Get = "Get API key";
        public string SettingsForm_Language = "Language";
        public string SettingsForm_Save = "Save";
        public string SettingsForm_Open = "Open settings file";
        public string SettingsForm_DirectUpload = "Direct file upload";

        public string UploadForm_Info = "File information";
        public string UploadForm_Upload = "Upload";
        public string UploadForm_Cancel = "Cancel";
        public string UploadForm_NoApiKey = "You have not entered an API key. Please go to settings and add one.";
        public string UploadForm_InvalidLength = "Invalid API key length. The key must contain 64 characters.";
        public string UploadForm_InvalidKey = "Invalid API key";
        public string UploadForm_Error = "Error";

        public string Message_Idle = "Idle.";
        public string Message_Init = "Initializing...";
        public string Message_Check = "Checking...";
        public string Message_Upload = "Uploading...";
        public string Message_NoLink = "No permalink found in response!";
        public string Message_NoSettings = "No settings file exists.";
        public string Message_Saved = "Settings saved.";

        public string UploadForm_HashError = "Could not compute the file hashes: {0}";
        public string UploadForm_OpenResults = "{0} files processed. Open all results in the browser?";
        public string Message_CheckFile = "Checking {0}...";
        public string Message_UploadFile = "Uploading {0}...";
        public string Message_RateLimit = "API request limit reached, retrying in {0} s...";
        public string Message_Cancelled = "Cancelled.";
        public string Message_DoneWithErrors = "Completed with errors.";
        public string Message_OpeningResults = "Done. Opening the results...";
        public string Error_FileMissing = "File {0} does not exist.";
        public string Error_FolderRead = "Could not read folder {0}: {1}";
        public string Error_TooLarge = "The file is larger than 650 MB, the maximum VirusTotal accepts.";
        public string Error_Network = "Network error: {0}";
        public string Error_Http = "VirusTotal returned HTTP {0}.";
        public string Error_InvalidResponse = "VirusTotal returned an invalid response.";
        public string Error_RateLimit = "The API request limit is still exceeded after several attempts. Try again later.";
        public string Error_Forbidden = "VirusTotal rejected the API key. Check it in the settings.";
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace uploader
{
    public partial class MainForm : ModernForm
    {
        private SettingsForm _settingsForm = new SettingsForm();

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Set working directory to exe location because of language files
            Directory.SetCurrentDirectory(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            //LocalizationHelper.Export();
            
            LocalizationHelper.Update();

            dropZone.Text = LocalizationHelper.Base.MainForm_DragFile;
            moreButton.Text = LocalizationHelper.Base.MainForm_More;
        }

        private void moreLabel_Click(object sender, EventArgs e)
        {
            if (_settingsForm.IsDisposed)
            {
                _settingsForm = new SettingsForm();
            }
            _settingsForm.Show();
        }

        private void MainForm_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;

            e.Effect = DragDropEffects.Copy;
            dropZone.Highlight = true;
        }

        private void MainForm_DragLeave(object sender, EventArgs e)
        {
            dropZone.Highlight = false;
        }

        private void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            dropZone.Highlight = false;
            var settings = Settings.LoadSettings();

            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (var file in files)
            {
                var uploadForm = new UploadForm(this, settings, true, file);
                uploadForm.Show();
                this.Hide();
            }
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            var settings = Settings.LoadSettings();
            var args = Environment.GetCommandLineArgs();

            if (args.Length == 2)
            {
                var file = args[1]; // Second argument because .NET puts program filename to the first
                var uploadForm = new UploadForm(this, settings, false, file);
                uploadForm.Show();
                this.Hide();
            }
        }
    }
}

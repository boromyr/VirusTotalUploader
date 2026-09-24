namespace uploader
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.rootLayout = new System.Windows.Forms.TableLayoutPanel();
            this.dropZone = new uploader.DropZone();
            this.moreButton = new uploader.ModernButton();
            this.rootLayout.SuspendLayout();
            this.SuspendLayout();
            //
            // rootLayout
            //
            this.rootLayout.ColumnCount = 1;
            this.rootLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.rootLayout.Controls.Add(this.dropZone, 0, 0);
            this.rootLayout.Controls.Add(this.moreButton, 0, 1);
            this.rootLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rootLayout.Location = new System.Drawing.Point(16, 16);
            this.rootLayout.Margin = new System.Windows.Forms.Padding(0);
            this.rootLayout.Name = "rootLayout";
            this.rootLayout.RowCount = 2;
            this.rootLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.rootLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 52F));
            this.rootLayout.Size = new System.Drawing.Size(408, 388);
            this.rootLayout.TabIndex = 0;
            //
            // dropZone
            //
            this.dropZone.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dropZone.Font = uploader.Theme.BodyStrong;
            this.dropZone.Margin = new System.Windows.Forms.Padding(0);
            this.dropZone.Name = "dropZone";
            this.dropZone.TabIndex = 0;
            this.dropZone.Text = "Drag file here";
            this.dropZone.DragDrop += new System.Windows.Forms.DragEventHandler(this.MainForm_DragDrop);
            this.dropZone.DragEnter += new System.Windows.Forms.DragEventHandler(this.MainForm_DragEnter);
            this.dropZone.DragLeave += new System.EventHandler(this.MainForm_DragLeave);
            //
            // moreButton
            //
            this.moreButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.moreButton.AutoSize = true;
            this.moreButton.Margin = new System.Windows.Forms.Padding(0);
            this.moreButton.Name = "moreButton";
            this.moreButton.TabIndex = 1;
            this.moreButton.Text = "More";
            this.moreButton.Click += new System.EventHandler(this.moreLabel_Click);
            //
            // MainForm
            //
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(440, 420);
            this.Controls.Add(this.rootLayout);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(360, 340);
            this.Name = "MainForm";
            this.Padding = new System.Windows.Forms.Padding(16);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "VirusTotal Uploader";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.MainForm_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.MainForm_DragEnter);
            this.DragLeave += new System.EventHandler(this.MainForm_DragLeave);
            this.rootLayout.ResumeLayout(false);
            this.rootLayout.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel rootLayout;
        private uploader.DropZone dropZone;
        private uploader.ModernButton moreButton;
    }
}

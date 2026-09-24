namespace uploader
{
    partial class UploadForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UploadForm));
            this.infoCard = new uploader.ModernCard();
            this.cardLayout = new System.Windows.Forms.TableLayoutPanel();
            this.infoLabel = new System.Windows.Forms.Label();
            this.darkLabel1 = new System.Windows.Forms.Label();
            this.mdTextbox = new uploader.ModernTextBox();
            this.darkLabel2 = new System.Windows.Forms.Label();
            this.shaTextbox = new uploader.ModernTextBox();
            this.darkLabel3 = new System.Windows.Forms.Label();
            this.sha2Textbox = new uploader.ModernTextBox();
            this.buttonPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.uploadButton = new uploader.ModernButton();
            this.statusLabel = new System.Windows.Forms.Label();
            this.infoCard.SuspendLayout();
            this.cardLayout.SuspendLayout();
            this.buttonPanel.SuspendLayout();
            this.SuspendLayout();
            //
            // infoCard
            //
            this.infoCard.Controls.Add(this.cardLayout);
            this.infoCard.Location = new System.Drawing.Point(16, 16);
            this.infoCard.Name = "infoCard";
            this.infoCard.Padding = new System.Windows.Forms.Padding(16, 12, 16, 12);
            this.infoCard.Size = new System.Drawing.Size(648, 192);
            this.infoCard.TabIndex = 0;
            //
            // cardLayout
            //
            this.cardLayout.ColumnCount = 2;
            this.cardLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.cardLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.cardLayout.Controls.Add(this.infoLabel, 0, 0);
            this.cardLayout.Controls.Add(this.darkLabel1, 0, 1);
            this.cardLayout.Controls.Add(this.mdTextbox, 1, 1);
            this.cardLayout.Controls.Add(this.darkLabel2, 0, 2);
            this.cardLayout.Controls.Add(this.shaTextbox, 1, 2);
            this.cardLayout.Controls.Add(this.darkLabel3, 0, 3);
            this.cardLayout.Controls.Add(this.sha2Textbox, 1, 3);
            this.cardLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardLayout.Location = new System.Drawing.Point(16, 12);
            this.cardLayout.Margin = new System.Windows.Forms.Padding(0);
            this.cardLayout.Name = "cardLayout";
            this.cardLayout.RowCount = 4;
            this.cardLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.cardLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 44F));
            this.cardLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 44F));
            this.cardLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 44F));
            this.cardLayout.Size = new System.Drawing.Size(616, 168);
            this.cardLayout.TabIndex = 0;
            //
            // infoLabel
            //
            this.infoLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.infoLabel.AutoSize = true;
            this.cardLayout.SetColumnSpan(this.infoLabel, 2);
            this.infoLabel.Font = uploader.Theme.Subtitle;
            this.infoLabel.Margin = new System.Windows.Forms.Padding(0);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.TabIndex = 0;
            this.infoLabel.Text = "File information";
            //
            // darkLabel1
            //
            this.darkLabel1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.darkLabel1.AutoSize = true;
            this.darkLabel1.ForeColor = uploader.Theme.TextSecondary;
            this.darkLabel1.Margin = new System.Windows.Forms.Padding(0, 0, 16, 0);
            this.darkLabel1.Name = "darkLabel1";
            this.darkLabel1.TabIndex = 1;
            this.darkLabel1.Text = "MD5";
            //
            // mdTextbox
            //
            this.mdTextbox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.mdTextbox.Font = uploader.Theme.Mono;
            this.mdTextbox.Margin = new System.Windows.Forms.Padding(0);
            this.mdTextbox.Name = "mdTextbox";
            this.mdTextbox.ReadOnly = true;
            this.mdTextbox.Size = new System.Drawing.Size(540, 32);
            this.mdTextbox.TabIndex = 2;
            //
            // darkLabel2
            //
            this.darkLabel2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.darkLabel2.AutoSize = true;
            this.darkLabel2.ForeColor = uploader.Theme.TextSecondary;
            this.darkLabel2.Margin = new System.Windows.Forms.Padding(0, 0, 16, 0);
            this.darkLabel2.Name = "darkLabel2";
            this.darkLabel2.TabIndex = 3;
            this.darkLabel2.Text = "SHA1";
            //
            // shaTextbox
            //
            this.shaTextbox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.shaTextbox.Font = uploader.Theme.Mono;
            this.shaTextbox.Margin = new System.Windows.Forms.Padding(0);
            this.shaTextbox.Name = "shaTextbox";
            this.shaTextbox.ReadOnly = true;
            this.shaTextbox.Size = new System.Drawing.Size(540, 32);
            this.shaTextbox.TabIndex = 4;
            //
            // darkLabel3
            //
            this.darkLabel3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.darkLabel3.AutoSize = true;
            this.darkLabel3.ForeColor = uploader.Theme.TextSecondary;
            this.darkLabel3.Margin = new System.Windows.Forms.Padding(0, 0, 16, 0);
            this.darkLabel3.Name = "darkLabel3";
            this.darkLabel3.TabIndex = 5;
            this.darkLabel3.Text = "SHA256";
            //
            // sha2Textbox
            //
            this.sha2Textbox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.sha2Textbox.Font = uploader.Theme.Mono;
            this.sha2Textbox.Margin = new System.Windows.Forms.Padding(0);
            this.sha2Textbox.Name = "sha2Textbox";
            this.sha2Textbox.ReadOnly = true;
            this.sha2Textbox.Size = new System.Drawing.Size(540, 32);
            this.sha2Textbox.TabIndex = 6;
            //
            // buttonPanel
            //
            this.buttonPanel.Controls.Add(this.uploadButton);
            this.buttonPanel.Controls.Add(this.statusLabel);
            this.buttonPanel.Location = new System.Drawing.Point(16, 224);
            this.buttonPanel.Margin = new System.Windows.Forms.Padding(0);
            this.buttonPanel.Name = "buttonPanel";
            this.buttonPanel.Size = new System.Drawing.Size(648, 36);
            this.buttonPanel.TabIndex = 1;
            this.buttonPanel.WrapContents = false;
            //
            // uploadButton
            //
            this.uploadButton.Accent = true;
            this.uploadButton.Margin = new System.Windows.Forms.Padding(0, 0, 16, 0);
            this.uploadButton.Name = "uploadButton";
            this.uploadButton.Size = new System.Drawing.Size(140, 32);
            this.uploadButton.TabIndex = 0;
            this.uploadButton.Text = "Upload";
            this.uploadButton.Click += new System.EventHandler(this.uploadButton_Click);
            //
            // statusLabel
            //
            this.statusLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.statusLabel.AutoEllipsis = true;
            this.statusLabel.ForeColor = uploader.Theme.TextSecondary;
            this.statusLabel.Margin = new System.Windows.Forms.Padding(0);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(480, 32);
            this.statusLabel.TabIndex = 1;
            this.statusLabel.Text = "Idle.";
            this.statusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // UploadForm
            //
            this.AcceptButton = this.uploadButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(680, 276);
            this.Controls.Add(this.buttonPanel);
            this.Controls.Add(this.infoCard);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "UploadForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "VirusTotal Uploader";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.UploadForm_FormClosing);
            this.Load += new System.EventHandler(this.UploadForm_Load);
            this.infoCard.ResumeLayout(false);
            this.cardLayout.ResumeLayout(false);
            this.cardLayout.PerformLayout();
            this.buttonPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private uploader.ModernCard infoCard;
        private System.Windows.Forms.TableLayoutPanel cardLayout;
        private System.Windows.Forms.Label infoLabel;
        private uploader.ModernButton uploadButton;
        private System.Windows.Forms.FlowLayoutPanel buttonPanel;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.Label darkLabel1;
        private uploader.ModernTextBox sha2Textbox;
        private System.Windows.Forms.Label darkLabel3;
        private uploader.ModernTextBox shaTextbox;
        private System.Windows.Forms.Label darkLabel2;
        private uploader.ModernTextBox mdTextbox;
    }
}

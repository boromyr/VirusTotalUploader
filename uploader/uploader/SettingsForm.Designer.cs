namespace uploader
{
    partial class SettingsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            this.generalCard = new uploader.ModernCard();
            this.cardLayout = new System.Windows.Forms.TableLayoutPanel();
            this.generalLabel = new System.Windows.Forms.Label();
            this.apiLabel = new System.Windows.Forms.Label();
            this.apiTextbox = new uploader.ModernTextBox();
            this.getApiButton = new uploader.ModernButton();
            this.languageLabel = new System.Windows.Forms.Label();
            this.languageCombo = new uploader.ModernComboBox();
            this.directCheckbox = new uploader.ModernCheckBox();
            this.buttonPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.saveButton = new uploader.ModernButton();
            this.openButton = new uploader.ModernButton();
            this.statusLabel = new System.Windows.Forms.Label();
            this.generalCard.SuspendLayout();
            this.cardLayout.SuspendLayout();
            this.buttonPanel.SuspendLayout();
            this.SuspendLayout();
            //
            // generalCard
            //
            this.generalCard.Controls.Add(this.cardLayout);
            this.generalCard.Location = new System.Drawing.Point(16, 16);
            this.generalCard.Name = "generalCard";
            this.generalCard.Padding = new System.Windows.Forms.Padding(16, 12, 16, 12);
            this.generalCard.Size = new System.Drawing.Size(528, 232);
            this.generalCard.TabIndex = 0;
            //
            // cardLayout
            //
            this.cardLayout.ColumnCount = 2;
            this.cardLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.cardLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.cardLayout.Controls.Add(this.generalLabel, 0, 0);
            this.cardLayout.Controls.Add(this.apiLabel, 0, 1);
            this.cardLayout.Controls.Add(this.apiTextbox, 1, 1);
            this.cardLayout.Controls.Add(this.getApiButton, 1, 2);
            this.cardLayout.Controls.Add(this.languageLabel, 0, 3);
            this.cardLayout.Controls.Add(this.languageCombo, 1, 3);
            this.cardLayout.Controls.Add(this.directCheckbox, 0, 4);
            this.cardLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardLayout.Location = new System.Drawing.Point(16, 12);
            this.cardLayout.Margin = new System.Windows.Forms.Padding(0);
            this.cardLayout.Name = "cardLayout";
            this.cardLayout.RowCount = 5;
            this.cardLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.cardLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 44F));
            this.cardLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 44F));
            this.cardLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 44F));
            this.cardLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.cardLayout.Size = new System.Drawing.Size(496, 208);
            this.cardLayout.TabIndex = 0;
            //
            // generalLabel
            //
            this.generalLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.generalLabel.AutoSize = true;
            this.cardLayout.SetColumnSpan(this.generalLabel, 2);
            this.generalLabel.Font = uploader.Theme.Subtitle;
            this.generalLabel.Margin = new System.Windows.Forms.Padding(0);
            this.generalLabel.Name = "generalLabel";
            this.generalLabel.TabIndex = 0;
            this.generalLabel.Text = "General settings";
            //
            // apiLabel
            //
            this.apiLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.apiLabel.AutoSize = true;
            this.apiLabel.Margin = new System.Windows.Forms.Padding(0, 0, 16, 0);
            this.apiLabel.Name = "apiLabel";
            this.apiLabel.TabIndex = 1;
            this.apiLabel.Text = "API key";
            //
            // apiTextbox
            //
            this.apiTextbox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.apiTextbox.Margin = new System.Windows.Forms.Padding(0);
            this.apiTextbox.Name = "apiTextbox";
            this.apiTextbox.Size = new System.Drawing.Size(380, 32);
            this.apiTextbox.TabIndex = 2;
            //
            // getApiButton
            //
            this.getApiButton.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.getApiButton.AutoSize = true;
            this.getApiButton.Margin = new System.Windows.Forms.Padding(0);
            this.getApiButton.Name = "getApiButton";
            this.getApiButton.TabIndex = 3;
            this.getApiButton.Text = "Get API key";
            this.getApiButton.Click += new System.EventHandler(this.getApiButton_Click);
            //
            // languageLabel
            //
            this.languageLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.languageLabel.AutoSize = true;
            this.languageLabel.Margin = new System.Windows.Forms.Padding(0, 0, 16, 0);
            this.languageLabel.Name = "languageLabel";
            this.languageLabel.TabIndex = 4;
            this.languageLabel.Text = "Language";
            //
            // languageCombo
            //
            this.languageCombo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.languageCombo.Margin = new System.Windows.Forms.Padding(0);
            this.languageCombo.Name = "languageCombo";
            this.languageCombo.Size = new System.Drawing.Size(380, 32);
            this.languageCombo.TabIndex = 5;
            //
            // directCheckbox
            //
            this.directCheckbox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.directCheckbox.AutoSize = true;
            this.cardLayout.SetColumnSpan(this.directCheckbox, 2);
            this.directCheckbox.Margin = new System.Windows.Forms.Padding(0);
            this.directCheckbox.Name = "directCheckbox";
            this.directCheckbox.TabIndex = 6;
            this.directCheckbox.Text = "Direct file upload";
            //
            // buttonPanel
            //
            this.buttonPanel.Controls.Add(this.saveButton);
            this.buttonPanel.Controls.Add(this.openButton);
            this.buttonPanel.Controls.Add(this.statusLabel);
            this.buttonPanel.Location = new System.Drawing.Point(16, 264);
            this.buttonPanel.Margin = new System.Windows.Forms.Padding(0);
            this.buttonPanel.Name = "buttonPanel";
            this.buttonPanel.Size = new System.Drawing.Size(528, 36);
            this.buttonPanel.TabIndex = 1;
            this.buttonPanel.WrapContents = false;
            //
            // saveButton
            //
            this.saveButton.Accent = true;
            this.saveButton.AutoSize = true;
            this.saveButton.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.saveButton.Name = "saveButton";
            this.saveButton.TabIndex = 0;
            this.saveButton.Text = "Save";
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            //
            // openButton
            //
            this.openButton.AutoSize = true;
            this.openButton.Margin = new System.Windows.Forms.Padding(0, 0, 12, 0);
            this.openButton.Name = "openButton";
            this.openButton.TabIndex = 1;
            this.openButton.Text = "Open settings file";
            this.openButton.Click += new System.EventHandler(this.darkButton1_Click);
            //
            // statusLabel
            //
            this.statusLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.statusLabel.AutoSize = true;
            this.statusLabel.ForeColor = uploader.Theme.TextSecondary;
            this.statusLabel.Margin = new System.Windows.Forms.Padding(0);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.TabIndex = 2;
            //
            // SettingsForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(560, 316);
            this.Controls.Add(this.buttonPanel);
            this.Controls.Add(this.generalCard);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.SettingsForm_Load);
            this.generalCard.ResumeLayout(false);
            this.cardLayout.ResumeLayout(false);
            this.cardLayout.PerformLayout();
            this.buttonPanel.ResumeLayout(false);
            this.buttonPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private uploader.ModernCard generalCard;
        private System.Windows.Forms.TableLayoutPanel cardLayout;
        private System.Windows.Forms.Label generalLabel;
        private uploader.ModernTextBox apiTextbox;
        private System.Windows.Forms.Label apiLabel;
        private uploader.ModernButton getApiButton;
        private System.Windows.Forms.Label languageLabel;
        private uploader.ModernComboBox languageCombo;
        private System.Windows.Forms.FlowLayoutPanel buttonPanel;
        private uploader.ModernButton saveButton;
        private uploader.ModernButton openButton;
        private System.Windows.Forms.Label statusLabel;
        private uploader.ModernCheckBox directCheckbox;
    }
}

namespace Keiser.M3i.ReceiverDebug
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
            receiver.stop();
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
            this.bottomStatusStrip = new System.Windows.Forms.StatusStrip();
            this.statusBarCurrentState = new System.Windows.Forms.ToolStripStatusLabel();
            this.errorStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusBarClearButton = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusBarSaveButton = new System.Windows.Forms.ToolStripStatusLabel();
            this.mainControlPanel = new System.Windows.Forms.Panel();
            this.versionLabel = new System.Windows.Forms.Label();
            this.versionBox = new System.Windows.Forms.ComboBox();
            this.threadToggleButton = new System.Windows.Forms.Button();
            this.portBox = new System.Windows.Forms.TextBox();
            this.portBoxLabel = new System.Windows.Forms.Label();
            this.ipAddressBox = new System.Windows.Forms.TextBox();
            this.ipAddressLabel = new System.Windows.Forms.Label();
            this.outputBox = new System.Windows.Forms.ListBox();
            this.bottomStatusStrip.SuspendLayout();
            this.mainControlPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // bottomStatusStrip
            // 
            this.bottomStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusBarCurrentState,
            this.errorStatus,
            this.statusBarClearButton,
            this.statusBarSaveButton});
            this.bottomStatusStrip.Location = new System.Drawing.Point(0, 274);
            this.bottomStatusStrip.Name = "bottomStatusStrip";
            this.bottomStatusStrip.Size = new System.Drawing.Size(534, 22);
            this.bottomStatusStrip.SizingGrip = false;
            this.bottomStatusStrip.TabIndex = 0;
            this.bottomStatusStrip.Text = "bottomStatusStrip";
            // 
            // statusBarCurrentState
            // 
            this.statusBarCurrentState.AutoSize = false;
            this.statusBarCurrentState.BackColor = System.Drawing.SystemColors.Control;
            this.statusBarCurrentState.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.statusBarCurrentState.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusBarCurrentState.ForeColor = System.Drawing.Color.White;
            this.statusBarCurrentState.Margin = new System.Windows.Forms.Padding(2, 3, 0, 2);
            this.statusBarCurrentState.Name = "statusBarCurrentState";
            this.statusBarCurrentState.Size = new System.Drawing.Size(65, 17);
            // 
            // errorStatus
            // 
            this.errorStatus.AutoSize = false;
            this.errorStatus.BackColor = System.Drawing.SystemColors.Control;
            this.errorStatus.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.errorStatus.ForeColor = System.Drawing.Color.Red;
            this.errorStatus.Name = "errorStatus";
            this.errorStatus.Padding = new System.Windows.Forms.Padding(12, 0, 12, 0);
            this.errorStatus.Size = new System.Drawing.Size(424, 17);
            this.errorStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // statusBarClearButton
            // 
            this.statusBarClearButton.AutoSize = false;
            this.statusBarClearButton.BackColor = System.Drawing.SystemColors.Control;
            this.statusBarClearButton.BackgroundImage = global::Keiser.M3i.ReceiverDebug.Properties.Resources.action_Cancel_16xLG;
            this.statusBarClearButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.statusBarClearButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.statusBarClearButton.Margin = new System.Windows.Forms.Padding(2, 3, 2, 2);
            this.statusBarClearButton.Name = "statusBarClearButton";
            this.statusBarClearButton.Size = new System.Drawing.Size(17, 17);
            this.statusBarClearButton.Text = "statusBarClearButton";
            this.statusBarClearButton.Click += new System.EventHandler(this.statusBarClearButton_Click);
            // 
            // statusBarSaveButton
            // 
            this.statusBarSaveButton.AutoSize = false;
            this.statusBarSaveButton.BackColor = System.Drawing.SystemColors.Control;
            this.statusBarSaveButton.BackgroundImage = global::Keiser.M3i.ReceiverDebug.Properties.Resources.save_16xLG;
            this.statusBarSaveButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.statusBarSaveButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.statusBarSaveButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.statusBarSaveButton.Margin = new System.Windows.Forms.Padding(2, 3, 2, 2);
            this.statusBarSaveButton.Name = "statusBarSaveButton";
            this.statusBarSaveButton.Size = new System.Drawing.Size(16, 17);
            this.statusBarSaveButton.Text = "statusBarSaveButton";
            this.statusBarSaveButton.Click += new System.EventHandler(this.statusBarSaveButton_Click);
            // 
            // mainControlPanel
            // 
            this.mainControlPanel.Controls.Add(this.versionLabel);
            this.mainControlPanel.Controls.Add(this.versionBox);
            this.mainControlPanel.Controls.Add(this.threadToggleButton);
            this.mainControlPanel.Controls.Add(this.portBox);
            this.mainControlPanel.Controls.Add(this.portBoxLabel);
            this.mainControlPanel.Controls.Add(this.ipAddressBox);
            this.mainControlPanel.Controls.Add(this.ipAddressLabel);
            this.mainControlPanel.Location = new System.Drawing.Point(0, -1);
            this.mainControlPanel.Name = "mainControlPanel";
            this.mainControlPanel.Size = new System.Drawing.Size(534, 34);
            this.mainControlPanel.TabIndex = 1;
            // 
            // versionLabel
            // 
            this.versionLabel.AutoSize = true;
            this.versionLabel.Location = new System.Drawing.Point(323, 10);
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.Size = new System.Drawing.Size(62, 13);
            this.versionLabel.TabIndex = 0;
            this.versionLabel.Text = "API Version";
            // 
            // versionBox
            // 
            this.versionBox.FormattingEnabled = true;
            this.versionBox.Items.AddRange(new object[] {
            "1.0+",
            "0.8"});
            this.versionBox.Location = new System.Drawing.Point(389, 6);
            this.versionBox.Name = "versionBox";
            this.versionBox.Size = new System.Drawing.Size(61, 21);
            this.versionBox.TabIndex = 4;
            this.versionBox.Text = "1.0+";
            // 
            // threadToggleButton
            // 
            this.threadToggleButton.Location = new System.Drawing.Point(456, 5);
            this.threadToggleButton.Name = "threadToggleButton";
            this.threadToggleButton.Size = new System.Drawing.Size(75, 23);
            this.threadToggleButton.TabIndex = 3;
            this.threadToggleButton.Text = "threadToggleButton";
            this.threadToggleButton.UseVisualStyleBackColor = true;
            this.threadToggleButton.Click += new System.EventHandler(this.threadToggleButton_Click);
            // 
            // portBox
            // 
            this.portBox.Location = new System.Drawing.Point(275, 7);
            this.portBox.Name = "portBox";
            this.portBox.Size = new System.Drawing.Size(42, 20);
            this.portBox.TabIndex = 2;
            this.portBox.Text = "35680";
            this.portBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.portBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.portBox_KeyPress);
            this.portBox.Validating += new System.ComponentModel.CancelEventHandler(this.portBox_Validating);
            // 
            // portBoxLabel
            // 
            this.portBoxLabel.AutoSize = true;
            this.portBoxLabel.Location = new System.Drawing.Point(201, 10);
            this.portBoxLabel.Name = "portBoxLabel";
            this.portBoxLabel.Size = new System.Drawing.Size(77, 13);
            this.portBoxLabel.TabIndex = 0;
            this.portBoxLabel.Text = "Broadcast Port";
            // 
            // ipAddressBox
            // 
            this.ipAddressBox.Location = new System.Drawing.Point(109, 7);
            this.ipAddressBox.Name = "ipAddressBox";
            this.ipAddressBox.Size = new System.Drawing.Size(86, 20);
            this.ipAddressBox.TabIndex = 1;
            this.ipAddressBox.Text = "239.10.10.10";
            this.ipAddressBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.ipAddressBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ipAddressBox_KeyPress);
            this.ipAddressBox.Validating += new System.ComponentModel.CancelEventHandler(this.ipAddressBox_Validating);
            // 
            // ipAddressLabel
            // 
            this.ipAddressLabel.AutoSize = true;
            this.ipAddressLabel.Location = new System.Drawing.Point(3, 10);
            this.ipAddressLabel.Name = "ipAddressLabel";
            this.ipAddressLabel.Size = new System.Drawing.Size(109, 13);
            this.ipAddressLabel.TabIndex = 0;
            this.ipAddressLabel.Text = "Broadcast IP Address";
            // 
            // outputBox
            // 
            this.outputBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.outputBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.outputBox.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.outputBox.FormattingEnabled = true;
            this.outputBox.ItemHeight = 11;
            this.outputBox.Location = new System.Drawing.Point(0, 32);
            this.outputBox.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.outputBox.MaximumSize = new System.Drawing.Size(534, 1000);
            this.outputBox.MinimumSize = new System.Drawing.Size(534, 242);
            this.outputBox.Name = "outputBox";
            this.outputBox.Size = new System.Drawing.Size(534, 242);
            this.outputBox.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(534, 296);
            this.Controls.Add(this.outputBox);
            this.Controls.Add(this.mainControlPanel);
            this.Controls.Add(this.bottomStatusStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(550, 1000);
            this.MinimumSize = new System.Drawing.Size(550, 335);
            this.Name = "MainForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "M3 Relay Debug";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.bottomStatusStrip.ResumeLayout(false);
            this.bottomStatusStrip.PerformLayout();
            this.mainControlPanel.ResumeLayout(false);
            this.mainControlPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip bottomStatusStrip;
        private System.Windows.Forms.Panel mainControlPanel;
        private System.Windows.Forms.Label ipAddressLabel;
        private System.Windows.Forms.TextBox ipAddressBox;
        private System.Windows.Forms.Label portBoxLabel;
        private System.Windows.Forms.TextBox portBox;
        private System.Windows.Forms.ToolStripStatusLabel statusBarCurrentState;
        private System.Windows.Forms.ToolStripStatusLabel errorStatus;
        private System.Windows.Forms.Button threadToggleButton;
        private System.Windows.Forms.ListBox outputBox;
        private System.Windows.Forms.ToolStripStatusLabel statusBarSaveButton;
        private System.Windows.Forms.ToolStripStatusLabel statusBarClearButton;
        private System.Windows.Forms.Label versionLabel;
        private System.Windows.Forms.ComboBox versionBox;
    }
}


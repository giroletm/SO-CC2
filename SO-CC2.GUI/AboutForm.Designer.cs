using System.Windows.Forms;

namespace SO_CC2.GUI
{
    partial class AboutForm
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
            firstLinkLabel = new LinkLabel();
            licenseLinkLabel = new LinkLabel();
            repoLinkLabel = new LinkLabel();
            closeButton = new Button();
            SuspendLayout();
            // 
            // firstLinkLabel
            // 
            firstLinkLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            firstLinkLabel.LinkArea = new LinkArea(0, 0);
            firstLinkLabel.Location = new Point(12, 9);
            firstLinkLabel.Name = "firstLinkLabel";
            firstLinkLabel.Size = new Size(626, 21);
            firstLinkLabel.TabIndex = 1;
            firstLinkLabel.Text = "This program is RedStoneMatt's entry to Stack Overflow's Code Challenge #2: Secret messages in game boards.";
            firstLinkLabel.TextAlign = ContentAlignment.MiddleCenter;
            firstLinkLabel.UseCompatibleTextRendering = true;
            firstLinkLabel.LinkClicked += linkLabel_LinkClicked;
            // 
            // licenseLinkLabel
            // 
            licenseLinkLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            licenseLinkLabel.AutoSize = true;
            licenseLinkLabel.LinkArea = new LinkArea(0, 0);
            licenseLinkLabel.Location = new Point(477, 52);
            licenseLinkLabel.Name = "licenseLinkLabel";
            licenseLinkLabel.Size = new Size(161, 21);
            licenseLinkLabel.TabIndex = 2;
            licenseLinkLabel.Text = "Licensed under CC BY-SA 4.0";
            licenseLinkLabel.UseCompatibleTextRendering = true;
            licenseLinkLabel.LinkClicked += linkLabel_LinkClicked;
            // 
            // repoLinkLabel
            // 
            repoLinkLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            repoLinkLabel.AutoSize = true;
            repoLinkLabel.LinkArea = new LinkArea(0, 0);
            repoLinkLabel.Location = new Point(12, 52);
            repoLinkLabel.Name = "repoLinkLabel";
            repoLinkLabel.Size = new Size(102, 21);
            repoLinkLabel.TabIndex = 3;
            repoLinkLabel.Text = "GitHub repository";
            repoLinkLabel.UseCompatibleTextRendering = true;
            repoLinkLabel.LinkClicked += linkLabel_LinkClicked;
            // 
            // closeButton
            // 
            closeButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            closeButton.DialogResult = DialogResult.OK;
            closeButton.Location = new Point(12, 76);
            closeButton.Name = "closeButton";
            closeButton.Size = new Size(626, 23);
            closeButton.TabIndex = 4;
            closeButton.Text = "Close";
            closeButton.UseVisualStyleBackColor = true;
            // 
            // AboutForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(650, 111);
            Controls.Add(closeButton);
            Controls.Add(repoLinkLabel);
            Controls.Add(licenseLinkLabel);
            Controls.Add(firstLinkLabel);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "AboutForm";
            ShowIcon = false;
            Text = "About";
            Load += AboutForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private LinkLabel firstLinkLabel;
        private LinkLabel licenseLinkLabel;
        private LinkLabel repoLinkLabel;
        private Button closeButton;
    }
}
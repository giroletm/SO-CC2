namespace SO_CC2
{
    partial class Main
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            rightGroupBox = new GroupBox();
            codeTextBox = new TextBox();
            mainMenuStrip = new MenuStrip();
            helpToolStripMenuItem = new ToolStripMenuItem();
            aboutToolStripMenuItem = new ToolStripMenuItem();
            boardTableLayoutPanel = new TableLayoutPanel();
            playersPictureBox = new PictureBox();
            rightGroupBox.SuspendLayout();
            mainMenuStrip.SuspendLayout();
            boardTableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)playersPictureBox).BeginInit();
            SuspendLayout();
            // 
            // rightGroupBox
            // 
            rightGroupBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            rightGroupBox.Controls.Add(codeTextBox);
            rightGroupBox.Location = new Point(540, 27);
            rightGroupBox.Name = "rightGroupBox";
            rightGroupBox.Size = new Size(200, 434);
            rightGroupBox.TabIndex = 1;
            rightGroupBox.TabStop = false;
            rightGroupBox.Text = "Data";
            // 
            // codeTextBox
            // 
            codeTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            codeTextBox.Location = new Point(6, 94);
            codeTextBox.MaxLength = 8;
            codeTextBox.Name = "codeTextBox";
            codeTextBox.Size = new Size(188, 23);
            codeTextBox.TabIndex = 0;
            codeTextBox.TextChanged += codeTextBox_TextChanged;
            // 
            // mainMenuStrip
            // 
            mainMenuStrip.Items.AddRange(new ToolStripItem[] { helpToolStripMenuItem });
            mainMenuStrip.Location = new Point(0, 0);
            mainMenuStrip.Name = "mainMenuStrip";
            mainMenuStrip.Size = new Size(752, 24);
            mainMenuStrip.TabIndex = 2;
            mainMenuStrip.Text = "menuStrip1";
            // 
            // helpToolStripMenuItem
            // 
            helpToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { aboutToolStripMenuItem });
            helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            helpToolStripMenuItem.Size = new Size(44, 20);
            helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            aboutToolStripMenuItem.Size = new Size(107, 22);
            aboutToolStripMenuItem.Text = "About";
            // 
            // boardTableLayoutPanel
            // 
            boardTableLayoutPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            boardTableLayoutPanel.BackColor = Color.Transparent;
            boardTableLayoutPanel.BackgroundImage = Properties.Resources.Board;
            boardTableLayoutPanel.BackgroundImageLayout = ImageLayout.Zoom;
            boardTableLayoutPanel.ColumnCount = 3;
            boardTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            boardTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            boardTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            boardTableLayoutPanel.Controls.Add(playersPictureBox, 1, 1);
            boardTableLayoutPanel.Location = new Point(12, 27);
            boardTableLayoutPanel.Name = "boardTableLayoutPanel";
            boardTableLayoutPanel.RowCount = 3;
            boardTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
            boardTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
            boardTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
            boardTableLayoutPanel.Size = new Size(434, 434);
            boardTableLayoutPanel.TabIndex = 3;
            boardTableLayoutPanel.CellPaint += boardTableLayoutPanel_CellPaint;
            // 
            // playersPictureBox
            // 
            playersPictureBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            playersPictureBox.BackColor = Color.Transparent;
            playersPictureBox.Location = new Point(147, 147);
            playersPictureBox.Name = "playersPictureBox";
            playersPictureBox.Size = new Size(138, 138);
            playersPictureBox.TabIndex = 0;
            playersPictureBox.TabStop = false;
            // 
            // Main
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(752, 473);
            Controls.Add(boardTableLayoutPanel);
            Controls.Add(rightGroupBox);
            Controls.Add(mainMenuStrip);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = mainMenuStrip;
            MinimumSize = new Size(768, 512);
            Name = "Main";
            Text = "Stack Overflow - Code Challenge #2: Secret messages in game boards";
            Load += Main_Load;
            Resize += Main_Resize;
            rightGroupBox.ResumeLayout(false);
            rightGroupBox.PerformLayout();
            mainMenuStrip.ResumeLayout(false);
            mainMenuStrip.PerformLayout();
            boardTableLayoutPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)playersPictureBox).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private GroupBox rightGroupBox;
        private TextBox codeTextBox;
        private MenuStrip mainMenuStrip;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private TableLayoutPanel boardTableLayoutPanel;
        private PictureBox playersPictureBox;
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SO_CC2
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Called when the Form loads
        /// </summary>
        private void AboutForm_Load(object sender, EventArgs e)
        {
            // LinkLabels can contain multiple links only if added here.
            // They can't be added to the Designer, and if added manually to its .cs file, it will be removed once changes are made on the Designer.

            firstLinkLabel.Text = "This program is RedStoneMatt's entry to Stack Overflow's Code Challenge #2: Secret messages in game boards.";
            firstLinkLabel.Links.Clear();
            firstLinkLabel.Links.Add(16, 12, "https://stackoverflow.com/users/9399492/redstonematt");
            firstLinkLabel.Links.Add(40, 66, "https://stackoverflow.com/beta/challenges/79651567/code-challenge-2-secret-messages-in-game-boards");

            licenseLinkLabel.Text = "Licensed under CC BY-SA 4.0";
            licenseLinkLabel.Links.Clear();
            licenseLinkLabel.Links.Add(15, 12, "https://github.com/giroletm/SO-CC2/blob/master/LICENSE");

            repoLinkLabel.Text = "GitHub repository";
            repoLinkLabel.Links.Clear();
            repoLinkLabel.Links.Add(0, repoLinkLabel.Text.Length, "https://github.com/giroletm/SO-CC2");
        }

        /// <summary>
        /// Called each time a <see cref="LinkLabel.Link"/> is clicked on a <see cref="LinkLabel"/>
        /// </summary>
        private void linkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if(e.Link?.LinkData is string link)
                Process.Start(new ProcessStartInfo(link) { UseShellExecute = true });
        }
    }
}

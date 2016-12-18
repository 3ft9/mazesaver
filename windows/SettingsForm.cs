using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MazeSaver
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            InitializeComponent();

            boxSizeTextBox.Text = Settings.BoxSize.ToString();
            restartDelayTextBox.Text = Settings.RestartDelay.ToString();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            Settings.BoxSize = Int32.Parse(boxSizeTextBox.Text);
            Settings.RestartDelay = Int32.Parse(restartDelayTextBox.Text);
            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}

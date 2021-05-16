using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TaskAllocationGenerator.Forms;
using TaskAllocationUtils.Files;

namespace TaskAllocationGenerator
{
    public partial class TaskAllocationForm : Form
    {
        // AutoResetEvent for waiting
        System.Threading.AutoResetEvent autoResetEvent = new System.Threading.AutoResetEvent(false);


        public TaskAllocationForm()
        {
            InitializeComponent();
        }

        private void ExitToolStripMenuItemClick(object sender, EventArgs e)
        {
            Dispose();
        }

        private void AboutToolStripMenuItemClick(object sender, EventArgs e)
        {
            AboutBoxForm aboutBox = new AboutBoxForm();
            aboutBox.ShowDialog();
        }

        private void GeneratorButtonClick(object sender, EventArgs e)
        {
            // Pre process
            ConfigurationFile configurationFile = new ConfigurationFile(urlTextBox.Text);


            // In Process
            webBrowser.DocumentText = "Finding the optimal task allocations is in process...";

            configurationFile.ReadAndExtractData();
            Console.WriteLine(configurationFile.LocalCommunicationInfo);

            // Post process
        }
    }
}

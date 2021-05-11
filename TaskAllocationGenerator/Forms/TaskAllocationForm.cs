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
using TaskAllocationGenerator.Utils.Files;

namespace TaskAllocationGenerator
{
    public partial class TaskAllocationForm : Form
    {
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
            ConfigurationFile configurationFile = new ConfigurationFile(urlTextBox.Text);
            configurationFile.ReadAndExtractData();
        }
    }
}

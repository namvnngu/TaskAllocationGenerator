using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TaskAllocationGenerator.Forms;
using TaskAllocationUtils.Files;
using TaskAllocationUtils.Constants;
using TaskAllocationGenerator.Utils.Asynchronous;

namespace TaskAllocationGenerator
{
    public partial class TaskAllocationForm : Form
    {
        // AutoResetEvent for waiting
        System.Threading.AutoResetEvent autoResetEvent = new System.Threading.AutoResetEvent(false);
        readonly object ALock = new object();


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

        private async void GeneratorButtonClick(object sender, EventArgs e)
        {
            /// Pre Process
            ConfigurationFile configurationFile = new ConfigurationFile(urlComboBox.Text);
            AsyncHandler asyncHandler = new AsyncHandler();
            System.Threading.SynchronizationContext syncContext = System.Threading.SynchronizationContext.Current;

            /// In Process
            webBrowser.DocumentText = "Finding the optimal task allocations is in process...";

            asyncHandler.Configuration = configurationFile.ReadAndExtractData();
            asyncHandler.AutoResetEvent = autoResetEvent;

            // Create 2nd thread
            await System.Threading.Tasks.Task.Run(() => asyncHandler.SendAsyncRequests());

            // GUI thread waits
            autoResetEvent.WaitOne(AsyncCall.TIMEOUT_LIMIT);

            /// Post Process
            // Process all results that return within 5 mins
            string text = "";

            lock (ALock)
            {
                List<string> results = asyncHandler.Results;
                foreach (string result in results)
                {
                    text += result + Environment.NewLine;
                    Console.WriteLine(result);
                }

            }

            webBrowser.DocumentText = text;
        }
    }
}

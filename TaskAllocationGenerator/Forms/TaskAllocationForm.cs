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
using TaskAllocationUtils.Classes;
using TaskAllocationUtils.Constants;
using TaskAllocationGenerator.Utils.Asynchronous;
using TaskAllocationGenerator.Utils.Allocations;

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
            /*
            /// Pre Process
            string textUrl = urlComboBox.Text;
            ConfigurationFile configurationFile = new ConfigurationFile(textUrl);
            AsyncHandler asyncHandler = new AsyncHandler();

            configurationFile = configurationFile.ReadAndExtractData();
            asyncHandler.Url = textUrl;
            asyncHandler.AutoResetEvent = autoResetEvent;
            System.Threading.SynchronizationContext syncContext = System.Threading.SynchronizationContext.Current;

            /// In Process
            webBrowser.DocumentText = "Finding the optimal task allocations is in process...";

            // Create 2nd thread
            await System.Threading.Tasks.Task.Run(() => asyncHandler.SendAsyncRequests());

            // GUI thread waits
            autoResetEvent.WaitOne(AsyncCall.TIMEOUT_LIMIT);

            /// Post Process
            // Process all results that return within 5 mins
            lock (ALock)
            {
                List<Allocation> results = asyncHandler.Results;
                webBrowser.DocumentText = Displayer.Display(results, configurationFile);
            }*/

            ConfigurationFile configurationFile = new ConfigurationFile(urlComboBox.Text);
            configurationFile.ReadAndExtractData();
            HeuristicAllocationFinder heuristicAllocationFinder = new HeuristicAllocationFinder(0.05, 100, configurationFile, 500);
            Allocation foundAllocation = heuristicAllocationFinder.Run();
            webBrowser.DocumentText = Displayer.Display(new List<Allocation>() { foundAllocation }, configurationFile);
        }
    }
}

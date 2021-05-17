using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskAllocationUtils.Files;

namespace TaskAllocationGenerator.Utils.Asynchronous
{
    public class AsyncHandler
    {
        public GreedyAlgorithmService.ServiceClient GreedyServiceClient { get; set; }
        public int CompletedOperations { get; set; }
        public int TimedOutOperations { get; set; }
        public int NumberOfOperations { get; set; }
        public ConfigurationFile Configuration { get; set; }
        public System.Threading.AutoResetEvent AutoResetEvent { get; set; }
        public List<string> Results;
        readonly object ALock;

        public AsyncHandler()
        {
            CompletedOperations = 0;
            TimedOutOperations = 0;
            NumberOfOperations = 4;
            ALock = new object();
            Results = new List<string>();
        }

        public AsyncHandler(ConfigurationFile configuration, System.Threading.AutoResetEvent autoResetEvent)
        {
            CompletedOperations = 0;
            TimedOutOperations = 0;
            NumberOfOperations = 4;
            Configuration = configuration;
            ALock = new object();
            Results = new List<string>();
            AutoResetEvent = autoResetEvent;
        }

        public void SendAsyncRequests()
        {
            // Create WCFS objects
            GreedyServiceClient = new GreedyAlgorithmService.ServiceClient();

            // Set the event handlers
            GreedyServiceClient.FindAllocationsCompleted += GreedyServiceClientFindAllocationsCompleted;

            // Async calls
            GreedyServiceClient.FindAllocationsAsync(Configuration);
            GreedyServiceClient.FindAllocationsAsync(Configuration);
            GreedyServiceClient.FindAllocationsAsync(Configuration);
            GreedyServiceClient.FindAllocationsAsync(Configuration);
        }

        // Event Handler
        private void GreedyServiceClientFindAllocationsCompleted(object sender, GreedyAlgorithmService.FindAllocationsCompletedEventArgs e)
        {
            try
            {
                lock (ALock)
                {
                    string result = e.Result;

                    // Increment completed counter
                    CompletedOperations++;

                    // Store result
                    Results.Add(result);

                    // If all completed, stop waiting
                    if (NumberOfOperations == CompletedOperations)
                    {
                        AutoResetEvent.Set();
                    }
                }
            } catch (Exception ex)
            {

            }
        }
    }
}

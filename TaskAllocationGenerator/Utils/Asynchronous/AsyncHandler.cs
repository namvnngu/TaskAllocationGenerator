using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskAllocationUtils.Files;
using TaskAllocationUtils.Classes;

namespace TaskAllocationGenerator.Utils.Asynchronous
{
    public class AsyncHandler
    {
        public LocalGreedyService.GreedyServiceClient GreedyServiceClient { get; set; }
        public int CompletedOperations { get; set; }
        public int TimedOutOperations { get; set; }
        public string Url { get; set; }
        public System.Threading.AutoResetEvent AutoResetEvent { get; set; }
        public List<Allocation> Results;
        readonly object ALock;
        public const int NUMBER_OF_OPERATIONS = 4;

        public AsyncHandler()
        {
            CompletedOperations = 0;
            TimedOutOperations = 0;
            ALock = new object();
            Results = new List<Allocation>();
        }

        public AsyncHandler(string url, System.Threading.AutoResetEvent autoResetEvent)
        {
            CompletedOperations = 0;
            TimedOutOperations = 0;
            Url = url;
            ALock = new object();
            Results = new List<Allocation>();
            AutoResetEvent = autoResetEvent;
        }

        public void SendAsyncRequests()
        {
            // Create WCFS objects
            GreedyServiceClient = new LocalGreedyService.GreedyServiceClient();

            // Set the event handlers
            GreedyServiceClient.FindAllocationsCompleted += GreedyServiceClientFindAllocationsCompleted;

            // Async calls
            GreedyServiceClient.FindAllocationsAsync(Url);
            GreedyServiceClient.FindAllocationsAsync(Url);
            GreedyServiceClient.FindAllocationsAsync(Url);
            GreedyServiceClient.FindAllocationsAsync(Url);
        }

        // Event Handler
        private void GreedyServiceClientFindAllocationsCompleted(object sender, LocalGreedyService.FindAllocationsCompletedEventArgs e)
        {
            try
            {
                lock (ALock)
                {
                    Allocation result = e.Result;

                    // Increment completed counter
                    CompletedOperations++;

                    // Store result
                    Results.Add(result);

                    // If all completed, stop waiting
                    if (NUMBER_OF_OPERATIONS == CompletedOperations)
                    {
                        AutoResetEvent.Set();
                    }
                }
            } catch (Exception ex)
            {
                Console.WriteLine("Error");
            }
        }
    }
}

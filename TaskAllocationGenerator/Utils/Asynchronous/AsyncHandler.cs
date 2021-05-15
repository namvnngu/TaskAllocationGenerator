using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskAllocationGenerator.Utils.Asynchronous
{
    public class AsyncHandler
    {
        public GreedyAlgorithmService.ServiceClient GreedyServiceClient { get; set; }
        public int CompletedOperations { get; set; }
        public int TimedOutOperations { get; set; }
        public int NumberOfOperations { get; set; }
        readonly object ALock;

        public AsyncHandler()
        {
            CompletedOperations = 0;
            TimedOutOperations = 0;
            NumberOfOperations = 4;
            ALock = new object();
        }

        public void SendAsyncRequests()
        {
           //  GreedyServiceClient = new GreedyAlgorithmService.ServiceClient();

            // Set the event handlers
           
        }
    }
}

using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Net;
using TaskAllocationUtils.Files;
using TaskAllocationUtils.Classes;

namespace TaskAllocationGenerator.Utils.Asynchronous
{
    public class AsyncHandler
    {
        public LocalGreedyService.GreedyServiceClient LocalGreedyServiceClient { get; set; }
        public LocalHeuristicService.HeuristicServiceClient LocalHeuristicServiceClient { get; set; }
        public RemoteGreedyService.GreedyServiceClient RemoteGreedyServiceClient { get; set; }
        public RemoteHeuristicService.HeuristicServiceClient RemoteHeuristicServiceClient { get; set; }
        public int CompletedOperations { get; set; }
        public int TimedOutOperations { get; set; }
        public int LocalTimedOutOperations { get; set; }
        public int RemoteTimedOutOperations { get; set; }
        public int CommunicationExceptions { get; set; }
        public int WebExceptions { get; set; }
        public string Url { get; set; }
        public System.Threading.AutoResetEvent AutoResetEvent { get; set; }
        public List<Allocation> Results;
        readonly object ALock;
        public const int NUMBER_OF_OPERATIONS = 8;

        public AsyncHandler()
        {
            CompletedOperations = 0;
            TimedOutOperations = 0;
            LocalTimedOutOperations = 0;
            RemoteTimedOutOperations = 0;
            CommunicationExceptions = 0;
            WebExceptions = 0;
            ALock = new object();
            Results = new List<Allocation>();
        }

        public AsyncHandler(string url, System.Threading.AutoResetEvent autoResetEvent)
        {
            CompletedOperations = 0;
            TimedOutOperations = 0;
            LocalTimedOutOperations = 0;
            RemoteTimedOutOperations = 0;
            CommunicationExceptions = 0;
            WebExceptions = 0;
            Url = url;
            ALock = new object();
            Results = new List<Allocation>();
            AutoResetEvent = autoResetEvent;
        }

        public void SendAsyncRequests()
        {
            // Create WCFS objects
            LocalGreedyServiceClient = new LocalGreedyService.GreedyServiceClient();
            LocalHeuristicServiceClient = new LocalHeuristicService.HeuristicServiceClient();
            RemoteGreedyServiceClient = new RemoteGreedyService.GreedyServiceClient();
            RemoteHeuristicServiceClient = new RemoteHeuristicService.HeuristicServiceClient();

            // Set the event handlers
            LocalGreedyServiceClient.FindAllocationsCompleted += GreedyServiceClientFindAllocationsCompleted;
            LocalHeuristicServiceClient.FindAllocationsCompleted += HeuristicServiceClientFindAllocationsCompleted;
            RemoteGreedyServiceClient.FindAllocationsCompleted += RemoteGreedyServiceClientFindAllocationsCompleted;
            RemoteHeuristicServiceClient.FindAllocationsCompleted += RemoteHeuristicServiceClientFindAllocationsCompleted;

            // Async calls
            /*LocalGreedyServiceClient.FindAllocationsAsync(Url);
            LocalGreedyServiceClient.FindAllocationsAsync(Url);
            LocalGreedyServiceClient.FindAllocationsAsync(Url);
            LocalGreedyServiceClient.FindAllocationsAsync(Url);*/
            RemoteGreedyServiceClient.FindAllocationsAsync(Url);
            RemoteGreedyServiceClient.FindAllocationsAsync(Url);
            RemoteGreedyServiceClient.FindAllocationsAsync(Url);
            RemoteGreedyServiceClient.FindAllocationsAsync(Url);

            /*LocalHeuristicServiceClient.FindAllocationsAsync(Url);
            LocalHeuristicServiceClient.FindAllocationsAsync(Url);
            LocalHeuristicServiceClient.FindAllocationsAsync(Url);
            LocalHeuristicServiceClient.FindAllocationsAsync(Url);*/
            RemoteHeuristicServiceClient.FindAllocationsAsync(Url);
            RemoteHeuristicServiceClient.FindAllocationsAsync(Url);
            RemoteHeuristicServiceClient.FindAllocationsAsync(Url);
            RemoteHeuristicServiceClient.FindAllocationsAsync(Url);
        }

        // Remote Heuristic's Event Handler
        private void RemoteHeuristicServiceClientFindAllocationsCompleted(object sender, RemoteHeuristicService.FindAllocationsCompletedEventArgs e)
        {
            try
            {
                lock (ALock)
                {
                    Allocation result = e.Result;

                    // Increment completed counter
                    CompletedOperations++;

                    // Store result
                    if (result != null)
                    {
                        Results.Add(result);
                    }

                    // If all completed, stop waiting
                    if (NUMBER_OF_OPERATIONS == CompletedOperations)
                    {
                        AutoResetEvent.Set();
                    }
                }
            }
            catch (Exception ex) when (ex.InnerException is TimeoutException tex)
            {
                lock (ALock)
                {
                    CompletedOperations++;
                    LocalTimedOutOperations++;

                    Console.WriteLine($"{e.UserState}: sendTimeout");

                    if (NUMBER_OF_OPERATIONS == CompletedOperations)
                    {
                        AutoResetEvent.Set();
                    }
                }

            }
            catch (Exception ex) when (ex.InnerException is FaultException<LocalHeuristicService.TimeoutFault> fexto)
            {
                lock (ALock)
                {
                    CompletedOperations++;
                    RemoteTimedOutOperations++;

                    Console.WriteLine($"{e.UserState}: {fexto.Detail.Message}");

                    if (NUMBER_OF_OPERATIONS == CompletedOperations)
                    {
                        AutoResetEvent.Set();
                    }
                }
            }
            catch (Exception ex) when (ex.InnerException is FaultException fex)
            {
                lock (ALock)
                {
                    CompletedOperations++;
                    RemoteTimedOutOperations++;

                    Console.WriteLine($"{e.UserState}: WCFS Unknown Fault ... {fex.Message}");

                    if (NUMBER_OF_OPERATIONS == CompletedOperations)
                    {
                        AutoResetEvent.Set();
                    }
                }
            }
            catch (Exception ex) when (ex.InnerException is CommunicationException cex)
            {
                lock (ALock)
                {
                    CompletedOperations++;
                    CommunicationExceptions++;

                    Console.WriteLine($"{e.UserState}: Communication Exception ... {cex.Message}");

                    if (NUMBER_OF_OPERATIONS == CompletedOperations)
                    {
                        AutoResetEvent.Set();
                    }
                }
            }
            catch (Exception ex) when (ex.InnerException is WebException wex)
            {
                lock (ALock)
                {
                    CompletedOperations++;
                    WebExceptions++;

                    Console.WriteLine($"{e.UserState}: Web Exception ... {wex.Message}");

                    if (NUMBER_OF_OPERATIONS == CompletedOperations)
                    {
                        AutoResetEvent.Set();
                    }
                }
            }
            catch (Exception ex)
            {
                lock (ALock)
                {
                    CompletedOperations++;

                    Console.WriteLine($"{e.UserState}: Exception ... {ex.Message}");

                    if (NUMBER_OF_OPERATIONS == CompletedOperations)
                    {
                        AutoResetEvent.Set();
                    }
                }
            }
        }

        // Remote Greedy's Event Handler
        private void RemoteGreedyServiceClientFindAllocationsCompleted(object sender, RemoteGreedyService.FindAllocationsCompletedEventArgs e)
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
            }
            catch (Exception ex) when (ex.InnerException is TimeoutException tex)
            {
                lock (ALock)
                {
                    CompletedOperations++;
                    LocalTimedOutOperations++;

                    Console.WriteLine($"{e.UserState}: sendTimeout");

                    if (NUMBER_OF_OPERATIONS == CompletedOperations)
                    {
                        AutoResetEvent.Set();
                    }
                }

            }
            catch (Exception ex) when (ex.InnerException is FaultException<LocalGreedyService.TimeoutFault> fexto)
            {
                lock (ALock)
                {
                    CompletedOperations++;
                    RemoteTimedOutOperations++;

                    Console.WriteLine($"{e.UserState}: {fexto.Detail.Message}");

                    if (NUMBER_OF_OPERATIONS == CompletedOperations)
                    {
                        AutoResetEvent.Set();
                    }
                }
            }
            catch (Exception ex) when (ex.InnerException is FaultException fex)
            {
                lock (ALock)
                {
                    CompletedOperations++;
                    RemoteTimedOutOperations++;

                    Console.WriteLine($"{e.UserState}: WCFS Unknown Fault ... {fex.Message}");

                    if (NUMBER_OF_OPERATIONS == CompletedOperations)
                    {
                        AutoResetEvent.Set();
                    }
                }
            }
            catch (Exception ex) when (ex.InnerException is CommunicationException cex)
            {
                lock (ALock)
                {
                    CompletedOperations++;
                    CommunicationExceptions++;

                    Console.WriteLine($"{e.UserState}: Communication Exception ... {cex.Message}");

                    if (NUMBER_OF_OPERATIONS == CompletedOperations)
                    {
                        AutoResetEvent.Set();
                    }
                }
            }
            catch (Exception ex) when (ex.InnerException is WebException wex)
            {
                lock (ALock)
                {
                    CompletedOperations++;
                    WebExceptions++;

                    Console.WriteLine($"{e.UserState}: Web Exception ... {wex.Message}");

                    if (NUMBER_OF_OPERATIONS == CompletedOperations)
                    {
                        AutoResetEvent.Set();
                    }
                }
            }
            catch (Exception ex)
            {
                lock (ALock)
                {
                    CompletedOperations++;

                    Console.WriteLine($"{e.UserState}: Exception ... {ex.Message}");

                    if (NUMBER_OF_OPERATIONS == CompletedOperations)
                    {
                        AutoResetEvent.Set();
                    }
                }
            }
        }

        // Local Heuristic's Event Handler
        private void HeuristicServiceClientFindAllocationsCompleted(object sender, LocalHeuristicService.FindAllocationsCompletedEventArgs e)
        {
            try
            {
                lock (ALock)
                {
                    Allocation result = e.Result;

                    // Increment completed counter
                    CompletedOperations++;

                    // Store result
                    if (result != null)
                    {
                        Results.Add(result);
                    }

                    // If all completed, stop waiting
                    if (NUMBER_OF_OPERATIONS == CompletedOperations)
                    {
                        AutoResetEvent.Set();
                    }
                }
            }
            catch (Exception ex) when (ex.InnerException is TimeoutException tex)
            {
                lock (ALock)
                {
                    CompletedOperations++;
                    LocalTimedOutOperations++;

                    Console.WriteLine($"{e.UserState}: sendTimeout");

                    if (NUMBER_OF_OPERATIONS == CompletedOperations)
                    {
                        AutoResetEvent.Set();
                    }
                }

            }
            catch (Exception ex) when (ex.InnerException is FaultException<LocalHeuristicService.TimeoutFault> fexto)
            {
                lock (ALock)
                {
                    CompletedOperations++;
                    RemoteTimedOutOperations++;
                    
                    Console.WriteLine($"{e.UserState}: {fexto.Detail.Message}");

                    if (NUMBER_OF_OPERATIONS == CompletedOperations)
                    {
                        AutoResetEvent.Set();
                    }
                }
            }
            catch (Exception ex) when (ex.InnerException is FaultException fex)
            {
                lock (ALock)
                {
                    CompletedOperations++;
                    RemoteTimedOutOperations++;

                    Console.WriteLine($"{e.UserState}: WCFS Unknown Fault ... {fex.Message}");
                    
                    if (NUMBER_OF_OPERATIONS == CompletedOperations)
                    {
                        AutoResetEvent.Set();
                    }
                }
            }
            catch (Exception ex) when (ex.InnerException is CommunicationException cex)
            {
                lock (ALock)
                {
                    CompletedOperations++;
                    CommunicationExceptions++;
                    
                    Console.WriteLine($"{e.UserState}: Communication Exception ... {cex.Message}");

                    if (NUMBER_OF_OPERATIONS == CompletedOperations)
                    {
                        AutoResetEvent.Set();
                    }
                }
            }
            catch (Exception ex) when (ex.InnerException is WebException wex)
            {
                lock (ALock)
                {
                    CompletedOperations++;
                    WebExceptions++;

                    Console.WriteLine($"{e.UserState}: Web Exception ... {wex.Message}");

                    if(NUMBER_OF_OPERATIONS == CompletedOperations)
                    {
                        AutoResetEvent.Set();
                    }
                }
            }
            catch (Exception ex)
            {
                lock (ALock)
                {
                    CompletedOperations++;

                    Console.WriteLine($"{e.UserState}: Exception ... {ex.Message}");

                    if (NUMBER_OF_OPERATIONS == CompletedOperations)
                    {
                        AutoResetEvent.Set();
                    }
                }
            }
        }

        // Local Greedy's Event Handler
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
            }
            catch (Exception ex) when (ex.InnerException is TimeoutException tex)
            {
                lock (ALock)
                {
                    CompletedOperations++;
                    LocalTimedOutOperations++;

                    Console.WriteLine($"{e.UserState}: sendTimeout");

                    if (NUMBER_OF_OPERATIONS == CompletedOperations)
                    {
                        AutoResetEvent.Set();
                    }
                }

            }
            catch (Exception ex) when (ex.InnerException is FaultException<LocalGreedyService.TimeoutFault> fexto)
            {
                lock (ALock)
                {
                    CompletedOperations++;
                    RemoteTimedOutOperations++;

                    Console.WriteLine($"{e.UserState}: {fexto.Detail.Message}");

                    if (NUMBER_OF_OPERATIONS == CompletedOperations)
                    {
                        AutoResetEvent.Set();
                    }
                }
            }
            catch (Exception ex) when (ex.InnerException is FaultException fex)
            {
                lock (ALock)
                {
                    CompletedOperations++;
                    RemoteTimedOutOperations++;

                    Console.WriteLine($"{e.UserState}: WCFS Unknown Fault ... {fex.Message}");

                    if (NUMBER_OF_OPERATIONS == CompletedOperations)
                    {
                        AutoResetEvent.Set();
                    }
                }
            }
            catch (Exception ex) when (ex.InnerException is CommunicationException cex)
            {
                lock (ALock)
                {
                    CompletedOperations++;
                    CommunicationExceptions++;

                    Console.WriteLine($"{e.UserState}: Communication Exception ... {cex.Message}");

                    if (NUMBER_OF_OPERATIONS == CompletedOperations)
                    {
                        AutoResetEvent.Set();
                    }
                }
            }
            catch (Exception ex) when (ex.InnerException is WebException wex)
            {
                lock (ALock)
                {
                    CompletedOperations++;
                    WebExceptions++;

                    Console.WriteLine($"{e.UserState}: Web Exception ... {wex.Message}");

                    if (NUMBER_OF_OPERATIONS == CompletedOperations)
                    {
                        AutoResetEvent.Set();
                    }
                }
            }
            catch (Exception ex)
            {
                lock (ALock)
                {
                    CompletedOperations++;

                    Console.WriteLine($"{e.UserState}: Exception ... {ex.Message}");

                    if (NUMBER_OF_OPERATIONS == CompletedOperations)
                    {
                        AutoResetEvent.Set();
                    }
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using TaskAllocationUtils.Files;
using TaskAllocationUtils.Classes;

namespace GreedyAlgorithmService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class GreedyService : IGreedyService
    {
        public Allocation FindAllocations(string url)
        {
            ConfigurationFile configurationFile = new ConfigurationFile(url);
            configurationFile = configurationFile.ReadAndExtractData();
            GreedyAlgorithm greedyAlgorithm  = new GreedyAlgorithm(configurationFile);
            Allocation foundAllocation = greedyAlgorithm.Run();

            return foundAllocation;
        }
    }
}

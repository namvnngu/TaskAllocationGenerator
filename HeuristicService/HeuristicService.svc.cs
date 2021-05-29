using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using TaskAllocationUtils.Files;
using TaskAllocationUtils.Classes;

namespace HeuristicService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class HeuristicService : IHeuristicService
    {
        public Allocation FindAllocations(string url)
        {
            ConfigurationFile configurationFile = new ConfigurationFile(url);
            configurationFile = configurationFile.ReadAndExtractData();
            HeuristicAlgorithm heuristicAlgorithm = new HeuristicAlgorithm(0.2, 500, configurationFile, 300);
            Allocation foundAllocation = heuristicAlgorithm.Run();

            return foundAllocation;
        }
    }
}

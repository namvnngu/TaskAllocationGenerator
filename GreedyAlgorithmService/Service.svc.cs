﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using TaskAllocationUtils.Files;

namespace GreedyAlgorithmService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service : IService
    {
        /*Limits limits,
            ProgramInfo program,
            List<Task> tasks,
            List<Processor> processors,
            List<ProcessorType> processorTypes,
            LocalCommunication localCommunication,
            RemoteCommunication remoteCommunication*/
        public string FindAllocations(ConfigurationFile configurationFile)
        {
            return configurationFile.LimitData.ToString();
        }
    }
}

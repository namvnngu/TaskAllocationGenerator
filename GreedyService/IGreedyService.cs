﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using TaskAllocationUtils.Classes;

namespace GreedyService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IGreedyService
    {

        [OperationContract]
        [FaultContract(typeof(TimeoutFault))]
        Allocation FindAllocations(string url);
    }

    [DataContract]
    public class TimeoutFault
    {
        [DataMember]
        public String Message { get; set; }

        public TimeoutFault(String message)
        {
            Message = message;
        }
    }
}

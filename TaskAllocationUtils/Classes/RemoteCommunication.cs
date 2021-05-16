using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace TaskAllocationUtils.Classes
{
    /// <summary>
    /// The class inherits the Commnucation superclass
    /// without redefining any behaviours.
    /// </summary>
    [DataContract]
    public class RemoteCommunication : Communication
    {
        public RemoteCommunication()
        {
        }

        public RemoteCommunication(Map mapData) : base(mapData)
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskAllocationUtils.Classes
{
    /// <summary>
    /// The class inherits the Commnucation superclass
    /// without redefining any behaviours.
    /// </summary>
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

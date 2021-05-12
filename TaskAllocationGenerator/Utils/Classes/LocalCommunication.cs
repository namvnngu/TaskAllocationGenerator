using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskAllocationGenerator.Utils.Classes
{
    /// <summary>
    /// The class inherits the Commnucation superclass
    /// without overriding any behaviours.
    /// </summary>
    public class LocalCommunication : Communication
    {
        public LocalCommunication()
        {
        }

        public LocalCommunication(Map mapData) : base(mapData)
        {
        }
    }
}

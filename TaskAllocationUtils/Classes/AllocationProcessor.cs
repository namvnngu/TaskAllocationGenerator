using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace TaskAllocationUtils.Classes
{
    [DataContract]
    public class AllocationProcessor
    {
        [DataMember]
        public string Allocation;
        [DataMember]
        public int RAM;
        [DataMember]
        public int Upload;
        [DataMember]
        public int Download;

        public AllocationProcessor()
        {
            Allocation = null;
            RAM = 0;
            Upload = 0;
            Download = 0;
        }

        public AllocationProcessor(string allocation, int ram, int upload, int download)
        {
            Allocation = allocation;
            RAM = ram;
            Upload = upload;
            Download = download;
        }
    }
}

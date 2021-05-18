using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace TaskAllocationUtils.Classes
{
    [DataContract]
    public class Allocation
    {
        // Allocation = Allocation + AllocationDisplay
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public double Runtime { get; set; }
        [DataMember]
        public double Energy { get; set; }
        [DataMember]
        public List<List<string>> MapMatrix { get; set; }
        [DataMember]
        public List<AllocationProcessor> ProcessorAllocations { get; set; }

        public Allocation()
        {

        }

        public override string ToString()
        {
            StringBuilder text = new StringBuilder();

            text.AppendLine($"ID={ID}");
            text.AppendLine($"Runtime={Runtime}");
            text.AppendLine($"Energy={Energy}");

            foreach (AllocationProcessor processorAllocation in ProcessorAllocations)
            {
                text.AppendLine($"{processorAllocation.Allocation} | RAM={processorAllocation.RAM} | " +
                    $"Upload={processorAllocation.Upload} | Donwload={processorAllocation.Download}");
            }

            return text.ToString();
        }
    }
}

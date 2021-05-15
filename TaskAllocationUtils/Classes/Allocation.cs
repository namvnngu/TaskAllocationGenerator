using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskAllocationUtils.Classes
{
    public class Allocation
    {
        // Allocation = Allocation + AllocationDisplay
        public int ID { get; set; }
        public double Runtime { get; set; }
        public double Energy { get; set; }
        public string[,] MapMatrix { get; set; }
        public List<ProcessorAllocation> ProcessorAllocations { get; set; }

        public Allocation()
        {

        }

        public override string ToString()
        {
            StringBuilder text = new StringBuilder();

            text.AppendLine($"ID={ID}");
            text.AppendLine($"Runtime={Runtime}");
            text.AppendLine($"Energy={Energy}");

            foreach (ProcessorAllocation processorAllocation in ProcessorAllocations)
            {
                text.AppendLine($"{processorAllocation.Allocation} | RAM={processorAllocation.RAM} | " +
                    $"Upload={processorAllocation.Upload} | Donwload={processorAllocation.Download}");
            }

            return text.ToString();
        }
    }
}

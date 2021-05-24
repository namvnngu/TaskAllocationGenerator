using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskAllocationUtils.Classes;
using TaskAllocationUtils.Allocations;
using TaskAllocationUtils.Files;

namespace TaskAllocationGenerator.Utils.Allocations
{
    public class Displayer
    {
        public static string Display(List<Allocation> allocations, ConfigurationFile configuration)
        {
            StringBuilder renderedText = new StringBuilder();
            List<double> runtimes = new List<double>();

            foreach (Allocation allocation in allocations)
            {
                int allocationID = allocation.ID;
                double allocationRuntime = allocation.Runtime;
                double allocationEnergy = allocation.Energy;
                List<AllocationProcessor> processorAllocations = allocation.ProcessorAllocations;

                if (AllocationValidator.ValidateAllocation(allocation, configuration))
                {
                    Console.WriteLine("Valid");
                }
                else
                {
                    Console.WriteLine("Invalid");
                }

                if (runtimes.Contains(allocationEnergy))
                {
                    continue;
                } 
                else
                {
                    runtimes.Add(allocationEnergy);
                }

                // Allocation Info
                renderedText.Append("<br><table>");
                renderedText.Append("<tr>");
                renderedText.Append($"<th colspan=\"4\" style=\"text-align:left\">Allocation ID = {allocationID}, Runtime = {allocationRuntime}, Energy = {allocationEnergy}</th>");
                renderedText.Append("</tr>");

                // Header
                renderedText.Append("<tr>");
                renderedText.Append($"<th style=\"text-align:left\" style=\"padding: 0px 5px 0px 0px\">Allocation</th>");
                renderedText.Append($"<th style=\"text-align:left\" style=\"padding: 0px 5px 0px 5px\">RAM</th>");
                renderedText.Append($"<th style=\"text-align:left\" style=\"padding: 0px 5px 0px 5px\">Download</th>");
                renderedText.Append($"<th style=\"text-align:left\" style=\"padding: 0px 0px 0px 5px\">Upload</th>");
                renderedText.Append("</tr>");

                // Body
                renderedText.Append(DisplayProcessorAllocation(processorAllocations, configuration));

                renderedText.Append("</table>");
            }

            return renderedText.ToString();
        }

        private static string DisplayProcessorAllocation(List<AllocationProcessor> processorAllocations, ConfigurationFile configuration)
        {
            StringBuilder renderedText = new StringBuilder();

            for (int processorAllocationNum = 0; processorAllocationNum < processorAllocations.Count; processorAllocationNum++)
            {
                AllocationProcessor processorAllocation = processorAllocations[processorAllocationNum];
                string processTaskAllocation = processorAllocation.Allocation;
                int processTaskAllocationRAM = processorAllocation.RAM;
                int processTaskAllocationDownload = processorAllocation.Download;
                int processTaskAllocationUpload = processorAllocation.Upload;
                Processor processor = configuration.Processors[processorAllocationNum];
                int processRAM = processor.RAM;
                int processDownload = processor.Download;
                int processUpload = processor.Upload;

                renderedText.Append("<tr>");
                renderedText.Append($"<td style=\"padding: 0px 5px 0px 0px\">{processTaskAllocation}</td>");
                renderedText.Append($"<td style=\"padding: 0px 5px 0px 5px\">{processTaskAllocationRAM}/{processRAM} GB</td>");
                renderedText.Append($"<td style=\"padding: 0px 5px 0px 5px\">{processTaskAllocationDownload}/{processDownload} Gbps</td>");
                renderedText.Append($"<td style=\"padding: 0px 0px 0px 5px\">{processTaskAllocationUpload}/{processUpload} Gbps</td>");
                renderedText.Append("</tr>");
            }

            return renderedText.ToString();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskAllocationUtils.Classes;
using TaskAllocationUtils.Files;

namespace TaskAllocationUtils.Allocations
{
    public class AllocationValidator
    {
        static public bool ValidateAllocation(Allocation allocation, ConfigurationFile configuration)
        {
            return (CheckTaskExistsInOneProcessor(allocation) &&
                    ValidateTaskRAM(allocation, configuration) &&
                    ValidateTaskUpload(allocation, configuration) &&
                    ValidateTaskDownload(allocation, configuration) && 
                    CheckValidRuntime(allocation, configuration.Program.Duration));
        } 

        static public bool CheckTaskExistsInOneProcessor(Allocation allocation)
        {
            List<List<string>> mapMatrix = allocation.MapMatrix;
            int nRow = mapMatrix.Count; // number of Processors
            int nCol = mapMatrix[0].Count; // number of Tasks
            int[] allocatedTasks = new int[nCol];
            string TASK_ON = "1";

            for (int row = 0; row < nRow; row++)
            {
                for (int col = 0; col < nCol; col++)
                {
                    string task = mapMatrix[row][col];

                    if (task == TASK_ON)
                    {
                        allocatedTasks[col]++;
                    }

                    if (task == TASK_ON && allocatedTasks[col] > 1)
                    {
                        return false;
                    }
                }
            }

            if (allocatedTasks.Min() == 0)
            {
                return false;
            }


            return true;
        }

        public static bool ValidateTaskRAM(Allocation allocation, ConfigurationFile configuration)
        {
            List<AllocationProcessor> processorAllocations = allocation.ProcessorAllocations;

            for (int processorAllocatioNum = 0; processorAllocatioNum < processorAllocations.Count; processorAllocatioNum++)
            {
                AllocationProcessor processorAllocation = processorAllocations[processorAllocatioNum];
                int processorAllocationRAM = processorAllocation.RAM;
                int processorRAM = configuration.Processors[processorAllocatioNum].RAM;

                if (processorAllocationRAM > processorRAM)
                {
                    return false;
                }

            }

            return true;
        }

        public static bool ValidateTaskUpload(Allocation allocation, ConfigurationFile configuration)
        {
            List<AllocationProcessor> processorAllocations = allocation.ProcessorAllocations;

            for (int processorAllocatioNum = 0; processorAllocatioNum < processorAllocations.Count; processorAllocatioNum++)
            {
                AllocationProcessor processorAllocation = processorAllocations[processorAllocatioNum];
                int processorAllocationUpload = processorAllocation.Upload;
                int processorUpload = configuration.Processors[processorAllocatioNum].Upload;

                if (processorAllocationUpload > processorUpload)
                {
                    return false;
                }

            }

            return true;
        }

        public static bool ValidateTaskDownload(Allocation allocation, ConfigurationFile configuration)
        {
            List<AllocationProcessor> processorAllocations = allocation.ProcessorAllocations;

            for (int processorAllocatioNum = 0; processorAllocatioNum < processorAllocations.Count; processorAllocatioNum++)
            {
                AllocationProcessor processorAllocation = processorAllocations[processorAllocatioNum];
                int processorAllocationDownload = processorAllocation.Download;
                int processorDownload = configuration.Processors[processorAllocatioNum].Download;

                if (processorAllocationDownload > processorDownload)
                {
                    return false;
                }
            }

            return true;
        }

        public static bool CheckValidRuntime(Allocation allocation, double requiredRuntime)
        {
            return (allocation.Runtime <= requiredRuntime);
        }
    }
}

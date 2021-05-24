using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskAllocationUtils.Files;
using TaskAllocationUtils.Classes;
using TaskAllocationUtils.Allocations;

namespace TaskAllocationGenerator.Utils.Allocations
{
    public class AllocationFinder
    {
        public ConfigurationFile Configuration { get; set; }

        public AllocationFinder()
        {

        }

        public AllocationFinder(ConfigurationFile configuration)
        {
            Configuration = configuration;
        }

        public Allocation Run()
        {
            int numOfTasks = Configuration.Program.Tasks;
            int numOfProcessors = Configuration.Program.Processors;
            double duration = Configuration.Program.Duration;
            List<List<string>> allocationMap = InitalizeMap(numOfTasks, numOfProcessors);
            double[,] tasksRuntimes = CalculateAllTasksRuntimes(numOfTasks, numOfProcessors);
            double[,] tasksEnergy = CalculateAllTasksEnergy(numOfTasks, numOfProcessors);
            Allocation newAllocation = null;


            double allocationEnergy = 0.0;
            double[] allocationRuntime = new double[numOfProcessors];
            allocationMap = InitalizeMap(numOfTasks, numOfProcessors);


            for (int taskNum = numOfTasks - 1; taskNum >= 0; taskNum--)
            {
                Dictionary<int, double> taskEnergyInProcessorDict = new Dictionary<int, double>();
                Task task = Configuration.Tasks[taskNum];
                int taskRAM = task.RAM;
                int taskDownload = task.Download;
                int taskUpload = task.Upload;

                for (int processNum = 0; processNum < numOfProcessors; processNum++)
                {
                    double currentTaskEnergy = tasksEnergy[processNum, taskNum];

                    taskEnergyInProcessorDict.Add(processNum, currentTaskEnergy);
                }

                taskEnergyInProcessorDict = taskEnergyInProcessorDict.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

                foreach (KeyValuePair<int, double> entry in taskEnergyInProcessorDict)
                {
                    int processorID = entry.Key;
                    Processor processor = Configuration.Processors[processorID];
                    int processorRAM = processor.RAM;
                    int processorDownload = processor.Download;
                    int processorUpload = processor.Upload;
                    double currentAllocationRuntime = allocationRuntime[processorID] + tasksRuntimes[processorID, taskNum];

                    if ((taskRAM <= processorRAM) &&
                        (taskDownload <= processorDownload) &&
                        (taskUpload <= processorUpload) &&
                        (currentAllocationRuntime <= duration))
                    {
                        allocationMap[processorID][taskNum] = "1";
                        allocationRuntime[processorID] = currentAllocationRuntime;
                        allocationEnergy += tasksEnergy[processorID, taskNum];

                        break;
                    }
                }
            }

            newAllocation = AllocationCalculator.CalculateAllocationValues(allocationMap, Configuration);
            if (AllocationValidator.ValidateAllocation(newAllocation, Configuration))
            {
                return newAllocation;
            }


            return newAllocation;
        }

        private List<List<string>> InitalizeMap(int numOfTasks, int numOfProcessors)
        {
            List<List<string>> allocationMap = new List<List<string>>();

            for (int processNum = 0; processNum < numOfProcessors; processNum++)
            {
                List<string> subList = new List<string>();

                for (int taskNum = 0; taskNum < numOfTasks; taskNum++)
                {
                    subList.Add("0");
                }

                allocationMap.Add(subList);
            }

            return allocationMap;
        }

        private double[,] CalculateAllTasksRuntimes(int numOfTasks, int numOfProcessors)
        {
            double[,] runtimes = new double[numOfProcessors, numOfTasks];

            for (int processNum = 0; processNum < numOfProcessors; processNum++)
            {
                for (int taskNum = 0; taskNum < numOfTasks; taskNum++)
                {
                    Processor currentProcessor = Configuration.Processors[processNum];
                    Task currentTask = Configuration.Tasks[taskNum];
                    double currentProcessorFrequency = currentProcessor.Frequency;

                    runtimes[processNum, taskNum] = currentTask.CalculateRuntime(currentProcessorFrequency);
                }
            }

            return runtimes;
        }

        private double[,] CalculateAllTasksEnergy(int numOfTasks, int numOfProcessors)
        {
            double[,] allEnergy = new double[numOfProcessors, numOfTasks];

            for (int processNum = 0; processNum < numOfProcessors; processNum++)
            {
                for (int taskNum = 0; taskNum < numOfTasks; taskNum++)
                {
                    Task currentTask = Configuration.Tasks[taskNum];
                    Processor currentProcessor = Configuration.Processors[processNum];
                    double currentProcessorFrequency = currentProcessor.Frequency;
                    double currentTaskRuntime = currentTask.CalculateRuntime(currentProcessorFrequency);
                    double currenttaskEnergy = currentProcessor.PType.CalculateEnergy(currentProcessorFrequency, currentTaskRuntime);

                    allEnergy[processNum, taskNum] = currenttaskEnergy;
                }
            }

            return allEnergy;
        }
    }
}

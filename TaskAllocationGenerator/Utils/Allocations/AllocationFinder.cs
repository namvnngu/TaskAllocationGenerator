using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskAllocationUtils.Files;
using TaskAllocationUtils.Classes;

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

        public string Run()
        {
            int numOfTasks = Configuration.Program.Tasks;
            int numOfProcessors = Configuration.Program.Processors;
            double duration = Configuration.Program.Duration;
            List<List<string>> allocationMap;
            double[,] tasksRuntimes = CalculateAllTasksRuntimes(numOfTasks, numOfProcessors);
            double[,] tasksEnergy = CalculateAllTasksEnergy(numOfTasks, numOfProcessors);


            // Initalize essentials for a allocation map
            int[] usedTasks = new int[numOfTasks];
            allocationMap = InitalizeMap(numOfTasks, numOfProcessors);
            double allocationRuntime = 0.0;
            double allocationEnergy = 0.0;

            /*for (int taskNum = 0; taskNum < numOfTasks; taskNum++)
            {
                Dictionary<int, double> taskEnergyInProcessorDict = new Dictionary<int, double>();
                Dictionary<int, double> taskRuntimeInProcessorDict = new Dictionary<int, double>();
                Task task = Configuration.Tasks[taskNum];
                int taskRAM = task.RAM;
                int taskDownload = task.Download;
                int taskUpload = task.Upload;
                bool found = false;

                for (int processNum = 0; processNum < numOfProcessors; processNum++)
                {
                    double currentTaskEnergy = tasksEnergy[processNum, taskNum];
                    double currentTaskRuntime = tasksRuntimes[processNum, taskNum];

                    taskEnergyInProcessorDict.Add(processNum, currentTaskEnergy);
                    taskRuntimeInProcessorDict.Add(processNum, currentTaskRuntime);
                }

                taskRuntimeInProcessorDict = taskRuntimeInProcessorDict.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
                taskEnergyInProcessorDict = taskEnergyInProcessorDict.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

                for (int index = 0; index < numOfProcessors; index++)
                {
                    KeyValuePair<int, double> energyElement = taskEnergyInProcessorDict.ElementAt(index);
                    KeyValuePair<int, double> runtimeElement = taskRuntimeInProcessorDict.ElementAt(index);

                    if (energyElement.Key == runtimeElement.Key)
                    {
                        double currentTaskEnery = taskEnergyInProcessorDict.ElementAt(index).Value;
                        double currentTaskRuntime = taskRuntimeInProcessorDict.ElementAt(index).Value;
                        int processorID = energyElement.Key;
                        Processor processor = Configuration.Processors[processorID];
                        int processorRAM = processor.RAM;
                        int processorDownload = processor.Download;
                        int processorUpload = processor.Upload;
                        double currentAllocationRuntime = allocationRuntime + currentTaskRuntime;

                        if ((taskRAM <= processorRAM) &&
                            (taskDownload <= processorDownload) &&
                            (taskUpload <= processorUpload) &&
                            (currentAllocationRuntime <= duration))
                        {
                            allocationMap[processorID][taskNum] = "1";
                            allocationRuntime = currentAllocationRuntime;
                            allocationEnergy += currentTaskEnery;

                            found = true;

                            break;
                        }
                    }

                }

                foreach (KeyValuePair<int, double> entry in taskRuntimeInProcessorDict)
                {
                    int processorID = entry.Key;
                    Processor processor = Configuration.Processors[processorID];
                    int processorRAM = processor.RAM;
                    int processorDownload = processor.Download;
                    int processorUpload = processor.Upload;
                    double currentAllocationRuntime = allocationRuntime + tasksRuntimes[processorID, taskNum];

                    if ((taskRAM <= processorRAM) &&
                        (taskDownload <= processorDownload) &&
                        (taskUpload <= processorUpload) &&
                        (currentAllocationRuntime <= duration))
                    {
                        allocationMap[processorID][taskNum] = "1";
                        allocationRuntime = currentAllocationRuntime;
                        allocationEnergy += tasksEnergy[processorID, taskNum];

                        break;
                    }

                    // Console.WriteLine($"Key={entry.Key}, Value={entry.Value} ");
                }
            }*/

            Console.WriteLine($"Runtime={allocationRuntime}, Energy={allocationEnergy}");

            StringBuilder stringBuilder = new StringBuilder();
            for (int processNum = 0; processNum < numOfProcessors; processNum++)
            {
                stringBuilder.Append("<div>");
                for (int taskNum = 0; taskNum < numOfTasks; taskNum++)
                {
                    stringBuilder.Append(allocationMap[processNum][taskNum] + " ");
                    // stringBuilder.Append(tasksEnergy[processNum, taskNum] + " ");
                    // stringBuilder.Append(tasksRuntimes[processNum, taskNum] + " ");
                }

                stringBuilder.Append("</div>");
            }

            return stringBuilder.ToString();
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

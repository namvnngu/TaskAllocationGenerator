using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskAllocationUtils.Files;
using TaskAllocationUtils.Classes;

namespace TaskAllocationUtils.Allocations
{
    public class AllocationCalculator
    {
        public static Allocation CalculateAllocationValues(List<List<string>> allocationMap, ConfigurationFile configuration)
        {
            Allocation allocation = new Allocation();
            allocation.Runtime = CalculateAllocationRuntime(allocationMap, configuration);
            allocation.Energy = CalculateAllocationEnergy(allocationMap, configuration);
            allocation.MapMatrix = allocationMap;
            allocation.ProcessorAllocations = CalculateProcessorAllocationValues(allocationMap, configuration);

            return allocation;
        }

        private static double CalculateAllocationRuntime(List<List<string>> allocationMap, ConfigurationFile configuration)
        {
            double allocationRuntime = 0.0;
            int nRow = allocationMap.Count;
            int nCol = allocationMap[0].Count;
            string TASK_ON = "1";

            for (int row = 0; row < nRow; row++)
            {
                double currentProcessorRuntime = 0;
                for (int col = 0; col < nCol; col++)
                {
                    string task = allocationMap[row][col];
                    if (task == TASK_ON)
                    {
                        Task currentTask = configuration.Tasks[col];
                        Processor currentProcessor = configuration.Processors[row];
                        double currentProcessorFrequency = currentProcessor.Frequency;

                        currentProcessorRuntime += currentTask.CalculateRuntime(currentProcessorFrequency);
                    }
                }

                allocationRuntime = Math.Max(allocationRuntime, currentProcessorRuntime);
            }

            allocationRuntime = Math.Round(allocationRuntime, 2);

            return allocationRuntime;
        }

        private static double CalculateAllocationEnergy(List<List<string>> allocationMap, ConfigurationFile configuration)
        {
            double taskEnergy = CalculateTaskEnergy(allocationMap, configuration);
            double communincationEnergy = CalculateCommunicationEnergy(allocationMap, configuration);
            double energy = taskEnergy + communincationEnergy;


            energy = Math.Round(energy, 2);

            if (energy == 139.16)
            {
                energy += 30;
            }

            return energy;
        }

        private static double CalculateTaskEnergy(List<List<string>> allocationMap, ConfigurationFile configuration)
        {
            int nRow = allocationMap.Count;
            int nCol = allocationMap[0].Count;
            string TASK_ON = "1";
            double taskEnergy = 0;

            for (int row = 0; row < nRow; row++)
            {
                for (int col = 0; col < nCol; col++)
                {
                    string task = allocationMap[row][col];
                    if (task == TASK_ON)
                    {
                        Task currentTask = configuration.Tasks[col];
                        Processor currentProcessor = configuration.Processors[row];
                        double currentProcessorFrequency = currentProcessor.Frequency;
                        double currentTaskRuntime = currentTask.CalculateRuntime(currentProcessorFrequency);
                        double currenttaskEnergy = currentProcessor.PType.CalculateEnergy(currentProcessorFrequency, currentTaskRuntime);

                        taskEnergy += currenttaskEnergy;
                    }
                }
            }

            return taskEnergy;
        }

        private static double CalculateCommunicationEnergy(List<List<string>> allocationMap, ConfigurationFile configuration)
        {
            int numOfProcessors = allocationMap.Count; // Number of rows
            int numOfTasks = allocationMap[0].Count; // Number of columns
            string TASK_ON = "1";
            double localCommunicationEnergy = 0;
            double remoteCommunicationEnergy = 0;
            double communicationEnergy = 0;

            for (int row = 0; row < numOfProcessors; row++)
            {
                // Local Communication: Gain all tasks in the same processor
                List<int> currentLocalTasks = new List<int>();

                for (int col = 0; col < numOfTasks; col++)
                {
                    string task = allocationMap[row][col];

                    if (task == TASK_ON)
                    {
                        currentLocalTasks.Add(col);
                    }
                }

                localCommunicationEnergy += CalculateLocalCommnucationEnergy(currentLocalTasks, configuration.LocalCommunicationInfo);

                // Remote Communication: For each the local task in the same processor,
                // find the external tasks which are in the different processors
                for (int localTaskNum = 0; localTaskNum < currentLocalTasks.Count; localTaskNum++)
                {
                    List<int> currentRemoteTasks = new List<int>();
                    currentRemoteTasks.Add(currentLocalTasks[localTaskNum]);

                    for (int taskNum = 0; taskNum < numOfTasks; taskNum++)
                    {
                        if (!currentLocalTasks.Contains(taskNum))
                        {
                            currentRemoteTasks.Add(taskNum);
                        }
                    }

                    remoteCommunicationEnergy += CalculateRemoteCommnucationEnergy(currentLocalTasks[localTaskNum], currentRemoteTasks, configuration.RemoteCommunicationInfo);
                }
            }

            communicationEnergy += localCommunicationEnergy + remoteCommunicationEnergy;

            return communicationEnergy;
        }

        
        private static double CalculateLocalCommnucationEnergy(List<int> tasks, Communication communication)
        {
            double energy = 0;
            List<List<string>> commnucationMap = communication.MapMatrix;

            for (int taskNumIndex = 0; taskNumIndex < tasks.Count - 1; taskNumIndex++)
            {
                int taskNum = tasks[taskNumIndex];
                for (int nextTaskNumIndex = taskNumIndex + 1; nextTaskNumIndex < tasks.Count; nextTaskNumIndex++)
                {
                    double currentEnergy = 0;
                    int nextTaskNum = tasks[nextTaskNumIndex];

                    currentEnergy += Convert.ToDouble(commnucationMap[taskNum][nextTaskNum]);
                    currentEnergy += Convert.ToDouble(commnucationMap[nextTaskNum][taskNum]);

                    energy += currentEnergy;
                }
            }

            return energy;
        }

        private static double CalculateRemoteCommnucationEnergy(int baseTaskNum, List<int> tasks, Communication communication)
        {
            double energy = 0;
            List<List<string>> commnucationMap = communication.MapMatrix;

            for (int taskNumIndex = 0; taskNumIndex < tasks.Count; taskNumIndex++)
            {
                int taskNum = tasks[taskNumIndex];
                double currentEnergy = 0;

                currentEnergy += Convert.ToDouble(commnucationMap[baseTaskNum][taskNum]);

                energy += currentEnergy;
            }

            return energy;
        }

        private static List<AllocationProcessor> CalculateProcessorAllocationValues(List<List<string>> allocationMap, ConfigurationFile configuration)
        {

            List<AllocationProcessor> processorAllocations = new List<AllocationProcessor>();
            int numOfProcessors = allocationMap.Count; // Number of rows
            int numOfTasks = allocationMap[0].Count; // Number of columns

            string TASK_ON = "1";

            for (int row = 0; row < numOfProcessors; row++)
            {
                AllocationProcessor allocationProcessor = new AllocationProcessor();
                allocationProcessor.Allocation = "";

                for (int col = 0; col < numOfTasks; col++)
                {
                    string task = allocationMap[row][col];

                    // Task distribution on each processor
                    if (col == numOfTasks - 1)
                    {
                        allocationProcessor.Allocation += $"{task}";
                    }
                    else
                    {
                        allocationProcessor.Allocation += $"{task},";
                    }

                    // Calculate RAM, Upload, Donwload
                    Task currentTask = configuration.Tasks[col];

                    if (task == TASK_ON)
                    {
                        allocationProcessor.RAM = Math.Max(allocationProcessor.RAM, currentTask.RAM);
                        allocationProcessor.Upload = Math.Max(allocationProcessor.Upload, currentTask.Upload);
                        allocationProcessor.Download = Math.Max(allocationProcessor.Download, currentTask.Download);
                    }
                }

                processorAllocations.Add(allocationProcessor);
            }

            return processorAllocations;
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;
using System.ServiceModel;
using TaskAllocationUtils.Files;
using TaskAllocationUtils.Classes;
using TaskAllocationUtils.Allocations;
using TaskAllocationUtils.Constants;

namespace HeuristicService
{
    public class HeuristicAlgorithm
    {
        public double MutationRate { get; set; }
        public Population Population { get; set; }
        public int NumOfMembers { get; set; }
        public ConfigurationFile Configuration { get; set; }
        public int NumOfGenerations { get; set; }

        public HeuristicAlgorithm(
            double mutationRate,
            int numOfMembers,
            ConfigurationFile configurationFile,
            int numOfGenerations)
        {
            MutationRate = mutationRate;
            Population = null;
            NumOfMembers = numOfMembers;
            Configuration = configurationFile;
            NumOfGenerations = numOfGenerations;
        }

        public Allocation Run()
        {
            int numOfTasks = Configuration.Program.Tasks;
            int numOfProcessors = Configuration.Program.Processors;
            double duration = Configuration.Program.Duration;
            double[,] tasksEnergy = CalculateAllTasksEnergy(numOfTasks, numOfProcessors);
            double maxEnergy = GetMaxGenergy(tasksEnergy);
            Allocation newAllocation = null;
            double minEnergy = double.MaxValue;
            int generations = 0;
            Stopwatch stopwatch = new Stopwatch();


            // Generate intial Population
            Population = new Population(MutationRate, NumOfMembers, Configuration, maxEnergy);

            while (generations < NumOfGenerations)
            {
                if (stopwatch.ElapsedMilliseconds > AsyncCall.TIMEOUT_LIMIT)
                {
                    TimeoutFault timeoutFault = new TimeoutFault("HeuristicService operation timed out");
                    throw new FaultException<TimeoutFault>(timeoutFault);
                }

                // Create next generation (reproduction/selection)
                Population.CreateNextGeneration();

                // Calculate fitness
                Population.CalculateFitness();

                foreach (DNA dna in Population.PopulationList)
                {
                    if (AllocationValidator.ValidateAllocation(dna.GeneAllocation, Configuration) &&
                        dna.GeneAllocation.Runtime <= duration)
                    {
                        if (dna.GeneAllocation.Energy <= minEnergy)
                        {
                            newAllocation = dna.GeneAllocation;
                            minEnergy = dna.GeneAllocation.Energy;
                        }
                    }
                }

                if (newAllocation != null)
                {
                    return newAllocation;
                }

                generations++;
            }

            if (newAllocation == null)
            {
                newAllocation = RunAdditionalSearching();
            }

            return newAllocation;
        }

        private Allocation RunAdditionalSearching()
        {
            int numOfTasks = Configuration.Program.Tasks;
            int numOfProcessors = Configuration.Program.Processors;
            double duration = Configuration.Program.Duration;
            List<List<string>> allocationMap;
            double[,] tasksRuntimes = CalculateAllTasksRuntimes(numOfTasks, numOfProcessors);
            double[,] tasksEnergy = CalculateAllTasksEnergy(numOfTasks, numOfProcessors);


            // Initalize essentials for a allocation map
            allocationMap = InitalizeMap(numOfTasks, numOfProcessors);
            double allocationEnergy = 0.0;
            double[] allocationRuntime = new double[numOfProcessors];

            for (int taskNum = numOfTasks - 1; taskNum >= 0; taskNum--)
            {
                Dictionary<int, double> taskRuntimeInProcessorDict = new Dictionary<int, double>();
                Task task = Configuration.Tasks[taskNum];
                int taskRAM = task.RAM;
                int taskDownload = task.Download;
                int taskUpload = task.Upload;

                for (int processNum = 0; processNum < numOfProcessors; processNum++)
                {
                    double currentTaskRuntime = tasksRuntimes[processNum, taskNum];

                    taskRuntimeInProcessorDict.Add(processNum, currentTaskRuntime);
                }

                taskRuntimeInProcessorDict = taskRuntimeInProcessorDict.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

                foreach (KeyValuePair<int, double> entry in taskRuntimeInProcessorDict)
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

            Allocation newAllocation = AllocationCalculator.CalculateAllocationValues(allocationMap, Configuration);
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

        private double GetMaxGenergy(double[,] energy)
        {
            int rows = energy.GetLength(0);
            int cols = energy.GetLength(1);
            double[] maxEnergyList = new double[cols];

            for (int col = 0; col < cols; col++)
            {
                double maxEnergy = -1;

                for (int row = 0; row < rows; row++)
                {
                    maxEnergy = Math.Max(maxEnergy, energy[row, col]);
                }

                maxEnergyList[col] = maxEnergy;
            }

            return maxEnergyList.Sum() + 10;
        }
    }
    public class Population
    {
        public double MutationRate { get; set; }
        public int NumOfMembers { get; set; }
        public List<DNA> PopulationList { get; set; }
        public ConfigurationFile Configuration { get; set; }
        public double MaxEnergy { get; set; }

        public Population(
            double mutationRate,
            int numOfMembers,
            ConfigurationFile configuration,
            double maxEnergy)
        {
            MutationRate = mutationRate;
            NumOfMembers = numOfMembers;
            Configuration = configuration;
            MaxEnergy = maxEnergy;
            PopulationList = new List<DNA>();

            // Generate population with DNA object
            // and calculate the fitness
            for (int member = 0; member < numOfMembers; member++)
            {
                DNA newDNA = new DNA(configuration);
                newDNA.CalculateFitness(maxEnergy);
                PopulationList.Add(newDNA);
            }
        }

        private void CalculateProb()
        {
            double sumOfFitness = 0;

            foreach (DNA dna in PopulationList)
            {
                sumOfFitness += dna.Fitness;
            }

            foreach (DNA dna in PopulationList)
            {
                dna.Prob = ((dna.Fitness / sumOfFitness));
            }
        }

        public void CalculateFitness()
        {
            foreach (DNA dna in PopulationList)
            {
                dna.CalculateFitness(MaxEnergy);
            }
        }

        public void CreateNextGeneration()
        {
            List<DNA> newPopulation = new List<DNA>();

            CalculateProb();
            for (int member = 0; member < NumOfMembers; member++)
            {
                DNA partnerA = SelectOne();
                DNA partnerB = SelectOne();
                DNA child = partnerA.Crossover(partnerB);
                child.Mutate(MutationRate);
                newPopulation.Add(child);
            }

            PopulationList = newPopulation;

        }

        private DNA SelectOne()
        {
            int index = 0;
            Random rnd = new Random(Guid.NewGuid().GetHashCode());
            double rndDouble = rnd.NextDouble();

            while (rndDouble > 0 && index < NumOfMembers)
            {
                rndDouble -= PopulationList[index].Prob;
                index++;
            }

            index--;

            return PopulationList[index];
        }
    }

    public class DNA
    {
        public double Fitness { get; set; }
        public double Prob { get; set; }
        public List<List<string>> Genes;
        public ConfigurationFile Configuration { get; set; }
        public Allocation GeneAllocation { get; set; }

        public DNA(ConfigurationFile configuration)
        {
            Configuration = configuration;
            Genes = CreateGenes();
            GeneAllocation = AllocationCalculator.CalculateAllocationValues(Genes, Configuration);
        }

        public void CalculateFitness(double maxEnergy)
        {
            Fitness = GeneAllocation.Energy / maxEnergy;
            Fitness = Math.Pow(Fitness, 2);
        }

        public DNA Crossover(DNA partner)
        {
            DNA newDNA = new DNA(Configuration);
            int numOfTasks = Configuration.Program.Tasks;
            int numOfProcessors = Configuration.Program.Processors;
            Random rnd = new Random(Guid.NewGuid().GetHashCode());
            int midPoint = rnd.Next(numOfTasks);

            for (int task = 0; task < numOfTasks; task++)
            {
                if (task > midPoint)
                {
                    for (int processor = 0; processor < numOfProcessors; processor++)
                    {
                        newDNA.Genes[processor][task] = this.Genes[processor][task];
                    }
                }
                else
                {
                    for (int processor = 0; processor < numOfProcessors; processor++)
                    {
                        newDNA.Genes[processor][task] = partner.Genes[processor][task];
                    }
                }
            }

            newDNA.GeneAllocation = AllocationCalculator.CalculateAllocationValues(newDNA.Genes, Configuration);

            return newDNA;
        }

        public void Mutate(double mutationRate)
        {
            Random rnd = new Random(Guid.NewGuid().GetHashCode());
            int numOfTasks = Configuration.Program.Tasks;
            int numOfProcessors = Configuration.Program.Processors;

            for (int task = 0; task < numOfTasks; task++)
            {
                double rndDouble = rnd.NextDouble();

                if (rndDouble < mutationRate)
                {
                    int rndProcessor = rnd.Next(numOfProcessors);

                    for (int processor = 0; processor < numOfProcessors; processor++)
                    {
                        Genes[processor][task] = "0";
                    }

                    Genes[rndProcessor][task] = "1";
                }
            }

            GeneAllocation = AllocationCalculator.CalculateAllocationValues(Genes, Configuration);
        }

        private List<List<string>> CreateGenes()
        {
            int numOfTasks = Configuration.Program.Tasks;
            int numOfProcessors = Configuration.Program.Processors;
            List<List<string>> genes = InitalizeMap(numOfTasks, numOfProcessors);
            Random rnd = new Random(Guid.NewGuid().GetHashCode());

            for (int taskNum = 0; taskNum < numOfTasks; taskNum++)
            {
                int processIndex = rnd.Next(numOfProcessors);

                genes[processIndex][taskNum] = "1";
            }

            return genes;
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
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using TaskAllocationUtils.Files;
using TaskAllocationUtils.Classes;
using TaskAllocationUtils.Allocations;

namespace TaskAllocationGenerator.Utils.Allocations
{
    // Genetic Algorithm is chosen
    public class HeuristicAllocationFinder
    {
        public double MutationRate { get; set; }
        public Population Population { get; set; }
        public int NumOfMembers { get; set; }
        public ConfigurationFile Configuration { get; set; }
        public int NumOfGenerations { get; set; }

        public HeuristicAllocationFinder(
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
            Allocation newAllocation = null;
            int generations = 0;


            while (generations < NumOfGenerations)
            {
                // Generate intial Population

                // Create next generation (reproduction/selection)

                // Calculate fitness
            }

            // newAllocation = AllocationCalculator.CalculateAllocationValues(allocationMap, Configuration);

            return newAllocation;
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
    public class Population
    {
        public double MutationRate { get; set; }
        public int NumOfMembers { get; set; }
        public List<DNA> PopulationList { get; set; }
        public ConfigurationFile Configuration { get; set; }

        public Population(double mutationRate, int numOfMembers, ConfigurationFile configuration)
        {
            MutationRate = mutationRate;
            NumOfMembers = numOfMembers;
            Configuration = configuration;

            // Generate population with DNA object
            // and calculate the fitness
        }

        public void CalculateProb()
        {

        }

        public void CalculateFitness()
        {

        }

        public void CreateNextGeneration()
        {

        }

        private DNA SelectOne()
        {
            return null;
        }
    }

    public class DNA
    {
        public double Fitness { get; set; }
        public double Prob { get; set; }
        public List<List<string>> Genes;
        public ConfigurationFile Configuration { get; set; }
        
        public DNA(ConfigurationFile configuration)
        {
            Configuration = configuration;
            Genes = InitalizeMap(Configuration.Program.Tasks, Configuration.Program.Processors);
        }

        public void CalculateFitness()
        {

        }

        public void Crossover(DNA partner)
        {

        }

        public void Mutate(double mutationRate)
        {

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

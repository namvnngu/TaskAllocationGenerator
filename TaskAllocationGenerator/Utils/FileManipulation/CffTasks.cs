using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskAllocationGenerator.Utils.Classes;
using TaskAllocationGenerator.Utils.Constants;

namespace TaskAllocationGenerator.Utils.FileManipulation
{
    public class CffTasks
    {
        public PairSection TasksSection { get; set; }
        public List<Task> Tasks { get; set; }
        public CffTask CffTaskExtraction { get; set; }

        public CffTasks()
        {
            TasksSection = new PairSection(
                CffKeywords.OPENING_TASKS,
                CffKeywords.CLOSING_TASKS);
            Tasks = new List<Task>();
            CffTaskExtraction = new CffTask();
        }

        public List<Task> ExtractTasks(string line)
        {
            // Check whether the line starts opening/closing TASKS section
            // If yes, mark it exist
            TasksSection.MarkSection(line);

            // Extract Task data within TASK section
            if (TasksSection.ValidSectionPair[0] &&
                !TasksSection.ValidSectionPair[1])
            {
                Task task;

                // Check whether the reader goes within the TASK section
                CffTaskExtraction.MarkInsideTask(line);

                task = CffTaskExtraction.ExtractTask(line);
                if (task != null)
                {
                    Tasks.Add(task);
                }
            }

            return Tasks;
        }

    }
}

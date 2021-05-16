using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskAllocationUtils.Classes;
using TaskAllocationUtils.Constants;
using System.Runtime.Serialization;

namespace TaskAllocationUtils.FileManipulation
{
    [DataContract]
    public class CffTasks
    {
        [DataMember]
        public PairSection TasksSection { get; set; }
        [DataMember]
        public List<Task> Tasks { get; set; }
        [DataMember]
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

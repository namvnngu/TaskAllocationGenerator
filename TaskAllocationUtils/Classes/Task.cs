using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskAllocationUtils.Constants;
using System.Runtime.Serialization;


namespace TaskAllocationUtils.Classes
{
    [DataContract]
    public class Task
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public double Runtime { get; set; }
        [DataMember]
        public double ReferenceFrequency { get; set; }
        [DataMember]
        public int RAM { get; set; }
        [DataMember]
        public int Download { get; set; }
        [DataMember]
        public int Upload { get; set; }

        public Task()
        {
            ID = -1;
            Runtime = -1;
            ReferenceFrequency = -1;
            RAM = -1;
            Download = -1;
            Upload = -1;
        }

        public Task(
            int id,
            int runtime,
            int referenceFrequency,
            int ram,
            int download,
            int upload)
        {
            ID = id;
            Runtime = runtime;
            ReferenceFrequency = referenceFrequency;
            RAM = ram;
            Download = download;
            Upload = upload;
        }

        /// <summary>
        /// The method compute the amount of runtime based on
        /// the frequency of the process in which the task is allocated
        /// </summary>
        public double CalculateRuntime(double processorFrequency)
        {
            double taskRuntimeInProcessor;

            taskRuntimeInProcessor = Runtime * (ReferenceFrequency / processorFrequency);

            return taskRuntimeInProcessor;
        }

        public override string ToString()
        {
            StringBuilder text = new StringBuilder();

            text.AppendLine($"TASK-{CffKeywords.TASK_ID}={ID}");
            text.AppendLine($"TASK-{CffKeywords.TASK_RUNTIME}={Runtime}");
            text.AppendLine($"TASK-{CffKeywords.TASK_REFERENCE_FREQUENCY}={ReferenceFrequency}");
            text.AppendLine($"TASK-{CffKeywords.TASK_RAM}={RAM}");
            text.AppendLine($"TASK-{CffKeywords.TASK_DOWNLOAD}={Download}");
            text.AppendLine($"TASK-{CffKeywords.TASK_UPLOAD}={Upload}");

            return text.ToString();
        }
    }
}

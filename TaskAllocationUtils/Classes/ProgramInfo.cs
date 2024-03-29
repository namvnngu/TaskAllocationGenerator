﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskAllocationUtils.Constants;
using System.Runtime.Serialization;

namespace TaskAllocationUtils.Classes
{
    /// <summary>
    /// The class provides the number of tasks,
    /// the number of processors and the maximum duration
    /// </summary>
    [DataContract]
    public class ProgramInfo
    {
        [DataMember]
        public double Duration { get; set; }
        [DataMember]
        public int Tasks { get; set; }
        [DataMember]
        public int Processors { get; set; }

        public ProgramInfo()
        {
            Duration = -1;
            Tasks = -1;
            Processors = -1;
        }

        public ProgramInfo(double duration, int tasks, int processors)
        {
            Duration = duration;
            Tasks = tasks;
            Processors = processors;
        }

        public override string ToString()
        {
            StringBuilder text = new StringBuilder();

            text.AppendLine($"PROGRAM-{CffKeywords.PROGRAM_DURATION}={Duration}");
            text.AppendLine($"PROGRAM-{CffKeywords.PROGRAM_TASKS}={Tasks}");
            text.AppendLine($"PROGRAM-{CffKeywords.PROGRAM_PROCESSORS}={Processors}");

            return text.ToString();
        }
    }
}

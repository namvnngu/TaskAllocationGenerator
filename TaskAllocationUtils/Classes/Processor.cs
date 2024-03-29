﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskAllocationUtils.Constants;
using System.Runtime.Serialization;

namespace TaskAllocationUtils.Classes
{
    [DataContract]
    public class Processor
    {
        [DataMember]
        public int ID { set; get; }
        [DataMember]
        public string Type { get; set; }
        [DataMember]
        public ProcessorType PType { get; set; }
        [DataMember]
        public double Frequency { get; set; }
        [DataMember]
        public int RAM { get; set; }
        [DataMember]
        public int Download { get; set; }
        [DataMember]
        public int Upload { get; set; }

        public Processor()
        {
            ID = -1;
            Type = null;
            PType = null;
            Frequency = -1;
            RAM = -1;
            Download = -1;
            Upload = -1;
        }

        public Processor(
            int id,
            string type,
            double frequency,
            int ram,
            int download,
            int upload)
        {
            ID = id;
            Type = type;
            Frequency = frequency;
            RAM = ram;
            Download = download;
            Upload = upload;
        }

        /// <summary>
        /// Determining whether the amount of RAM required by a task 
        /// is less than or equal to the amount of RAM associated
        /// with a processor.
        /// </summary>
        public bool IsRamSufficient(Task task)
        {
            if (task.RAM > RAM)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Determining whether the amount of download speed 
        /// required by a task is less than or equal to 
        /// the amount of download speed provided by a processor.
        /// </summary>
        public bool IsDownloadSufficient(Task task)
        {
            if (task.Download > Download)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Determining whether the amount of upload speed 
        /// required by a task is less than or equal to 
        /// the amount of upload speed provided by a processor.
        /// </summary>
        public bool IsUploadSufficient(Task task)
        {
            if (task.Upload > Upload)
            {
                return false;
            }

            return true;
        }

        public override string ToString()
        {
            StringBuilder text = new StringBuilder();

            text.AppendLine($"PROCESSOR-{CffKeywords.PROCESSOR_ID}={ID}");
            text.AppendLine($"PROCESSOR-{CffKeywords.PROCESSOR_TYPE}={Type}");
            text.AppendLine($"PROCESSOR-{CffKeywords.PROCESSOR_FREQUENCY}={Frequency}");
            text.AppendLine($"PROCESSOR-{CffKeywords.PROCESSOR_RAM}={RAM}");
            text.AppendLine($"PROCESSOR-{CffKeywords.PROCESSOR_DOWNLOAD}={Download}");
            text.AppendLine($"PROCESSOR-{CffKeywords.PROCESSOR_UPLOAD}={Upload}");

            return text.ToString();
        }
    }
}

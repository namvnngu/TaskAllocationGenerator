﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskAllocationUtils.Constants;
using TaskAllocationUtils.Classes;
using System.Runtime.Serialization;

namespace TaskAllocationUtils.FileManipulation
{
    [DataContract]
    public class CffLogFile
    {
        [DataMember]
        public PairSection LogFileSection { get; set; }
        [DataMember]
        public string LogFile { get; set; }
        [DataMember]
        public string LOGFILE_EXTENSION = "txt";

        public CffLogFile()
        {
            LogFileSection = new PairSection(
                CffKeywords.OPENING_LOGFILE,
                CffKeywords.CLOSING_LOGFILE);
            LogFile = null;
        }

        public string ExtractLogFile(string line)
        {
            // Check whether the line starts opening/closing LOGFILE section
            // If yes, mark it exist
            LogFileSection.MarkSection(line);

            bool openingSectionVisited = LogFileSection.ValidSectionPair[0];

            // Check the line start with the expected keyword, "DEFAULT"
            if (openingSectionVisited && line.StartsWith(CffKeywords.DEFAULT_LOGFILE))
            {
                LogFile = Extractor.ExtractString(line);
            }

            return LogFile;
        }
    }
}

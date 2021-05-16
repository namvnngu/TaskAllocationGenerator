using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskAllocationUtils.Classes;
using TaskAllocationUtils.Constants;
using System.Runtime.Serialization;

namespace TaskAllocationUtils.FileManipulation
{
    [DataContract]
    public class CffLimits
    {
        [DataMember]
        public Limits LimitData { get; set; }
        [DataMember]
        public PairSection LimitPairSection { get; set; }

        public CffLimits()
        {
            LimitData = new Limits();
            LimitPairSection = new PairSection(
                CffKeywords.OPENING_LIMITS,
                CffKeywords.CLOSING_LIMITS);
        }

        public Limits ExtractLimitData(string line)
        {
            // Check whether the line starts opening/closing LIMITS section
            // If yes, mark it exist
            LimitPairSection.MarkSection(line);

            bool openingSectionVisited = LimitPairSection.ValidSectionPair[0];

            if (openingSectionVisited)
            {
                ExtractMinimumTasks(line);
                ExtractMaximumTasks(line);
                ExtractMinimumProcessors(line);
                ExtractMaximumProcessors(line);
                ExtractMinimumProcessorFrequencies(line);
                ExtractMaximumProcessorFrequencies(line);
                ExtractMinimumRAM(line);
                ExtractMaximumRAM(line);
                ExtractMinimumDownload(line);
                ExtractMaximumDownload(line);
                ExtractMinimumUpload(line);
                ExtractMaximumUpload(line);
            }

            return LimitData;
        }

        private void ExtractMinimumTasks(string line)
        {
            if (LimitData.MinimumTasks < 0 && line.StartsWith(CffKeywords.MINIMUM_TASKS))
            {
                LimitData.MinimumTasks = Extractor.ExtractInteger(line);
            }
        }

        public void ExtractMaximumTasks(string line)
        {
            if (LimitData.MaximumTasks < 0 && line.StartsWith(CffKeywords.MAXIMUM_TASKS))
            {
                LimitData.MaximumTasks = Extractor.ExtractInteger(line);
            }
        }

        public void ExtractMinimumProcessors(string line)
        {
            if (LimitData.MinimumProcessors < 0 && line.StartsWith(CffKeywords.MINIMUM_PROCESSORS))
            {
                LimitData.MinimumProcessors = Extractor.ExtractInteger(line);
            }
        }

        public void ExtractMaximumProcessors(string line)
        {
            if (LimitData.MaximumProcessors < 0 && line.StartsWith(CffKeywords.MAXIMUM_PROCESSORS))
            {
                LimitData.MaximumProcessors = Extractor.ExtractInteger(line);
            }
        }

        public void ExtractMinimumProcessorFrequencies(string line)
        {
            if (LimitData.MinimumProcessorsFrequencies < 0 && line.StartsWith(CffKeywords.MINIMUM_PROCESSOR_FREQUENCIES))
            {
                LimitData.MinimumProcessorsFrequencies = Extractor.ExtractDouble(line);
            }
        }

        public void ExtractMaximumProcessorFrequencies(string line)
        {
            if (LimitData.MaximumProcessorsFrequencies < 0 && line.StartsWith(CffKeywords.MAXIMUM_PROCESSOR_FREQUENCIES))
            {
                LimitData.MaximumProcessorsFrequencies = Extractor.ExtractDouble(line);
            }
        }

        public void ExtractMinimumRAM(string line)
        {
            if (LimitData.MinimumRAM < 0 && line.StartsWith(CffKeywords.MINIMUM_RAM))
            {
                LimitData.MinimumRAM = Extractor.ExtractInteger(line);
            }
        }

        public void ExtractMaximumRAM(string line)
        {
            if (LimitData.MaximumRAM < 0 && line.StartsWith(CffKeywords.MAXIMUM_RAM))
            {
                LimitData.MaximumRAM = Extractor.ExtractInteger(line);
            }
        }

        public void ExtractMinimumDownload(string line)
        {
            if (LimitData.MinimumDownload < 0 && line.StartsWith(CffKeywords.MINIMUM_DOWNLOAD))
            {
                LimitData.MinimumDownload = Extractor.ExtractInteger(line);
            }
        }

        public void ExtractMaximumDownload(string line)
        {
            if (LimitData.MaximumDownload < 0 && line.StartsWith(CffKeywords.MAXINUM_DOWNLOAD))
            {
                LimitData.MaximumDownload = Extractor.ExtractInteger(line);
            }
        }

        public void ExtractMinimumUpload(string line)
        {
            if (LimitData.MinimumUpload < 0 && line.StartsWith(CffKeywords.MINIMUM_UPLOAD))
            {
                LimitData.MinimumUpload = Extractor.ExtractInteger(line);
            }
        }

        public void ExtractMaximumUpload(string line)
        {
            if (LimitData.MaximumUpload < 0 && line.StartsWith(CffKeywords.MAXIMUM_UPLOAD))
            {
                LimitData.MaximumUpload = Extractor.ExtractInteger(line);
            }
        }
    }
}

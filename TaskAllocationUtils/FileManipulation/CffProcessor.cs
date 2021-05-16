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
    public class CffProcessor
    {
        [DataMember]
        public Processor ProcessorInfo { get; set; }
        [DataMember]
        public bool InsideProcessor { get; set; }

        public CffProcessor()
        {
            ProcessorInfo = new Processor();
            InsideProcessor = false;
        }

        public void MarkInsideProcessor(string line)
        {
            if (line == CffKeywords.OPENING_PROCESSOR)
            {
                InsideProcessor = true;
            }

            if (line == CffKeywords.CLOSING_PROCESSOR)
            {
                // Reset Processor data
                InsideProcessor = false;
                ProcessorInfo = new Processor();
            }
        }

        public Processor ExtractProcessor(string line)
        {
            if (InsideProcessor)
            {
                ExtractProcessorID(line);
                ExtractProcessorType(line);
                ExtractProcessorFrequency(line);
                ExtractProcessorRAM(line);
                ExtractProcessorDownload(line);
                ExtractProcessorUpload(line);

                bool validProcessorInfo = CheckValidProcessorInfo();

                if (validProcessorInfo)
                {
                    return ProcessorInfo;
                }
            }

            return null;
        }

        private void ExtractProcessorID(string line)
        {
            if (ProcessorInfo.ID < 0 && line.StartsWith(CffKeywords.PROCESSOR_ID))
            {
                ProcessorInfo.ID = Extractor.ExtractInteger(line);
            }
        }

        private void ExtractProcessorType(string line)
        {
            if (ProcessorInfo.Type == null && line.StartsWith(CffKeywords.PROCESSOR_TYPE))
            {
                ProcessorInfo.Type = Extractor.ExtractString(line);
            }
        }

        private void ExtractProcessorFrequency(string line)
        {
            if (ProcessorInfo.Frequency < 0 && line.StartsWith(CffKeywords.PROCESSOR_FREQUENCY))
            {
                ProcessorInfo.Frequency = Extractor.ExtractDouble(line);
            }
        }

        private void ExtractProcessorRAM(string line)
        {
            if (ProcessorInfo.RAM < 0 && line.StartsWith(CffKeywords.PROCESSOR_RAM))
            {
                ProcessorInfo.RAM = Extractor.ExtractInteger(line);
            }
        }

        private void ExtractProcessorDownload(string line)
        {
            if (ProcessorInfo.Download < 0 && line.StartsWith(CffKeywords.PROCESSOR_DOWNLOAD))
            {
                ProcessorInfo.Download = Extractor.ExtractInteger(line);
            }
        }

        private void ExtractProcessorUpload(string line)
        {
            if (ProcessorInfo.Upload < 0 && line.StartsWith(CffKeywords.PROCESSOR_UPLOAD))
            {
                ProcessorInfo.Upload = Extractor.ExtractInteger(line);
            }
        }

        private bool CheckValidProcessorInfo()
        {
            return (
                ProcessorInfo.ID != -1 &&
                ProcessorInfo.Type != null &&
                ProcessorInfo.Frequency != -1 &&
                ProcessorInfo.RAM != -1 &&
                ProcessorInfo.Download != -1 &&
                ProcessorInfo.Upload != -1);
        }
    }
}

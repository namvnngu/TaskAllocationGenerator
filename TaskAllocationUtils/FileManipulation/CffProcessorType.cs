using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskAllocationUtils.Classes;
using TaskAllocationUtils.Constants;

namespace TaskAllocationUtils.FileManipulation
{
    public class CffProcessorType
    {
        public ProcessorType ProcessorTypeInfo { get; set; }
        public bool InsideProcessorType { get; set; }

        public CffProcessorType()
        {
            ProcessorTypeInfo = new ProcessorType();
            InsideProcessorType = false;
        }

        public void MarkInsideProcessorType(string line)
        {
            if (line == CffKeywords.OPENING_PROCESSOR_TYPE)
            {
                InsideProcessorType = true;
            }

            if (line == CffKeywords.CLOSING_PROCESSOR_TYPE)
            {
                // Reset Processor Type data
                InsideProcessorType = false;
                ProcessorTypeInfo = new ProcessorType();
            }
        }

        public ProcessorType ExtractProcessType(string line)
        {
            if (InsideProcessorType)
            {
                ExtractProcessTypeName(line);
                ExtractProcessTypeC2(line);
                ExtractProcessTypeC1(line);
                ExtractProcessTypeC0(line);

                bool validProcessTypeInfo = CheckValidProcessorTypeInfo();

                if (validProcessTypeInfo)
                {
                    return ProcessorTypeInfo;
                }
            }

            return null;
        }

        private void ExtractProcessTypeName(string line)
        {
            if (ProcessorTypeInfo.Name == null && line.StartsWith(CffKeywords.PROCESSOR_TYPE_NAME))
            {
                ProcessorTypeInfo.Name = Extractor.ExtractString(line);
            }
        }

        private void ExtractProcessTypeC2(string line)
        {
            if (ProcessorTypeInfo.C2 < 0 && line.StartsWith(CffKeywords.PROCESSOR_TYPE_C2))
            {
                ProcessorTypeInfo.C2 = Extractor.ExtractDouble(line);
            }
        }

        private void ExtractProcessTypeC1(string line)
        {
            if (ProcessorTypeInfo.C1 < 0 && line.StartsWith(CffKeywords.PROCESSOR_TYPE_C1))
            {
                ProcessorTypeInfo.C1 = Extractor.ExtractDouble(line);
            }
        }

        private void ExtractProcessTypeC0(string line)
        {
            if (ProcessorTypeInfo.C0 < 0 && line.StartsWith(CffKeywords.PROCESSOR_TYPE_C0))
            {
                ProcessorTypeInfo.C0 = Extractor.ExtractDouble(line);
            }
        }

        private bool CheckValidProcessorTypeInfo()
        {
            return (
                ProcessorTypeInfo.Name != null &&
                ProcessorTypeInfo.C2 != -1 &&
                ProcessorTypeInfo.C1 != -1 &&
                ProcessorTypeInfo.C0 != -1);
        }
    }
}

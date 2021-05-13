using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskAllocationUtils.Classes;
using TaskAllocationUtils.Constants;

namespace TaskAllocationUtils.FileManipulation
{
    public class CffProcessorTypes
    {
        public PairSection ProcessorTypesSection { get; set; }
        public List<ProcessorType> ProcessorTypes { get; set; }
        public CffProcessorType CffProcessorTypeExtraction { get; set; }

        public CffProcessorTypes()
        {
            ProcessorTypesSection = new PairSection(
                CffKeywords.OPENING_PROCESSOR_TYPES,
                CffKeywords.CLOSING_PROCESSOR_TYPES);
            ProcessorTypes = new List<ProcessorType>();
            CffProcessorTypeExtraction = new CffProcessorType();
        }

        public List<ProcessorType> ExtractProcessorTypes(string line)
        {
            // Check whether the line starts opening/closing PROCESSOR-TYPES section
            // If yes, mark it exist
            ProcessorTypesSection.MarkSection(line);

            // Extract Processor Type data within PROCESSOR-TYPE section
            if (ProcessorTypesSection.ValidSectionPair[0] &&
                !ProcessorTypesSection.ValidSectionPair[1])
            {
                ProcessorType processorType;

                // Check whether the reader goes within the PROCESSOR-TYPE section
                CffProcessorTypeExtraction.MarkInsideProcessorType(line);

                processorType = CffProcessorTypeExtraction.ExtractProcessType(line);

                if (processorType != null)
                {
                    ProcessorTypes.Add(processorType);
                }
            }

            return ProcessorTypes;
        }
    }
}

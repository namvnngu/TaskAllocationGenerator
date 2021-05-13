using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskAllocationUtils.Classes;
using TaskAllocationUtils.Constants;

namespace TaskAllocationUtils.FileManipulation
{
    public class CffProcessors
    {
        public PairSection ProcessorsSection { get; set; }
        public List<Processor> Processors { get; set; }
        public CffProcessor CffProcessorkExtraction { get; set; }

        public CffProcessors()
        {
            ProcessorsSection = new PairSection(
                CffKeywords.OPENING_PROCESSORS,
                CffKeywords.CLOSING_PROCESSORS);
            Processors = new List<Processor>();
            CffProcessorkExtraction = new CffProcessor();
        }

        public List<Processor> ExtractProcessors(string line)
        {
            // Check whether the line starts opening/closing PROCESSORS section
            // If yes, mark it exist
            ProcessorsSection.MarkSection(line);

            // Extract Processor data within PROCESSOR section
            if (ProcessorsSection.ValidSectionPair[0] &&
                !ProcessorsSection.ValidSectionPair[1])
            {
                Processor processor;

                // Check whether the reader goes within the PROCESSOR section
                CffProcessorkExtraction.MarkInsideProcessor(line);

                processor = CffProcessorkExtraction.ExtractProcessor(line);
                if (processor != null)
                {
                    Processors.Add(processor);
                }
            }

            return Processors;
        }
    }
}

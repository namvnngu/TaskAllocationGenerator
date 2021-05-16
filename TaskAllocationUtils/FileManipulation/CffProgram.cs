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
    public class CffProgram
    {
        [DataMember]
        public ProgramInfo Program { get; set; }
        [DataMember]
        public PairSection ProgramPairSection { get; set; }

        public CffProgram()
        {
            Program = new ProgramInfo();
            ProgramPairSection = new PairSection(
                CffKeywords.OPENING_PROGRAM,
                CffKeywords.CLOSING_PROGRAM);
        }

        public ProgramInfo ExtractProgramData(string line)
        {
            // Check whether the line starts opening/closing PROGRAM section
            // If yes, mark it exist
            ProgramPairSection.MarkSection(line);

            bool openingSectionVisited = ProgramPairSection.ValidSectionPair[0];

            if (openingSectionVisited)
            {
                ExtractDuration(line);
                ExtractTasks(line);
                ExtractProcessors(line);
            }

            return Program;
        }

        public void ExtractDuration(string line)
        {
            if (Program.Duration < 0 && line.StartsWith(CffKeywords.PROGRAM_DURATION))
            {
                Program.Duration = Extractor.ExtractDouble(line);
            }
        }

        public void ExtractTasks(string line)
        {
            if (Program.Tasks < 0 && line.StartsWith(CffKeywords.PROGRAM_TASKS))
            {
                Program.Tasks = Extractor.ExtractInteger(line);
            }
        }

        public void ExtractProcessors(string line)
        {
            if (Program.Processors < 0 && line.StartsWith(CffKeywords.PROGRAM_PROCESSORS))
            {
                Program.Processors = Extractor.ExtractInteger(line);
            }
        }
    }
}

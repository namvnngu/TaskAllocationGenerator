using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskAllocationUtils.Classes;
using TaskAllocationUtils.Constants;

namespace TaskAllocationUtils.FileManipulation
{
    public class CffLocalCommunication
    {
        public LocalCommunication LocalCommunication { get; set; }
        public PairSection LocalCommunicationSection { get; set; }

        public CffLocalCommunication()
        {
            LocalCommunication = new LocalCommunication();
            LocalCommunicationSection = new PairSection(
                CffKeywords.OPENING_LOCAL_COMMUNICATION,
                CffKeywords.CLOSING_LOCAL_COMMUNICATION);
        }

        public LocalCommunication ExtractLocalCommunication(string line, int numOfTasks)
        {
            // Check whether the line starts opening/closing LOCAL-COMMUNICATION section
            // If yes, mark it exist
            LocalCommunicationSection.MarkSection(line);

            bool openingSectionVisited = LocalCommunicationSection.ValidSectionPair[0];

            if (openingSectionVisited)
            {
                ExtractMapData(line, numOfTasks);
            }

            return LocalCommunication;
        }

        private void ExtractMapData(string line, int numOfTasks)
        {

            if (LocalCommunication.MapMatrix == null && line.StartsWith(CffKeywords.LOCAL_COMMUNICATION_MAP))
            {
                string returnedMap = Extractor.ExtractString(line);

                if (returnedMap != null)
                {
                    LocalCommunication.MapData = new Map(returnedMap);
                    LocalCommunication.MapMatrix = LocalCommunication.MapData.ConvertToMatrix(
                        numOfTasks,
                        numOfTasks);
                }
            }
        }
    }
}

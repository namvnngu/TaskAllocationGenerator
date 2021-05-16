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
    public class CffRemoteCommunication
    {
        [DataMember]
        public RemoteCommunication RemoteCommunication { get; set; }
        [DataMember]
        public PairSection RemoteCommunicationSection { get; set; }

        public CffRemoteCommunication()
        {
            RemoteCommunication = new RemoteCommunication();
            RemoteCommunicationSection = new PairSection(
                CffKeywords.OPENING_REMOTE_COMMUNICATION,
                CffKeywords.CLOSING_REMOTE_COMMUNICATION);
        }

        public RemoteCommunication ExtractRemoteCommunication(string line, int numOfTasks)
        {
            // Check whether the line starts opening/closing REMOTE-COMMUNICATION section
            // If yes, mark it exist
            RemoteCommunicationSection.MarkSection(line);

            bool openingSectionVisited = RemoteCommunicationSection.ValidSectionPair[0];

            if (openingSectionVisited)
            {
                ExtractMapData(line, numOfTasks);
            }

            return RemoteCommunication;
        }

        private void ExtractMapData(string line, int numOfTasks)
        {
            if (RemoteCommunication.MapMatrix == null && line.StartsWith(CffKeywords.REMOTE_COMMUNICATION_MAP))
            {
                string returnedMap = Extractor.ExtractString(line);

                if (returnedMap != null)
                {
                    RemoteCommunication.MapData = new Map(returnedMap);
                    RemoteCommunication.MapMatrix = RemoteCommunication.MapData.ConvertToMatrix(
                        numOfTasks,
                        numOfTasks);
                }
            }
        }
    }
}

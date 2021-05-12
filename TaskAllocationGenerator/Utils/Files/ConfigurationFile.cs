using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using TaskAllocationGenerator.Utils.Classes;

namespace TaskAllocationGenerator.Utils.Files
{
    public class ConfigurationFile
    {
        public string Source { get; set; }
        public string LogFilename { get; set; }
        public Limits LimitData { get; set; }
        public ProgramInfo Program { get; set; }
        public List<Classes.Task> Tasks { get; set; }
        public List<Processor> Processors { get; set; }
        public List<ProcessorType> ProcessorTypes { get; set; }
        public LocalCommunication LocalCommunicationInfo { get; set; }
        public RemoteCommunication RemoteCommunicationInfo { get; set; }

        public ConfigurationFile()
        {
            Source = null;
            LogFilename = null;
        }

        public ConfigurationFile(string source)
        {
            Source = source;
        }

        public void ReadAndExtractData()
        {
            if (Source == null)
            {
                return;
            }

            WebClient webClient = new WebClient();
            Stream stream = webClient.OpenRead(Source);
            StreamReader streamReader = new StreamReader(stream);
            string line;

            while (!streamReader.EndOfStream)
            {

                line = streamReader.ReadLine();
                line = line.Trim();
            }

            streamReader.Close();
        }
    }
}

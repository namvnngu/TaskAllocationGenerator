using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;

namespace TaskAllocationGenerator.Utils.Files
{
    class ConfigurationFile
    {
        public string Source { get; set; }

        public ConfigurationFile()
        {

        }

        public ConfigurationFile(string source)
        {
            Source = source;
        }

        public void ReadAndExtractData()
        {
            WebClient webClient = new WebClient();
            Stream stream = webClient.OpenRead(Source);
            StreamReader streamReader = new StreamReader(stream);

            while (!streamReader.EndOfStream)
            {
                Console.WriteLine(streamReader.ReadLine());
            }

            streamReader.Close();
        }
    }
}

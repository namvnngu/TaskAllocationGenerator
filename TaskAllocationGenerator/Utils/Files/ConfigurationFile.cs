using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using TaskAllocationGenerator.Utils.Classes;
using TaskAllocationGenerator.Utils.FileManipulation;
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
            CffLogFile cffLogFile = new CffLogFile();
            CffLimits cffLimits = new CffLimits();
            CffProgram cffProgram = new CffProgram();
            CffTasks cffTasks = new CffTasks();
            string line;

            while (!streamReader.EndOfStream)
            {

                line = streamReader.ReadLine();
                line = line.Trim();

                // Extract and validate the LOGFILE section
                // If the LOGFILE section is already visited, then ignore
                if (!cffLogFile.LogFileSection.ValidSectionPair[1])
                {
                    LogFilename = cffLogFile.ExtractLogFile(line);
                }

                // Extract and validate the LIMITS section
                // If the LIMITS sections is already visited, then ignore
                if (!cffLimits.LimitPairSection.ValidSectionPair[1])
                {
                    LimitData = cffLimits.ExtractLimitData(line);
                }

                // Extract and validate the PROGRAM section
                // If the LIMITS sections is already visited, then ignore
                if (!cffProgram.ProgramPairSection.ValidSectionPair[1])
                {
                    Program = cffProgram.ExtractProgramData(line);
                }

                // Extract and validate the TASKS section
                // If the TASKS sections is already visited, then ignore
                if (!cffTasks.TasksSection.ValidSectionPair[1])
                {
                    Tasks = cffTasks.ExtractTasks(line);
                }
            }

            foreach(var task in Tasks)
            {
                Console.WriteLine(task);
            }

            streamReader.Close();
        }
    }
}

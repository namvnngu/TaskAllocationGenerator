using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using TaskAllocationUtils.Classes;
using TaskAllocationUtils.FileManipulation;
using System.Runtime.Serialization;

namespace TaskAllocationUtils.Files
{
    [DataContract]
    public class ConfigurationFile
    {
        [DataMember]
        public string Source { get; set; }
        [DataMember]
        public string LogFilename { get; set; }
        [DataMember]
        public Limits LimitData { get; set; }
        [DataMember]
        public ProgramInfo Program { get; set; }
        [DataMember]
        public List<Classes.Task> Tasks { get; set; }
        [DataMember]
        public List<Processor> Processors { get; set; }
        [DataMember]
        public List<ProcessorType> ProcessorTypes { get; set; }
        [DataMember]
        public LocalCommunication LocalCommunicationInfo { get; set; }
        [DataMember]
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
            CffProcessorTypes cffProcessorTypes = new CffProcessorTypes();
            CffProcessors cffProcessors = new CffProcessors();
            CffLocalCommunication cffLocalCommunication = new CffLocalCommunication();
            CffRemoteCommunication cffRemoteCommunication = new CffRemoteCommunication();
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

                // Extract and validate the PROCESSORS section
                // If the PROCESSORS sections is already visited, then ignore
                if (!cffProcessors.ProcessorsSection.ValidSectionPair[1])
                {
                    Processors = cffProcessors.ExtractProcessors(line);
                }

                // Extract and validate the PROCESSOR-TYPES section
                // If the PROCESSOR-TYPES sections is already visited, then ignore
                if (!cffProcessorTypes.ProcessorTypesSection.ValidSectionPair[1])
                {
                    ProcessorTypes = cffProcessorTypes.ExtractProcessorTypes(line);
                }

                // Extract and validate the LOCAL-COMMUNICATION section
                // If the LOCAL-COMMUNICATION sections is already visited, then ignore
                if (!cffLocalCommunication.LocalCommunicationSection.ValidSectionPair[1])
                {
                    LocalCommunicationInfo = cffLocalCommunication.ExtractLocalCommunication(line, Program.Tasks);
                }

                // Extract and validate the REMOTE-COMMUNICATION section
                // If the REMOTE-COMMUNICATION sections is already visited, then ignore
                if (!cffRemoteCommunication.RemoteCommunicationSection.ValidSectionPair[1])
                {
                    RemoteCommunicationInfo = cffRemoteCommunication.ExtractRemoteCommunication(line, Program.Tasks);
                }
            }

            // Assign the corresponding Processor Type object to Processor
            AssignProcessType();

            streamReader.Close();
        }

        /// <summary>
        /// The method assigns process types to appropriate,
        /// corresponding processors.
        /// </summary>
        private void AssignProcessType()
        {
            if (ProcessorTypes.Count != 0 && Processors.Count != 0)
            {
                foreach (Processor processor in Processors)
                {
                    foreach (ProcessorType processorType in ProcessorTypes)
                    {
                        if (processor.Type == processorType.Name)
                        {
                            processor.PType = processorType;
                        }
                    }
                }
            }
        }
    }
}

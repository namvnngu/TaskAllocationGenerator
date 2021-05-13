using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskAllocationUtils.Classes;
using TaskAllocationUtils.Constants;

namespace TaskAllocationUtils.FileManipulation
{
    public class CffTask
    {
        public Task TaskInfo { get; set; }
        public bool InsideTask { get; set; }

        public CffTask()
        {
            TaskInfo = new Task();
            InsideTask = false;
        }

        public void MarkInsideTask(string line)
        {
            if (line == CffKeywords.OPENING_TASK)
            {
                InsideTask = true;
            }

            if (line == CffKeywords.CLOSING_TASK)
            {
                // Reset Task data
                InsideTask = false;
                TaskInfo = new Task();
            }
        }

        public Task ExtractTask(string line)
        {
            if (InsideTask)
            {
                ExtractTaskID(line);
                ExtractTaskRuntime(line);
                ExtractTaskReferenceFrequency(line);
                ExtractTaskRAM(line);
                ExtractTaskDonwload(line);
                ExtractTaskUpload(line);

                bool validTaskInfo = CheckValidTaskInfo();

                if (validTaskInfo)
                {
                    return TaskInfo;
                }
            }

            return null;
        }

        private void ExtractTaskID(string line)
        {
            if (TaskInfo.ID < 0 && line.StartsWith(CffKeywords.TASK_ID))
            {
                TaskInfo.ID = Extractor.ExtractInteger(line);
            }
        }

        private void ExtractTaskRuntime(string line)
        {
            if (TaskInfo.Runtime < 0 && line.StartsWith(CffKeywords.TASK_RUNTIME))
            {
                TaskInfo.Runtime = Extractor.ExtractDouble(line);
            }
        }

        private void ExtractTaskReferenceFrequency(string line)
        {
            if (TaskInfo.ReferenceFrequency < 0 && line.StartsWith(CffKeywords.TASK_REFERENCE_FREQUENCY))
            {
                TaskInfo.ReferenceFrequency = Extractor.ExtractDouble(line);
            }
        }

        private void ExtractTaskRAM(string line)
        {
            if (TaskInfo.RAM < 0 && line.StartsWith(CffKeywords.TASK_RAM))
            {
                TaskInfo.RAM = Extractor.ExtractInteger(line);
            }
        }

        private void ExtractTaskDonwload(string line)
        {
            if (TaskInfo.Download < 0 && line.StartsWith(CffKeywords.TASK_DOWNLOAD))
            {
                TaskInfo.Download = Extractor.ExtractInteger(line);
            }
        }

        private void ExtractTaskUpload(string line)
        {
            if (TaskInfo.Upload < 0 && line.StartsWith(CffKeywords.TASK_UPLOAD))
            {
                TaskInfo.Upload = Extractor.ExtractInteger(line);
            }
        }

        private bool CheckValidTaskInfo()
        {
            return (
                TaskInfo.ID != -1 &&
                TaskInfo.Runtime != -1 &&
                TaskInfo.ReferenceFrequency != -1 &&
                TaskInfo.RAM != -1 &&
                TaskInfo.Download != -1 &&
                TaskInfo.Upload != -1);
        }
    }
}

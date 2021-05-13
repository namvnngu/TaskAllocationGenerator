using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskAllocationUtils.Constants;

namespace TaskAllocationUtils.FileManipulation
{
    public class Extractor
    {
        public static string ExtractString(string line)
        {
            string stringValue = "";
            string[] lineData = line.Split(Symbols.EQUALITY);

            stringValue = lineData[1];
            stringValue = stringValue.Trim(Symbols.DOUBLE_QUOTE);

            return stringValue;
        } 

        public static int ExtractInteger(string line)
        {
            int integer = -1;
            string integerValue = ExtractString(line);

            integer = Convert.ToInt32(integerValue);

            return integer;
        }

        public static double ExtractDouble(string line)
        {
            double returnedDouble = 0.0;
            string doubleValue = ExtractString(line);

            returnedDouble = Convert.ToDouble(doubleValue);

            return returnedDouble;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using TaskAllocationUtils.Constants;

namespace TaskAllocationUtils.Classes
{
    [DataContract]
    public class Allocation
    {
        // Allocation = Allocation + AllocationDisplay
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public double Runtime { get; set; }
        [DataMember]
        public double Energy { get; set; }
        [DataMember]
        public List<List<string>> MapMatrix { get; set; }
        [DataMember]
        public List<AllocationProcessor> ProcessorAllocations { get; set; }

        public Allocation()
        {

        }


        public static string ConvertToMap(List<List<string>> mapMatrix)
        {
            int rows = mapMatrix.Count;
            int columns = mapMatrix[0].Count;
            string map = "";

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col <columns; col++)
                {
                    if (col == columns - 1)
                    {
                        map += mapMatrix[row][col];
                    }
                    else
                    {
                        map += mapMatrix[row][col] + Symbols.COMMA;
                    }
                }

                if (row != rows - 1)
                {
                    map += Symbols.SEMI_COLON;
                }
            }

            return map;
        }

        public static List<List<string>> CovertToMapMatrix(string map, int nRow, int nCol)
        {
            if (nRow < 0 || nCol < 0)
            {
                return null;
            }

            List<List<string>> mapMatrix = new List<List<string>>();
            string[] rows = map.Split(Symbols.SEMI_COLON);

            for (int rowNumber = 0; rowNumber < rows.Length; rowNumber++)
            {
                string row = rows[rowNumber];
                string[] cols = row.Split(Symbols.COMMA);

                mapMatrix.Add(new List<string>(new string[cols.Length]));

                for (int colNumber = 0; colNumber < cols.Length; colNumber++)
                {
                    mapMatrix[rowNumber][colNumber] = cols[colNumber];
                }
            }

            return mapMatrix;
        }

        public override string ToString()
        {
            StringBuilder text = new StringBuilder();

            text.AppendLine($"ID={ID}");
            text.AppendLine($"Runtime={Runtime}");
            text.AppendLine($"Energy={Energy}");

            foreach (AllocationProcessor processorAllocation in ProcessorAllocations)
            {
                text.AppendLine($"{processorAllocation.Allocation} | RAM={processorAllocation.RAM} | " +
                    $"Upload={processorAllocation.Upload} | Donwload={processorAllocation.Download}");
            }

            return text.ToString();
        }
    }
}

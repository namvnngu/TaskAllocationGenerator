using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskAllocationGenerator.Utils.Constants;

namespace TaskAllocationGenerator.Utils.Classes
{
    public class Map
    {
        public string Data { get; set; }

        public Map(string data)
        {
            Data = data;
        }

        /// <summary>
        /// The method converts a map string into a two-dimensional
        /// matrix of n rows and m columns, and also throws an error 
        /// when the number of rows or columns is different from
        /// the predefined values.
        /// </summary>
        /// <param name="nRow"></param>
        /// <param name="nCol"></param>
        /// <param name="validations"></param>
        /// <returns></returns>
        public string[,] ConvertToMatrix(int nRow, int nCol)
        {
            if (nRow < 0 || nCol < 0)
            {
                return null;
            }

            string[,] matrixData = new string[nRow, nCol];
            string[] rows = Data.Split(Symbols.SEMI_COLON);

            for (int rowNumber = 0; rowNumber < rows.Length; rowNumber++)
            {
                string row = rows[rowNumber];
                string[] cols = row.Split(Symbols.COMMA);

                for (int colNumber = 0; colNumber < cols.Length; colNumber++)
                {
                    matrixData[rowNumber, colNumber] = cols[colNumber];
                }
            }

            return matrixData;
        }
    }
}

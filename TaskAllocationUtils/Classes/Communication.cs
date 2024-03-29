﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace TaskAllocationUtils.Classes
{
    /// <summary>
    /// The superclass Commnucation allows subclasses
    /// to inherit and define their own behaviours.
    /// </summary>
    [DataContract]
    public class Communication
    {
        [DataMember]
        public Map MapData { get; set; }
        [DataMember]
        public List<List<string>> MapMatrix { get; set; }

        public Communication()
        {
            MapData = null;
            MapMatrix = null;
        }

        public Communication(Map mapData)
        {
            MapData = mapData;
            MapMatrix = null;
        }

        public override string ToString()
        {
            if (MapMatrix == null)
            {
                return null;
            }

            StringBuilder displayedMap = new StringBuilder();
            int nRow = MapMatrix.Count;
            int nCol = MapMatrix[0].Count;

            for (int row = 0; row < nRow; row++)
            {
                for (int col = 0; col < nCol; col++)
                {
                    displayedMap.Append(MapMatrix[row][col] + " | ");
                }
                displayedMap.Append('\n');
            }

            return displayedMap.ToString();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskAllocationGenerator.Utils.Classes
{
    public class PairSection
    {
        public bool[] ValidSectionPair { get; set; }
        public string OpeningSection { get; set; }
        public string ClosingSection { get; set; }

        public PairSection(string openingSection, string closingSection)
        {
            ValidSectionPair = new bool[2];
            ValidSectionPair[0] = false;
            ValidSectionPair[1] = false;
            OpeningSection = openingSection;
            ClosingSection = closingSection;
        }

        public bool SetValidOpeningSection(bool condition)
        {
            if (condition)
            {
                ValidSectionPair[0] = true;
                return true;
            }

            return false;
        }

        public bool SetValidClosingSection(bool condition)
        {
            if (condition)
            {
                ValidSectionPair[1] = true;
                return true;
            }

            return false;
        }

        public bool StartWithOpeningSection(string line)
        {
            if (line == OpeningSection)
            {
                ValidSectionPair[0] = true;
                return true;
            }

            return false;
        }

        public bool StartWithClosingSection(string line)
        {
            if (line == ClosingSection)
            {
                ValidSectionPair[1] = true;
                return true;
            }

            return false;
        }

        public void MarkSection(string line)
        {
            StartWithOpeningSection(line);

            if (!ValidSectionPair[0] || !ValidSectionPair[1])
            {
                if (ValidSectionPair[0])
                {
                    StartWithClosingSection(line);
                }
            }
        }
    }
}

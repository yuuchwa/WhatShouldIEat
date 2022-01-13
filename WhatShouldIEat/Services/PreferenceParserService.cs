using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatShouldIEat.Model;
using CsvHelper;
using System.Globalization;

namespace WhatShouldIEat.Services
{
    class PreferenceParserService : IPreferenceParserService
    {
        //private string PATH_TO_PREFERENCE_CSV = @"E:\Studium\ITS6_Kuenstliche_Intelligenz_und_Software_Agents\MyPDT\WhatShouldIEat\WhatShouldIEat\Resources\preferences.csv";
        private string PATH_TO_PREFERENCE_CSV = @"E:\Studium\ITS6_Kuenstliche_Intelligenz_und_Software_Agents\MyPDT\WhatShouldIEat\WhatShouldIEat\Resources\ingredientsgrid.csv";

        public Dictionary<string, string> GetUserPreference()
        {
            Dictionary<string, string> preferences = File
                .ReadLines(PATH_TO_PREFERENCE_CSV)
                .Select(line => line.Split(',')
                .ToDictionary(line => line[0], line => line[1]);

            return preferences;
        }

        public void safeScores(List<string> preference)
        {
            if(File.Exists(PATH_TO_PREFERENCE_CSV))
            {
                using (StreamReader reader = new StreamReader(PATH_TO_PREFERENCE_CSV))
                {
                    
                }
            }
        }
    }
}

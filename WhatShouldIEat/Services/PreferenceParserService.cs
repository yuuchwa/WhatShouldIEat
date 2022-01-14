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
        private string PATH_TO_PREFERENCE_CSV = @"E:\Studium\ITS6_Kuenstliche_Intelligenz_und_Software_Agents\MyPDT\WhatShouldIEat\WhatShouldIEat\Resources\preferences.csv";

        public Dictionary<string, int> GetUserPreference()
        {
            Dictionary<string, int> preferences = File
                .ReadLines(PATH_TO_PREFERENCE_CSV)
                .Select(line => line.Split(','))
                .Skip(1)
                .ToDictionary(line => line[0], line => Int32.Parse(line[1]));

            return preferences;
        }

        public void SetUserPreferences(Dictionary<string, int> preferences)
        {
            if(File.Exists(PATH_TO_PREFERENCE_CSV))
            {
                string csv = String.Join(
                    Environment.NewLine,
                    preferences.Select(x => $"{x.Key},{x.Value},")
                    );
                File.WriteAllText(PATH_TO_PREFERENCE_CSV, csv);
            }
        }
    }
}

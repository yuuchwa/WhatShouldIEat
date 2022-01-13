using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatShouldIEat.Model
{
    class UserPreference
    {
        /*        public int Id { get; set; }
                public string Ingredient { get; set; }
                public int Score { get; set; }*/

        string Ingredient;
        int Score;

        public static UserPreference FromCsv(string csvLine)
        {
            string[] values = csvLine.Split(',');
            UserPreference preferences = new UserPreference();

            preferences.Ingredient = values[0];
            preferences.Score = Convert.ToInt32(values[0]);
            return preferences;
        }
    }
}

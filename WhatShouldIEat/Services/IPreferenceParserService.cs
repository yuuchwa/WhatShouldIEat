using System;
using System.Collections.Generic;
using WhatShouldIEat.Model;

namespace WhatShouldIEat.Services
{
    interface IPreferenceParserService
    {
        public Dictionary<string, int> GetUserPreference();

        public void SetUserPreferences(Dictionary<string, int> preferences);
    }
}

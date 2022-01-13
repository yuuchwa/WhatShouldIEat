using System;
using System.Collections.Generic;
using WhatShouldIEat.Model;

namespace WhatShouldIEat.Services
{
    interface IPreferenceParserService
    {
        public Dictionary<string, string> GetUserPreference();

        public void safeScores(List<string> preference);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatShouldIEat.Services
{
    interface IIngredientParserService
    {
        public string ParseIngredients(string ingredients);

        public string RemoveUnnecessarySymbols(string str);
    }
}

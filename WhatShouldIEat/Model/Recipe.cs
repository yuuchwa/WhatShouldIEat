using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Reddit;
using Reddit.Controllers;

namespace WhatShouldIEat.Model
{
    class Recipe
    {
        public string Meal { get; set; }

        public List<string> Ingredients { get; set; }

        public string Dish { get; set; }

        public string Nationality { get; set; }

        public string duration { get; set; } // in Minutes

        public Post post { get; set; }
    }
}

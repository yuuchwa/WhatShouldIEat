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
        public string Title { get; set; }

        public List<string> Ingredients { get; set; }

        public string Author { get; set; }

        public string Dish { get; set; }

        public string Duration { get; set; } // in Minutes
 
        public string Instruction { get; set; }

        public Post PostURL { get; set; }

        public string ImageURL { get; set; }

        public Recipe()
        {
            Ingredients = new List<string>();
        }
    }
}

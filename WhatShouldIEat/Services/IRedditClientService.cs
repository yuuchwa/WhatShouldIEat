using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/* Reddit Client API */
using Reddit;

namespace WhatShouldIEat.Services
{
    interface IRedditClientService
    {
        public void RequestRecepies(Recipe recipe);

        public void RequestRecepiesByCountry(List<string> ingredieces, string nationality);
    }
}

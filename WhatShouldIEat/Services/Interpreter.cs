using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatShouldIEat.Model;

namespace WhatShouldIEat.Services
{
    class Interpreter
    {
        private int MAX_ATTEMPTS = 5;

        public Interpreter()
        {

        }

        public Recipe ReceiveNewRecipeRequest()
        {
            Recipe recipe = new Recipe();

            //Console.WriteLine("Was möchtest du heute ausprobieren.");
            while (true)
            {
                int option = 0;

                Console.WriteLine("Wonach möchtest du heute suchen?");
                Console.WriteLine("1) Ein Rezept aus bestimmten Zutaten?");
                Console.WriteLine("2) Ein Rezept für ein bestimmtes Gericht?");
                if (int.TryParse(Console.ReadLine(), out option))
                {
                    string responseText = "";
                    if (option == 1)
                    {
                        Console.WriteLine("Welche Zutaten sollen im Gericht enthalten sein?");
                        responseText = Console.ReadLine();
                        recipe.Ingredients = responseText.Split(", ").ToList();
                        break;
                    }
                    else if (option == 2)
                    {
                        Console.WriteLine("Welches Gericht möchtest du kochen?");
                        responseText = Console.ReadLine();
                        recipe.Dish = responseText;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Das ist keine gültige Eingabe. Versuche es nochmal.");
                    }
                }
                else
                {
                    Console.WriteLine("Das ist keine gültige Eingabe. Versuche es nochmal.");
                }
            }
               

/*                 Console.WriteLine("Soll das Gericht in unter 15 Minuten gekocht sein? j/n");
                while(true)
                {
                    responseText = Console.ReadLine();
                    if (String.Equals(responseText, "j")) {
                        recipe.duration = "fast";
                        break;
                    }
                    else if (String.Equals(responseText, "n"))
                    {
                        recipe.duration = "slow";
                        break;
                    }
                    Console.WriteLine("Das ist keine gültige Eingabe. Versuche es nochmal.");
                } */

            return recipe; 
        }

        /*
        private List<string> filterIngredientsFromResponse(string response)
        {
            // TODO: Check if evey string is actually an ingredient.
            List<string> ingredients = response.Split(',').ToList(); ;
            response = cleanString(response);
            return ingredients;
        }

        private string cleanString(string str)
        {
            string cleanedString = str.Replace(", ", "").Replace("; ", "");

            return cleanedString;
        }
        */
    }
}

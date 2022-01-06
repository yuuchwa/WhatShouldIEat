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
                        if (String.IsNullOrEmpty(responseText))
                        {
                            continue;
                        }
                        recipe.Ingredients = responseText.ToLower().Split(", ").ToList();
                        break;
                    }
                    else if (option == 2)
                    {
                        Console.WriteLine("Welches Gericht möchtest du kochen?");
                        responseText = Console.ReadLine();

                        if (String.IsNullOrEmpty(responseText))
                        {
                            continue;
                        }
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
            return recipe; 
        }

        public void PrintRecipe(Recipe recipe)
        {
            Console.WriteLine("\nTitle: " + recipe.Title);
            Console.WriteLine("Author: " + recipe.Author);
            Console.WriteLine("Ingredients: \n");
            foreach(string ingredient in recipe.Ingredients)
            {
                Console.WriteLine("- " + ingredient);
            }
            Console.WriteLine("\nInstruction\n");
            Console.WriteLine(recipe.Instruction);
        }
    }
}

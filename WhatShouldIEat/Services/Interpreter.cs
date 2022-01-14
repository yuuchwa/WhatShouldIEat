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
            while (true)
            {
                int option = 0;

                Console.WriteLine("Wonach möchtest du heute suchen?");
                Console.WriteLine("1 - Ein Rezept aus bestimmten Zutaten?");
                Console.WriteLine("2 - Ein Rezept für ein bestimmtes Gericht?");
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

        public bool CheckIfUserLikesTheRecipe(Recipe recipe)
        {
            int option = 0;
            int attempts = 5;
            int counter = 0;
            PrintRecipe(recipe);

            Console.WriteLine("Findest du das Rezept interessant? Möchtest du es nachkochen?");
            Console.WriteLine("1 - Ja");
            Console.WriteLine("2 - Nein");

            while(counter < attempts)
            {
                if (int.TryParse(Console.ReadLine(), out option))
                {
                    if (option == 1)
                    {
                        Console.WriteLine("Gute Wahl! Viel Spaß beim Kochen und guten Hunger!");
                        return true;
                    }
                    else if(option == 2)
                    {
                        Console.WriteLine("Schade, dann suchen wir nach einem neuen Rezept...");
                        return false;
                    }
                    else
                    {
                        Console.WriteLine("Leider ist das keine gültige Eingabe, Versuche es erneut.");
                    }
                }
            }
            return false;
        }

        public int WaitForFeedback()
        {
            Console.WriteLine("\nHat dir das Rezept geschmeckt?");
            Console.WriteLine("1) ja, das Rezept war super lecker!");
            Console.WriteLine("2) Es hat geschmeckt!");
            Console.WriteLine("3) Es reichte, um mich zu sättigen.");
            Console.WriteLine("4) ich hatte schonmal besseres gegessen.");
            Console.WriteLine("5) Es schmeckte mir ganz und gar nicht.");

            int option = 0;
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out option))
                {
                    if (1 <= option && option <= 5)
                    {
                        return option;
                    }
                    else
                    {
                        option = 0;
                        Console.WriteLine("Das ist keine gültige Eingabe. Versuche es nochmal.");
                    }
                }
                else
                {
                    Console.WriteLine("Das ist keine gültige Eingabe. Versuche es nochmal.");
                }
            }
        }

        public void PrintRecipe(Recipe recipe)
        {
            Console.WriteLine("\nTitel: " + recipe.Title);
            Console.WriteLine("Autor: " + recipe.Author);
            Console.WriteLine("Zutaten: \n");
            foreach(string ingredient in recipe.Ingredients)
            {
                Console.WriteLine("- " + ingredient);
            }
            Console.WriteLine("\nBeschreibung");
            Console.WriteLine(recipe.Instruction);
        }
    }
}

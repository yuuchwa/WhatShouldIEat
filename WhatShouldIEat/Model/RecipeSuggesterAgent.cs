using System;
using System.Collections.Generic;
using System.Linq;
using Mars.Interfaces.Agents;
using Reddit.Controllers;
using WhatShouldIEat.Services;

namespace WhatShouldIEat.Model
{
    /// <summary>
    ///  A simple agent stub that has an Init() method for initialization and a
    ///  Tick() method for acting in every tick of the simulation.
    /// </summary>
    public class RecipeSuggesterAgent : IAgent<RecipeGridLayer>
    {
        IRedditClientService redditClient;
        IIngredientParserService IngredientParserService;
        Interpreter Interpreter;

        public void Init(RecipeGridLayer layer)
        {
            Layer = layer; // store layer for access within agent class
            redditClient = new RedditClientService();
            Interpreter = new Interpreter();
            IngredientParserService = new IngredientParserService();
        }

        public void Tick()
        {
            //do something useful in every tick of the simulation
            Recipe recipe = Interpreter.ReceiveNewRecipeRequest();
            List<Post> posts = redditClient.RequestRecipePosts(recipe);

            foreach (Post post in posts)
            {
                recipe.Title = post.Title;
                recipe.Author = post.Author;

                List<Comment> postComments = post.Comments.GetComments();

                // Find den Posts mit den Zutaten und der Instruction
                Comment commentWithInstruction = redditClient.FindInstructionInPost(recipe.Author, postComments);

                if(commentWithInstruction != null)
                {
                    // Find alle Zutaten
                    // Überprüfen, ob die Zutaten passen
                    List<string> ingredientsStrList = redditClient
                        .GetTextBetween(commentWithInstruction.Body.ToLower(), "ingredients", "instructions")
                        .Split("\n\n")
                        .ToList();

                    string ingredient = "";
                    foreach (string ingredientStr in ingredientsStrList)
                    {
                        string cleanedStr = IngredientParserService.RemoveUnnecessarySymbols(ingredientStr);
                        if (!String.IsNullOrEmpty(cleanedStr))
                        {
                            if (!String.IsNullOrEmpty(ingredient = IngredientParserService.ParseIngredients(cleanedStr)))
                            {
                                recipe.Ingredients.Add(ingredient);
                            }
                        }
                    }
                    string instruction = redditClient.GetInstructionFromComment(commentWithInstruction);
                    recipe.Instruction = IngredientParserService.RemoveUnnecessarySymbols(instruction);
                    Interpreter.PrintRecipe(recipe);
                    break;
                }
            }
        }


        private RecipeGridLayer Layer { get; set; } // provides access to the main layer of this agent
        
        public Guid ID { get; set; } // identifies the agent
    }
}
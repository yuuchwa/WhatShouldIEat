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
    public class RecipeSuggesterAgent : IAgent<PreferenceGridLayer>
    {
        IRedditClientService redditClient;
        IIngredientParserService IngredientParserService;
        Interpreter Interpreter;
        IPreferenceParserService preferenceService;
        Dictionary<string, string> preferences;

        public void Init(PreferenceGridLayer layer)
        {
            Layer = layer; // store layer for access within agent class
            redditClient = new RedditClientService();
            Interpreter = new Interpreter();
            IngredientParserService = new IngredientParserService();
            preferenceService = new PreferenceParserService();
            preferences = preferenceService.GetUserPreference();
        }

        public void Tick()
        {
            /* Get Recipes from Reddit */
            Recipe recipe = Interpreter.ReceiveNewRecipeRequest();
            List<Post> posts = redditClient.RequestRecipePosts(recipe);
            bool recipeNotFound = true;

            foreach (Post post in posts)
            {
                recipe.Title = post.Title;
                recipe.Author = post.Author;

                List<Comment> postComments = post.Comments.GetComments();

                // Find den Posts mit den Zutaten und der Instruction
                Comment commentWithInstruction = redditClient.FindInstructionInPost(recipe.Author, postComments);

                if(commentWithInstruction != null)
                {
                    /* Find Substring where the ingrediets are located*/
                    List<string> ingredientsStrList = redditClient
                        .GetIngredientsInComment(commentWithInstruction.Body.ToLower())
                        .Split("\n\n")
                        .ToList();

                    /* Identify ingredients in string */
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

                    /* Find instruction */
                    string instruction = redditClient.GetInstructionFromComment(commentWithInstruction);
                    recipe.Instruction = IngredientParserService.RemoveUnnecessarySymbols(instruction);

                    /* Recommend recipe to user */
                    if (CheckIfUserMightLikeRecipe(recipe))
                    {
                        if (Interpreter.CheckIfUserLikesTheRecipe(recipe))
                        {
                            Interpreter.WaitForFeedback();
                            recipeNotFound = false;
                            break;
                        }
                        else
                        {

                            recipeNotFound = false;
                            break;
                        }
                    }
                }
            }

            if(recipeNotFound)
            {
                Console.WriteLine("Leider konnte passendes Rezept gefunden werden.");
                Console.WriteLine();
            }

        }
        public Guid ID { get; set; } // identifies the agent

        private PreferenceGridLayer Layer { get; set; } // provides access to the main layer of this agent

        private bool CheckIfUserMightLikeRecipe(Recipe recipe)
        {
            bool suitsUsersPreference = true;
            List<int> scores = new List<int>();

            foreach(string ingredient in recipe.Ingredients)
            {
                if (preferences.ContainsKey(ingredient))
                {
                    string score;
                    preferences.TryGetValue(ingredient, out score);

                    if(Convert.ToInt32(score) < 20)
                    {
                        suitsUsersPreference = false;
                        break;
                    }

                    scores.Add(Convert.ToInt32(score));
                }
            }
            if(scores.Count > 0)
            {
                if (meanScoreIsBelowTolerance(scores))
                {
                    suitsUsersPreference = false;
                }
            }

            return suitsUsersPreference;
        }
        

        private bool meanScoreIsBelowTolerance(List<int> scores)
        {
            int meanScore = 0;
            foreach(int score in scores)
            {
                meanScore += score;
            }
            meanScore /= scores.Count();

            return meanScore > 30 ? true : false;
        }
    }
}
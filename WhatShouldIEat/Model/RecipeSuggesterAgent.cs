using System;
using System.Collections.Generic;
using System.Linq;
using Mars.Interfaces.Agents;
using Reddit.Controllers;
using WhatShouldIEat.Services;
using WhatShouldIEat.Model;

namespace WhatShouldIEat.Model
{
    /// <summary>
    ///  A simple agent stub that has an Init() method for initialization and a
    ///  Tick() method for acting in every tick of the simulation.
    /// </summary>
    public class RecipeSuggesterAgent : IAgent<PreferenceGridLayer>
    {
        public int DEFAULT_SCORE = 50;

        IRedditClientService redditClient;
        IIngredientParserService IngredientParserService;
        Interpreter Interpreter;
        IPreferenceParserService PreferenceService;
        Dictionary<string, int> preferences;

        public void Init(PreferenceGridLayer layer)
        {
            Layer = layer; // store layer for access within agent class
            redditClient = new RedditClientService();
            Interpreter = new Interpreter();
            IngredientParserService = new IngredientParserService();
            PreferenceService = new PreferenceParserService();
            preferences = PreferenceService.GetUserPreference();
        }

        public void Tick()
        {
            /* Get Recipes from Reddit */
            Recipe recipeRequest = Interpreter.ReceiveNewRecipeRequest();
            List<Post> posts = redditClient.RequestRecipePosts(recipeRequest);
            bool recipeNotFound = true;

            foreach (Post post in posts)
            {
                Recipe recipe = new Recipe();
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
                        .Split("\n")
                        .ToList();

                    if (ingredientsStrList == null)
                    {
                        continue;
                    }

                    ingredientsStrList = removeExcessiveListElement(ingredientsStrList);

                    /* Identify ingredients in string */
                    string ingredient = "";
                    foreach (string ingredientStr in ingredientsStrList)
                    {
                        if (!String.IsNullOrEmpty(ingredientStr))
                        {
                            string cleanedStr = IngredientParserService.RemoveUnnecessarySymbols(ingredientStr);
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
                            int feedback = Interpreter.WaitForFeedback();
                            updateIngredientPreferencesList(recipe, feedback);
                            recipeNotFound = false;
                            break;
                        }
                        else
                        {
                            // Reduce ingredient score by 2
                            updateIngredientPreferencesList(recipe, 6);
                            Console.Clear();
                        }
                    }
                }
                else
                {
                    Console.Clear();
                }
            }

            if(recipeNotFound)
            {
                Console.WriteLine("Leider konnte passendes Rezept gefunden werden.");
                Console.WriteLine("\n");
            }

        }
        public Guid ID { get; set; } // identifies the agent

        private PreferenceGridLayer Layer { get; set; }

        private bool CheckIfUserMightLikeRecipe(Recipe recipe)
        {
            bool suitsUsersPreference = true;
            List<int> scores = new List<int>();
            List<string> ingredients = recipe.Ingredients;

            int score = 0;
            foreach (string ingredient in recipe.Ingredients)
            {
                if(!preferences.ContainsKey(ingredient) || !preferences.TryGetValue(ingredient, out score))
                {
                    preferences.Add(ingredient, DEFAULT_SCORE);
                    score = DEFAULT_SCORE;
                }

                if(score < 20)
                {
                    suitsUsersPreference = false;
                    break;
                }

                scores.Add(score);
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

            return meanScore < 30 ? true : false;
        }

        private List<string> removeExcessiveListElement(List<string> list)
        {
            list.RemoveAll(x => x.Length < 2);
            return list;
        }

        private void updateIngredientPreferencesList(Recipe recipe, int rating) // Enum rein
        {
            int bonus = 0;
            switch (rating)
            {
                case 1: bonus = 10; break;
                case 2: bonus = 5; break;
                case 3: bonus = 1; break;
                case 4: bonus = -5; break;
                case 5: bonus = -10; break;
                case 6: bonus = -2; break;
            }
            foreach (string ingredient in recipe.Ingredients)
            {
                preferences[ingredient] += bonus;
                PreferenceService.SetUserPreferences(preferences);
            }
        }
    }
}
using System;
using Mars.Interfaces.Agents;
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
        Interpreter interpreter;

        public void Init(RecipeGridLayer layer)
        {
            Layer = layer; // store layer for access within agent class
            redditClient = new RedditClientService();
            interpreter = new Interpreter();
        }
        
        public void Tick()
        {
            //do something useful in every tick of the simulation
            Recipe recipe = interpreter.ReceiveNewRecipeRequest();
            redditClient.RequestRecepies(recipe);
        }

        private RecipeGridLayer Layer { get; set; } // provides access to the main layer of this agent
        
        public Guid ID { get; set; } // identifies the agent
        
        
    }
}
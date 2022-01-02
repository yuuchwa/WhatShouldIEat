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

        public void Init(RecipeGridLayer layer)
        {
            Layer = layer; // store layer for access within agent class
            redditClient = new RedditClientService();
        }
        
        public void Tick()
        {
            //do something useful in every tick of the simulation
            fetchRedditPost();
        }

        private RecipeGridLayer Layer { get; set; } // provides access to the main layer of this agent

        private void fetchRedditPost()
        {

        }
        
        public Guid ID { get; set; } // identifies the agent
        
        
    }
}
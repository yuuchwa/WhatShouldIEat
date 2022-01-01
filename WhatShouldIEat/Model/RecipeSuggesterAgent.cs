using System;
using Mars.Interfaces.Agents;

namespace WhatShouldIEat.Model
{
    /// <summary>
    ///  A simple agent stub that has an Init() method for initialization and a
    ///  Tick() method for acting in every tick of the simulation.
    /// </summary>
    public class RecipeSuggesterAgent : IAgent<RecipeGridLayer>
    {
        public void Init(RecipeGridLayer layer)
        {
            Layer = layer; // store layer for access within agent class
        }
        
        public void Tick()
        {
            //do something useful in every tick of the simulation
            see();
            hear();
            touch();
            taste();
            smell();
        }

        private RecipeGridLayer Layer { get; set; } // provides access to the main layer of this agent
        
        private void see(){}
        private void hear(){}
        private void touch(){}
        private void taste(){}
        private void smell(){}
        
        public Guid ID { get; set; } // identifies the agent
        
        
    }
}
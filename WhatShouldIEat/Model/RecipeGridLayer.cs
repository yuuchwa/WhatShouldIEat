using System;
using System.Linq;
using Mars.Components.Layers;
using Mars.Core.Data;
using Mars.Interfaces.Layers;

namespace WhatShouldIEat.Model
{
    /// <summary>
    ///     A simple raster layer that provides access to the values of a raster input.
    /// </summary>
    public class RecipeGridLayer : RasterLayer
    {
        public override bool InitLayer(LayerInitData layerInitData, RegisterAgent registerAgentHandle,
            UnregisterAgent unregisterAgentHandle)
        {
            base.InitLayer(layerInitData, registerAgentHandle, unregisterAgentHandle); // base class requires init, too
            
            // the agent manager can create agents and initializes them as defined in the sim config
            var agentManager = layerInitData.Container.Resolve<IAgentManager>();
            var agents = agentManager.Spawn<RecipeSuggesterAgent, RecipeGridLayer>().ToList();
            //Console.WriteLine($"We created {agents.Count} agents.");
            
            return true;
        }
    }
}
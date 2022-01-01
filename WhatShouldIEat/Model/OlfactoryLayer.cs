using System;
using System.Linq;
using Mars.Components.Layers;
using Mars.Interfaces.Layers;
using Mars.Core.Data;

namespace WhatShouldIEat.Model
{
    public class OlfactoryLayer : RasterLayer
    {
        public override bool InitLayer(LayerInitData layerInitData, RegisterAgent registerAgentHandle,
            UnregisterAgent unregisterAgentHandle)
        {
            // base.InitLayer(layerInitData, registerAgentHandle, unregisterAgentHandle); // base class requires init, too
            //
            // // the agent manager can create agents and initializes them as defined in the sim config
            // var agentManager = layerInitData.Container.Resolve<IAgentManager>();
            // var agents = agentManager.Spawn<WhatShouldIEat, HumanGridLayer>().ToList();
            // Console.WriteLine($"We created {agents.Count} agents.");

            return true;
        }
    }
}

using System;
using System.IO;
using WhatShouldIEat.Model;
using Mars.Components.Starter;
using Mars.Interfaces.Model;

namespace WhatShouldIEat
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            // the scenario consist of the model (represented by the model description)
            // an the simulation configuration (see config.json)
            
            // Create a new model description that holds all parts of the model, here: one agent type and one layer
            var description = new ModelDescription();
            description.AddLayer<RecipeGridLayer>(); // we'll use a simple grid layer here
            description.AddAgent<Model.RecipeSuggesterAgent, RecipeGridLayer>(); // the agent type will be located at the grid layer

            // scenario definition
            // use config.json that provides the specification of the scenario
            var file = File.ReadAllText("config.json");
            var config = SimulationConfig.Deserialize(file);
            
            // Create simulation task accordingly
            var task = SimulationStarter.Start(description, config);
            
            // Run simulation
            var loopResults = task.Run();
            
            // Feedback to user that simulation run was successful
            Console.WriteLine($"Simulation execution finished after {loopResults.Iterations} steps");
        }
    }
}
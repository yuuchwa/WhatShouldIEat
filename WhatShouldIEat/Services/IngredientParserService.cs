using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RecipeIngredientParser.Core.Parser;
using RecipeIngredientParser.Core.Parser.Extensions;
using RecipeIngredientParser.Core.Parser.Sanitization;
using RecipeIngredientParser.Core.Parser.Strategy;
using RecipeIngredientParser.Core.Tokens;
using RecipeIngredientParser.Core.Tokens.Abstract;

namespace WhatShouldIEat.Services
{

    class IngredientParserService : IIngredientParserService
    {
        IngredientParser ingredientParser;

        public IngredientParserService()
        {
            CreateParser();
        }

        public string ParseIngredients(string ingredient)
        {
            ingredient = SanitizeInput(ingredient);
            if(this.ingredientParser.TryParseIngredient(ingredient, out var parseResult))
            {
                //Console.WriteLine($"\tIngredient: {parseResult.Details.Ingredient}");
                return parseResult.Details.Ingredient;
            }
            return null;
        }

        private string SanitizeInput(string str)
        {
            var rule1 = new ReplaceUnicodeFractionsRule();
            str = rule1.Apply(str);
            var rule2 = new ReplaceUnicodeFractionsRule();
            str = rule2.Apply(str);
            var rule3 = new RemoveAlternateIngredientsRule();
            str = rule3.Apply(str);
            var  rule4 = new RemoveExtraneousSpacesRule();
            return rule4.Apply(str);
        }

        public string RemoveUnnecessarySymbols(string str)
        {
            return str
                .Replace("\\", "")
                .Replace("*", "")
                .Replace(":", "")
                .Replace("[", "")
                .Replace("]", "")
                .Replace("-", "");
        }

        private void CreateParser()
        {
            ingredientParser = IngredientParser
                .Builder
                .New
                .WithDefaultConfiguration()
                .Build();
        }
    }
}

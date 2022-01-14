using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// https://github.com/JedS6391/RecipeIngredientParser
using RecipeIngredientParser.Core.Parser;
using RecipeIngredientParser.Core.Parser.Context;
using RecipeIngredientParser.Core.Parser.Extensions;
using RecipeIngredientParser.Core.Parser.Sanitization;
using RecipeIngredientParser.Core.Parser.Sanitization.Abstract;
using RecipeIngredientParser.Core.Parser.Strategy;
using RecipeIngredientParser.Core.Templates;
using RecipeIngredientParser.Core.Tokens;
using RecipeIngredientParser.Core.Tokens.Abstract;
using RecipeIngredientParser.Core.Tokens.Readers;

namespace WhatShouldIEat.Services
{
    class IngredientParserService : IIngredientParserService
    {
        IngredientParser IngredientParser;

        public IngredientParserService()
        {
            CreateParser();
        }

        public string ParseIngredients(string ingredients)
        {
            ingredients = SanitizeInput(ingredients);

            CreateParser();
            if (this.IngredientParser.TryParseIngredient(ingredients, out var parseResult))
            {
                //Console.WriteLine($"\tIngredient: {parseResult.Details.Ingredient}");
                return parseResult.Details.Ingredient;
            }
            return null;
        }

        private string SanitizeInput(string description)
        {
            var rules = new IInputSanitizationRule[]
            {
                new RemoveExtraneousSpacesRule(),
                new RangeSubstitutionRule(),
                new RemoveBracketedTextRule(),
                new RemoveAlternateIngredientsRule(),
                new ReplaceUnicodeFractionsRule(),
                new ConvertToLowerCaseRule()
            };

            string sanitizedInput = description;
            foreach (var rule in rules)
            {
                sanitizedInput = rule.Apply(sanitizedInput);
            }
            return sanitizedInput;
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
            var templateDefinitions = new String[] {
                "{amount} {unit} {form} {ingredient}",
                "{amount} {unit} {ingredient}",
                "{amount} {unit} {ingredient} {form}",
                "{amount} {unit} {ingredient} ({form})",
                "{amount} {ingredient} {and} {ingredient} ({form})",
                "{amount} {ingredient}",
                "{amount} {ingredient} {unit}",
            };

            IngredientParser = IngredientParser
                .Builder
                .New
                .WithTemplateDefinitions(templateDefinitions)
                .WithDefaultConfiguration()
                .Build();
        }





















        private string createTemplate()
        {
            var templateDefinition = "{amount} {unit} {ingredient}";
            var tokenReaderFactory = new TokenReaderFactory(new ITokenReader[]
            {
                new AmountTokenReader(),
                new UnitTokenReader(),
                new IngredientTokenReader()
            });

            var builder = Template
                .Builder
                .New
                .WithTemplateDefinition(templateDefinition)
                .WithTokenReaderFactory(tokenReaderFactory);

            var template = builder.Build();
            var context = new ParserContext("1 cup vegetable stock");
            var result = template.TryReadTokens(context, out var tokens);

            foreach (var token in tokens)
            {
                Console.WriteLine($"\t- {token.ToString()}");
            }
            return null;
        }

        private void testBestMatch()
        {
            string ingredients = "";

            static decimal ResolveTokenWeight(IToken token)
            {
                switch (token)
                {
                    case LiteralToken literalToken:
                        // Longer literals score more - the assumption being that
                        // a longer literal means a more specific value.
                        return 0.1m * literalToken.Value.Length;

                    case LiteralAmountToken _:
                    case FractionalAmountToken _:
                    case RangeAmountToken _:
                        return 1.0m;

                    case UnitToken unitToken:
                        return unitToken.Type == UnitType.Unknown ?
                            // Punish unknown unit types
                            -1.0m :
                            1.0m;

                    case FormToken _:
                        return 1.0m;

                    case IngredientToken _:
                        return 2.0m;
                }

                return 0.0m;
            }

            var tokenReaderFactory = new TokenReaderFactory(new ITokenReader[]
            {
                new AmountTokenReader(),
                new UnitTokenReader(),
                new FormTokenReader(),
                new IngredientTokenReader()
            });
            var context = new ParserContext(ingredients);

            var templateDefinitions = new String[]
            {
                "{amount} {unit} {form} {ingredient}",
                "{amount} {unit} {ingredient}",
                "{amount} {unit} {ingredient} {form}",
                "{amount} {unit} {ingredient} ({form})",
                "{amount} {unit} {ingredient} {form}",

            };

            var templates = new Template[]
            {


                Template
                    .Builder
                    .New
                    .WithTemplateDefinition("{amount} {unit} {form} {ingredient}")
                    .WithTokenReaderFactory(tokenReaderFactory)
                    .Build()
            };

            var strategy = new BestFullMatchParserStrategy(
                BestMatchHeuristics.WeightedTokenHeuristic(ResolveTokenWeight)); // ResolveTokenWeight - Wahrscheinlichkeit der Verteilung

            if (strategy.TryParseIngredient(context, templates, out var parseResult))
            {
                Console.WriteLine("Parse successful.");
                Console.WriteLine();
                Console.WriteLine($"\tAmount: {parseResult.Details.Amount}");
                Console.WriteLine($"\tUnit: {parseResult.Details.Unit}");
                Console.WriteLine($"\tForm: {parseResult.Details.Form}");
                Console.WriteLine($"\tIngredient: {parseResult.Details.Ingredient}");
            }
            else
            {
                Console.WriteLine("Parse not successful.");
            }
        }
    }
}

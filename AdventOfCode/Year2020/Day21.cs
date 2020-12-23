using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Year2020
{
    public class RecipeBook
    {
        private Dictionary<string, HashSet<string>> ingredientLookup = new Dictionary<string, HashSet<string>>();
        private Dictionary<string, HashSet<string>> alergyLookup = new Dictionary<string, HashSet<string>>();
        private List<HashSet<string>> recipes = new List<HashSet<string>>();

        // http://regexstorm.net/tester
        private static readonly Regex recipeReg = new Regex(@"((?<ing>\w+) )+\(contains ((?<aler>\w+)(, )?)+\)");

        public bool AddRecipe(string input)
        {
            var match = recipeReg.Match(input);
            if (match.Success)
            {
                var ingredients = match.Groups["ing"].Captures.Select(c => c.Value);
                var alergens = match.Groups["aler"].Captures.Select(a => a.Value);
                foreach (string alergen in alergens)
                {
                    var possibleIngredients = alergyLookup.GetValueOrDefault(alergen);
                    if (possibleIngredients == null)
                    {
                        possibleIngredients = ingredients.ToHashSet();
                        alergyLookup.Add(alergen, possibleIngredients);
                    }
                    else
                    {
                        possibleIngredients.IntersectWith(ingredients);
                    }

                    foreach (var ingredient in ingredients)
                    {
                        var possibleAlergens = ingredientLookup.GetValueOrDefault(ingredient);
                        if (possibleAlergens == null)
                        {
                            possibleAlergens = alergens.ToHashSet();
                            ingredientLookup.Add(ingredient, possibleAlergens);
                        }
                        else
                        {
                            possibleAlergens.UnionWith(alergens);
                        }
                    }
                }
                recipes.Add(ingredients.ToHashSet());
            }

            return match.Success;
        }

        public HashSet<string> InertIngredients()
        {
            var nonAlergicIngredients = new HashSet<string>(ingredientLookup.Keys);
            foreach(var allergicRecipeIngredients in alergyLookup.Values)
            {
                nonAlergicIngredients.ExceptWith(allergicRecipeIngredients);
            }
            return nonAlergicIngredients;
        }

        public IEnumerable<string> CanonicalDangerousIngredients()
        {
            var knownAlergens = new HashSet<string>();
            var ingredientsWithUnknownAlergens = ingredientLookup.Keys.Except(InertIngredients()).Select(ingredient => KeyValuePair.Create(ingredient, ingredientLookup[ingredient])).ToList();
            bool hasReduced = true;
            while (hasReduced)
            {
                hasReduced = false;
                int count = ingredientsWithUnknownAlergens.Count;
                for (int i = 0; i < count; i++)
                {
                    var ingredient = ingredientsWithUnknownAlergens[i];

                    var possibleAlergens = ingredientLookup[ingredient.Key];
                    var currentCount = possibleAlergens.Count;
                    //if (possibleAlergens.Count == 1)
                    //{
                    //    hasReduced = true;
                    //    knownAlergens.Add(possibleAlergens.First());
                    //    continue;
                    //}

                    possibleAlergens.ExceptWith(knownAlergens);
                    if (possibleAlergens.Count != currentCount)
                    {
                        hasReduced = true;
                        if (possibleAlergens.Count == 1)
                        {
                            knownAlergens.Add(possibleAlergens.First());
                        }
                        else if (possibleAlergens.Count == 0)
                        {
                            ingredientsWithUnknownAlergens.Remove(ingredient);
                            count--;
                        }
                    }
                }
            }

            return ingredientsWithUnknownAlergens.OrderBy(i => i.Value.First()).Select(i => i.Value.First());
        }

        public int NumberOfUsages(HashSet<string> ingredients)
        {
            int count = 0;

            foreach(var recipe in recipes)
            {
                count += recipe.Where(r => ingredients.Contains(r)).Count();
            }
            return count;
        }
    }

    // https://adventofcode.com/2020/day/21
    public class Day21
    {
        private ITestOutputHelper output;

        public Day21(ITestOutputHelper output)
        {
            this.output = output;
        }

        private RecipeBook ReadRecipeBook()
        {
            using var reader = new StreamReader(InputClient.GetFileStream(2020, 21, ""));
            var recipeBook = new RecipeBook();
            string input;

            while ((input = reader.ReadLine()) != null)
            {
                recipeBook.AddRecipe(input);
            }
            return recipeBook;
        }

        [Fact]
        public void Part1()
        {
            var recipeBook = ReadRecipeBook();
            var nonAlergicIngredients = recipeBook.InertIngredients();
            Assert.Equal(2659, recipeBook.NumberOfUsages(nonAlergicIngredients));
        }

        [Fact]
        public void Part2()
        {
            var recipeBook = ReadRecipeBook();
            output.WriteLine(string.Join(',', recipeBook.CanonicalDangerousIngredients().ToArray()));
            // qjvvcvz,zqzmzl,tfqsb,xhnk,tsqpn,nrl,cltx,rcqb
            Assert.Equal("qjvvcvz,zqzmzl,tfqsb,xhnk,tsqpn,nrl,cltx,rcqb", string.Join(',', recipeBook.CanonicalDangerousIngredients().ToArray()));
        }
    }
}

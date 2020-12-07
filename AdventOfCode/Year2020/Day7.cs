using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Year2020
{
    public enum Day7Items
    {
        BagDefinition,
        BagContents,
        Contain,
        Terminator
    }

    public class Day7Lexer : SimpleLexer<Day7Items>
    {
        private static readonly List<Tuple<Day7Items, string>> NewList = new List<Tuple<Day7Items, string>> {
            Tuple.Create(Day7Items.BagDefinition, @"^\w+ \w+ bags"),
            Tuple.Create(Day7Items.BagContents, @"^\d+ \w+ \w+ bags?"),
            Tuple.Create(Day7Items.Contain, @"^ contain "),
            Tuple.Create(Day7Items.Terminator, @""),
        };

        public Day7Lexer() : base(NewList)
        { }
    }

    public class Bag
    {
        public string Name { get; private set; }
        public List<BagSupply> Contains { get; set; }

        public Bag(string name)
        {
            Name = name;
            Contains = new List<BagSupply>();
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public bool DoesEventuallyContain(Dictionary<string, Bag> bags, Bag eventualBag)
        {
            if (Contains.FirstOrDefault(bs => bs.Name == eventualBag.Name) != null)
                return true;

            foreach(var bag in Contains)
            {
                var realBag = bags[bag.Name];
                if (realBag.DoesEventuallyContain(bags, eventualBag)) return true;
            }

            return false;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != typeof(Bag)) return false;
            return GetHashCode() == obj.GetHashCode();
        }

        internal int OverallBagCount(Dictionary<string, Bag> bags)
        {
            int totalBags = 1;

            foreach(BagSupply b in Contains)
            {
                totalBags += b.Quantity * bags[b.Name].OverallBagCount(bags);
            }

            return totalBags;
        }
    }

    public class BagSupply
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
    }


    // https://adventofcode.com/2020/day/7
    public class Day7
    {
        private static Regex defParser = new Regex(@"(\d+) (\w+ \w+ bag)", RegexOptions.Compiled);
        private readonly ITestOutputHelper output;

        public Day7(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void Part1()
        {
            Dictionary<string, Bag> bags = ReadBags().ToDictionary(bag => bag.Name);
            var shinyGoldBag = bags["shiny gold bag"];
            var hasShinyGoldBag = 0;
            foreach (var bag in bags.Values)
            {
                if (bag.DoesEventuallyContain(bags, shinyGoldBag)) hasShinyGoldBag++;
                output.WriteLine($"{bag.Name} has {bag.Contains.Count} requirements");
            }
            Assert.Equal(254, hasShinyGoldBag);
        }

        [Fact]
        public void Part2()
        {
            Dictionary<string, Bag> bags = ReadBags().ToDictionary(bag => bag.Name);
            var shinyGoldBag = bags["shiny gold bag"];
            var bagCount = shinyGoldBag.OverallBagCount(bags) - 1;

            Assert.Equal(6006, bagCount);
        }

        IEnumerable<Bag> ReadBags()
        {
            List<Bag> bags = new List<Bag>();
            var stream = InputClient.GetFileStream(2020, 7, "");
            using var reader = new StreamReader(stream);
            string input;
            var lexer = new Day7Lexer();

            Bag bag = new Bag("unused");
            bool expectingDefintion = true;
            while ((input = reader.ReadLine()) != null)
            {
                foreach(var token in lexer.Tokenize(input))
                {
                    switch(token.TokenType)
                    {
                        case Day7Items.BagContents:
                        {
                            var match = defParser.Matches(token.Value);
                            bag.Contains.Add(new BagSupply
                            {
                                Name = match[0].Groups[2].Value,
                                Quantity = int.Parse(match[0].Groups[1].Value)
                            });
                            break;
                        }
                        case Day7Items.Contain:
                        {
                            expectingDefintion = false;
                            break;
                        }
                        case Day7Items.BagDefinition:
                            if (expectingDefintion)
                            {
                                bag = new Bag(token.Value.Substring(0, token.Value.Length - 1));
                            }
                            expectingDefintion = false;
                            break;
                        case Day7Items.Terminator:
                            bags.Add(bag);
                            expectingDefintion = true;
                            yield return bag;
                            break;
                    }
                }
            }
        }
    }
}

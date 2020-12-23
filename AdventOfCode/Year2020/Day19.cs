using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Year2020
{
    public class Rule
    {
        public IEnumerable<IEnumerable<int>> Rules { get; set; }
        public char Character { get; set; }
    }

    public class RuleSet
    {
        private Dictionary<int, Rule> ruleset = new Dictionary<int, Rule>();

        // http://regexstorm.net/tester
        private static readonly Regex ruleReg = new Regex(@"(\d+): ((?<Value>""\w"")|(((?<RuleOne>\d+)\s?)+(\| ((?<RuleTwo>\d+)\s?)+)?))");

        public bool AddRule(string input)
        {
            var match = ruleReg.Match(input);
            if (match.Success)
            {
                var newRule = new Rule();
                var ruleNum = int.Parse(match.Groups[1].Value);
                List<IEnumerable<int>> rules = null;

                if (match.Groups["RuleOne"].Success)
                {
                    rules = new List<IEnumerable<int>>();
                    newRule.Rules = rules;
                    rules.Add(
                        match.Groups["RuleOne"].Captures.Select(c => int.Parse(c.Value))
                    );
                }
                if (match.Groups["RuleTwo"].Success)
                {
                    rules.Add(
                        match.Groups["RuleTwo"].Captures.Select(c => int.Parse(c.Value))
                    );
                }
                if (match.Groups["Value"].Success)
                {
                    newRule.Character = match.Groups["Value"].Value[0];
                }

                ruleset.Add(ruleNum, newRule);
            }

            return match.Success;
        }

        public bool MatchRule(string password)
        {
            int index;
            bool match;
            (index, match) = MatchRuleIndex(0, password, 0);
            return match;
        }

        public (int nextIndex, bool succes) MatchRuleIndex(int ruleNum, string password, int index)
        {
            var rule = ruleset[ruleNum];

            if (rule.Character != default)
            {
                if (rule.Character == password[index])
                {
                    return (index + 1, true);
                }
                else
                {
                    return (index, false);
                }
            }

            return (0, false);
        }

    }

    // https://adventofcode.com/2020/day/19
    public class Day19
    {
        private ITestOutputHelper output;

        public Day19(ITestOutputHelper output)
        {
            this.output = output;
        }

        private RuleSet ReadRuleset(StreamReader reader)
        {
            var ruleset = new RuleSet();
            string input;
            while(!String.IsNullOrEmpty(input = reader.ReadLine()))
            {
                ruleset.AddRule(input);
            }
            return ruleset;
        }

        private IEnumerable<string> GetPasswords(StreamReader reader)
        {
            string input;
            while ((input = reader.ReadLine()) != null) {
                yield return input;
            }
        }

        [Fact]
        public void Part1()
        {
            using var reader = new StreamReader(InputClient.GetFileStream(2020, 19, ""));
            var ruleset = ReadRuleset(reader);
            var passwords = GetPasswords(reader);
            int numGoodPasswords = 0;

            foreach(var p in passwords)
            {
                if (ruleset.MatchRule(p))
                {
                    numGoodPasswords++;
                }
            }
            Assert.Equal(2, numGoodPasswords);
        }

        [Fact]
        public void Part2()
        {
            Assert.Equal(true, false);
        }
    }
}

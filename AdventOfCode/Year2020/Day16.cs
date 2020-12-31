using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Year2020
{
    public class TicketVerifier
    {
        private static readonly List<int> EMPTY = new List<int>();
        Dictionary<string, TicketRule> rules = new Dictionary<string, TicketRule>();

        public void AddRule(TicketRule rule)
        {
            rules.Add(rule.Name, rule);
        }

        public IEnumerable<int> Validate(Ticket ticket)
        {
            List<int> invalidValues = EMPTY;
            bool satisifed = true;
            foreach (var val in ticket.Values)
            {
                satisifed = false;
                foreach (var rule in rules.Values)
                {
                    satisifed = rule.Predicate(val);
                    if (satisifed) break;
                }

                if (!satisifed)
                {
                    if (invalidValues == EMPTY) invalidValues = new List<int>();

                    invalidValues.Add(val);
                    // break; // do not stop after first invalid value check all in ticket
                }
            }
            return invalidValues;
        }

        public Dictionary<string, int> IndexForRule(IEnumerable<Ticket> validTickets)
        {
            var results = new Dictionary<string, HashSet<int>>();
            int count = validTickets.First().Values.Count();
            var possibleIndexes = new List<int>(count);

            for (int i=0; i<count; i++) possibleIndexes.Add(i);
            foreach (var key in rules.Keys) results.Add(key, new HashSet<int>(possibleIndexes));

            var index = 0;
            foreach(var validTicket in validTickets)
            {
                index = default;
                foreach(var value in validTicket.Values)
                {
                    foreach (var rule in rules.Values)
                    {
                        if (!rule.Predicate(value))
                        {
                            results[rule.Name].Remove(index);
                        }
                    }
                    index++;
                }
            }

            var foundIndices = new Dictionary<string, int>();
            var addIndices = new Action<string, int>((key, index) => {
                foundIndices.Add(key, index);
                results.Remove(key);
            });
            for (bool foundNew = true; foundNew; )
            {
                foundNew = false;
                foreach (var kp in results)
                {
                    if (kp.Value.Count == 0)
                    {
                        results.Remove(kp.Key);
                    }
                    else if (kp.Value.Count == 1)
                    {
                        foundNew = true;
                        addIndices(kp.Key, kp.Value.First());
                        break;
                    }
                    else
                    {
                        foundNew = true;
                        kp.Value.ExceptWith(foundIndices.Values);
                        if (kp.Value.Count == 1)
                        {
                            addIndices(kp.Key, kp.Value.First());
                            break;
                        }
                    }
                }
            }

            return foundIndices;
        }
    }

    public class TicketRule
    {
        private static readonly Regex ruleEx = new Regex(@"([\w\s]+): (\d+)-(\d+) or (\d+)-(\d+)", RegexOptions.Compiled);

        public static TicketRule TryParse(string input)
        {
            var match = ruleEx.Match(input);
            if (!match.Success) return null;

            int v1 = int.Parse(match.Groups[2].Value);
            int v2 = int.Parse(match.Groups[3].Value);
            int v3 = int.Parse(match.Groups[4].Value);
            int v4 = int.Parse(match.Groups[5].Value);

            var pred = new Func<int, bool>(val => (val >= v1 && val <= v2) || (val >= v3 && val <= v4));

            return new TicketRule(match.Groups[1].Value, pred);
        }

        private TicketRule(string name, Func<int, bool> pred)
        {
            this.Name = name;
            this.Predicate = pred;
        }

        public string Name { get; private set; }
        public Func<int, bool> Predicate { get; private set; }
    }

    public class Ticket
    {
        private static readonly Regex numberListEx = new Regex(@"(\d+),?", RegexOptions.Compiled);
        private List<int> values;

        public static Ticket TryParse(string input)
        {
            if (!numberListEx.Match(input).Success) return null;

            return new Ticket(input.Split(',').Select(i => int.Parse(i)));
        }

        public Ticket(IEnumerable<int> input)
        {
            values = new List<int>(input);
        }

        public IEnumerable<int> Values { get => values; }
    }

    // https://adventofcode.com/2020/day/16
    // http://regexstorm.net/tester
    public class Day16
    {
        private ITestOutputHelper output;

        public Day16(ITestOutputHelper output)
        {
            this.output = output;
        }

        private TicketVerifier ReadVerifier(StreamReader reader)
        {
            string input;
            var verifier = new TicketVerifier();
            while((input = reader.ReadLine()) != string.Empty)
            {
                var rule = TicketRule.TryParse(input);
                if (rule != null)
                {
                    verifier.AddRule(rule);
                }
            }
            return verifier;
        }

        private Ticket ReadYourTicket(StreamReader reader)
        {
            string input;
            Ticket yourTicket = null;
            while(yourTicket == null && !string.IsNullOrEmpty(input = reader.ReadLine()))
            {
                yourTicket = Ticket.TryParse(input);
            }
            return yourTicket;
        }

        private IEnumerable<Ticket> ReadNearbyTickets(StreamReader reader)
        {
            string input;
            while ((input = reader.ReadLine()) != null)
            {
                var ticket = Ticket.TryParse(input);
                if (ticket != null) yield return ticket;
            }
        }

        [Fact]
        public void Part1()
        {
            using var reader = new StreamReader(InputClient.GetFileStream(2020, 16, ""));
            var verifier = ReadVerifier(reader);
            var yourTicket = ReadYourTicket(reader);
            var nearbyTickets = ReadNearbyTickets(reader);

            var scanningErrorRate = nearbyTickets.Select(nbTicket => verifier.Validate(nbTicket).Sum()).Sum();
            Assert.Equal(30869, scanningErrorRate);
        }

        [Fact]
        public void Part2()
        {
            using var reader = new StreamReader(InputClient.GetFileStream(2020, 16, ""));
            var verifier = ReadVerifier(reader);
            var yourTicket = ReadYourTicket(reader);
            var nearbyTickets = ReadNearbyTickets(reader);

            var validNearByTickets = nearbyTickets.Where(nbTicket => verifier.Validate(nbTicket).Sum() == 0);

            var ruleIndexes = verifier.IndexForRule(validNearByTickets);
            var departureIndexes = ruleIndexes.Where(kvPair => kvPair.Key.Contains("departure")).Select(kvPair => kvPair.Value);

            ulong total = 1UL;
            List<int> yourValues = yourTicket.Values.ToList();
            foreach (var index in departureIndexes)
            {
                total *= (ulong)yourValues[index];
            }

            // 505946183
            // 609507353
            Assert.Equal(4381476149273UL, total);
        }
    }
}

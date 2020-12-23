using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Year2020
{
    // https://adventofcode.com/2020/day/10
    public class Day10
    {
        private readonly ITestOutputHelper output;

        public Day10(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void Part1()
        {
            var input = InputClient.GetIntInput(2020, 10, "");
            var orderedJoltage = input.OrderBy(a => a).ToList();
            orderedJoltage.Insert(0, 0);
            orderedJoltage.Add(orderedJoltage.Max() + 3);
            int num1Diffs = 0;
            int num3Diffs = 0;
            for(int i=1; i< orderedJoltage.Count; i++)
            {
                if (orderedJoltage[i] - orderedJoltage[i - 1] == 1)
                {
                    num1Diffs++;
                }
                if (orderedJoltage[i] - orderedJoltage[i - 1] == 3)
                {
                    num3Diffs++;
                }
            }

            Assert.Equal(2812, num1Diffs * num3Diffs);
        }

        // https://www.geeksforgeeks.org/solve-dynamic-programming-problem/#:~:text=DP%20problems%20are%20all%20about%20state%20and%20their,what%20do%20we%20mean%20by%20the%20term%20%E2%80%9Cstate%E2%80%9D.
        [Fact]
        public void Part2()
        {
            var input = InputClient.GetIntInput(2020, 10, "");
            var orderedJoltage = input.OrderBy(a => a).ToList();
            orderedJoltage.Insert(0, 0);
            orderedJoltage.Add(orderedJoltage[orderedJoltage.Count - 1] + 3);

            int[] outlets = orderedJoltage.ToArray();
            int right = outlets.Length - 1;

            ulong count = NumberOfPermutations(outlets, right);

            var joltageDiffs = new List<int>(orderedJoltage.Count - 1);
            for (int i = 1; i < orderedJoltage.Count; i++)
            {
                joltageDiffs.Add(orderedJoltage[i] - orderedJoltage[i - 1]);
            }

            HashSet<string> uniqueArrangements = new HashSet<string>();
            Stack<List<int>> arrangements = new Stack<List<int>>();
            string arrangeString;
            arrangeString = joltageDiffs.Aggregate("", (str, n) => { str += n.ToString(); return str; });
            arrangements.Push(joltageDiffs);
            uniqueArrangements.Add(arrangeString);
            ulong numArrangements = 0;


            // Do not uncomment unless you want to peg the cpu
            //while (arrangements.Count > 0)
            {
                List<int> arrangement = arrangements.Pop();
                numArrangements += 1UL;

                if (numArrangements % 100000 == 0)
                {
                    output.WriteLine(numArrangements.ToString());
                }

                for (int i = 0; i < arrangement.Count - 2; i++)
                {
                    if (arrangement[i] == 1 && arrangement[i + 1] == 1 && arrangement[i + 2] == 1)
                    {
                        var newArrangement = new List<int>(arrangement);
                        newArrangement.RemoveRange(i, 3);
                        var newArrangement2 = new List<int>(newArrangement);
                        var newArrangement3 = new List<int>(newArrangement);
                        newArrangement.Insert(i, 1);
                        newArrangement.Insert(i + 1, 2);
                        newArrangement2.Insert(i, 2);
                        newArrangement2.Insert(i + 1, 1);
                        newArrangement3.Insert(i, 3);

                        arrangeString = newArrangement.Aggregate("", (str, n) => { str += n.ToString(); return str; });
                        if (uniqueArrangements.Add(arrangeString))
                        {
                            arrangements.Push(newArrangement);
                        }
                        arrangeString = newArrangement2.Aggregate("", (str, n) => { str += n.ToString(); return str; });
                        if (uniqueArrangements.Add(arrangeString))
                        {
                            arrangements.Push(newArrangement2);
                        }
                        arrangeString = newArrangement3.Aggregate("", (str, n) => { str += n.ToString(); return str; });
                        if (uniqueArrangements.Add(arrangeString))
                        {
                            arrangements.Push(newArrangement3);
                        }
                    }
                    else if (arrangement[i] == 1 && arrangement[i + 1] == 1 && arrangement[i + 2] == 2)
                    {
                        var newArrangement = new List<int>(arrangement);
                        newArrangement.RemoveRange(i, 3);
                        var newArrangement2 = new List<int>(newArrangement);
                        newArrangement.Insert(i, 1);
                        newArrangement.Insert(i + 1, 3);
                        newArrangement2.Insert(i, 2);
                        newArrangement2.Insert(i + 1, 2);
                        arrangeString = newArrangement.Aggregate("", (str, n) => { str += n.ToString(); return str; });
                        if (uniqueArrangements.Add(arrangeString))
                        {
                            arrangements.Push(newArrangement);
                        }
                        arrangeString = newArrangement2.Aggregate("", (str, n) => { str += n.ToString(); return str; });
                        if (uniqueArrangements.Add(arrangeString))
                        {
                            arrangements.Push(newArrangement2);
                        }
                    }
                    else if (arrangement[i] == 1 && arrangement[i + 1] == 2 && arrangement[i + 2] == 1)
                    {
                        var newArrangement = new List<int>(arrangement);
                        newArrangement.RemoveRange(i, 3);
                        var newArrangement2 = new List<int>(newArrangement);
                        newArrangement.Insert(i, 3);
                        newArrangement.Insert(i + 1, 1);
                        newArrangement2.Insert(i, 1);
                        newArrangement2.Insert(i + 1, 3);
                        arrangeString = newArrangement.Aggregate("", (str, n) => { str += n.ToString(); return str; });
                        if (uniqueArrangements.Add(arrangeString))
                        {
                            arrangements.Push(newArrangement);
                        }
                        arrangeString = newArrangement2.Aggregate("", (str, n) => { str += n.ToString(); return str; });
                        if (uniqueArrangements.Add(arrangeString))
                        {
                            arrangements.Push(newArrangement2);
                        }
                    }
                    else if (arrangement[i] == 2 && arrangement[i + 1] == 1 && arrangement[i + 2] == 1)
                    {
                        var newArrangement = new List<int>(arrangement);
                        newArrangement.RemoveRange(i, 3);
                        var newArrangement2 = new List<int>(newArrangement);
                        newArrangement.Insert(i, 3);
                        newArrangement.Insert(i + 1, 1);
                        newArrangement2.Insert(i, 2);
                        newArrangement2.Insert(i + 1, 2);
                        arrangeString = newArrangement.Aggregate("", (str, n) => { str += n.ToString(); return str; });
                        if (uniqueArrangements.Add(arrangeString))
                        {
                            arrangements.Push(newArrangement);
                        }
                        arrangeString = newArrangement2.Aggregate("", (str, n) => { str += n.ToString(); return str; });
                        if (uniqueArrangements.Add(arrangeString))
                        {
                            arrangements.Push(newArrangement2);
                        }
                    }
                    else if (arrangement[i] == 1 && arrangement[i + 1] == 1 && arrangement[i + 2] == 3)
                    {
                        var newArrangement = new List<int>(arrangement);
                        newArrangement.RemoveRange(i, 3);
                        newArrangement.Insert(i, 2);
                        newArrangement.Insert(i + 1, 3);
                        arrangeString = newArrangement.Aggregate("", (str, n) => { str += n.ToString(); return str; });
                        if (uniqueArrangements.Add(arrangeString))
                        {
                            arrangements.Push(newArrangement);
                        }
                    }
                    else if (arrangement[i] == 3 && arrangement[i + 1] == 1 && arrangement[i + 2] == 1)
                    {
                        var newArrangement = new List<int>(arrangement);
                        newArrangement.RemoveRange(i, 3);
                        newArrangement.Insert(i, 3);
                        newArrangement.Insert(i + 1, 2);
                        arrangeString = newArrangement.Aggregate("", (str, n) => { str += n.ToString(); return str; });
                        if (uniqueArrangements.Add(arrangeString))
                        {
                            arrangements.Push(newArrangement);
                        }
                    }
                }
            }

            Assert.Equal(19208UL, numArrangements);
        }

        private ulong NumberOfPermutations(int[] outlets, int candidate)
        {
            if (candidate > 0 && outlets[candidate + 1] - outlets[candidate-1] < 4)
            {
                var newOutlets = new int[outlets.Length - 1];
                
                outlets.CopyTo(newOutlets, candidate + 1);
                {
                };
            }
            return default;
        }

        public IEnumerable<List<int>> ReducingSets(List<int> lists)
        {

            return null;
        }
    }
}

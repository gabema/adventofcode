using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Year2020
{
    public class Decoder14
    {
        private Dictionary<int, ulong> memory = new Dictionary<int, ulong>();

        private const ulong MAX_SIZE = 0xffffffffffffffffUL;

        private ulong ONES_MASK = 0xffffffffffffffffUL;
        private ulong ZEROS_MASK = 0x0UL;

        public void SetMask(string mask)
        {
            ONES_MASK = 0x0;
            ZEROS_MASK = 0xffffffffffffffffUL;

            int offset = 35;
            foreach(char c in mask.ToCharArray())
            {
                if (c == '1') {
                    ONES_MASK = ONES_MASK |= 1UL << offset;
                }

                if (c == '0')
                {
                    ZEROS_MASK &= (MAX_SIZE ^ (0x1UL << offset));
                }
                offset--;
            }
        }

        public void SetMemory(int index, ulong value)
        {
            ulong saveValue = value;
            saveValue |= ONES_MASK;
            saveValue &= ZEROS_MASK;
            memory[index] = saveValue;
        }

        public ulong SumValues()
        {
            ulong totalValue = 0UL;
            foreach(ulong value in memory.Values)
            {
                totalValue += value;
            }
            return totalValue;
        }
    }

    // https://adventofcode.com/2020/day/14
    public class Day14
    {
        private readonly Regex maskEx = new Regex(@"mask = ([10X]+)", RegexOptions.Compiled);
        private readonly Regex memEx = new Regex(@"mem\[(\d+)\] = (\d+)", RegexOptions.Compiled);
        private readonly ITestOutputHelper output;

        public Day14(ITestOutputHelper output)
        {
            this.output = output;
        }

        public Decoder14 LoadSystem()
        {
            using var reader = new StreamReader(InputClient.GetFileStream(2020, 14, ""));
            var system = new Decoder14();
            string input;

            while ((input = reader.ReadLine()) != null)
            {
                var match = memEx.Match(input);
                if (match.Success)
                {
                    system.SetMemory(int.Parse(match.Groups[1].Value), ulong.Parse(match.Groups[2].Value));                    
                } else if ((match = maskEx.Match(input)).Success) {
                    system.SetMask(match.Groups[1].Value);
                }
            }

            return system;
        }

        [Fact]
        public void Part1()
        {
            Decoder14 system = LoadSystem();

            Assert.Equal(17481577045893UL, system.SumValues());
        }

        [Fact]
        public void Part2()
        {
            Assert.Equal(true, false);
        }

    }
}

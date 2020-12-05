using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Year2020
{
    public enum Day3Items
    {
        Open,
        Tree,
        OpenLanding,
        TreeLanding,
        Terminator
    }

    public class Day3Lexer : SimpleLexer<Day3Items>
    {
        private static readonly List<Tuple<Day3Items, string>> NewList = new List<Tuple<Day3Items, string>> {
            Tuple.Create(Day3Items.Open, @"^\."),
            Tuple.Create(Day3Items.Tree, @"^#"),
            Tuple.Create(Day3Items.Terminator, @""),
        };

        public Day3Lexer() : base(NewList)
        { }
    }

    public class MapBuilder
    {
        List<List<Day3Items>> map = new List<List<Day3Items>>();
        List<Day3Items> currentRow = new List<Day3Items>();

        public void AddEntry(Day3Items item)
        {
            if (item == Day3Items.Open || item == Day3Items.Tree) currentRow.Add(item);
        }

        public void AddRow()
        {
            map.Add(currentRow);
            currentRow = new List<Day3Items>();
        }

        public List<List<Day3Items>> Build() {
            var returnedMap = map;
            map = map = new List<List<Day3Items>>();
            currentRow = new List<Day3Items>();
            return returnedMap;
        }
    }

    // https://adventofcode.com/2020/day/3
    public class Day3
    {
        private readonly ITestOutputHelper output;

        public Day3(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void Part1()
        {
            var map = LoadMap();

            int x = 3;
            int treeLandingCount = 0;
            for (int y=1; y<map.Count; y++)
            {
                map[y][x] = map[y][x] == Day3Items.Open ? Day3Items.OpenLanding : Day3Items.TreeLanding;

                if (map[y][x] == Day3Items.TreeLanding) treeLandingCount++;

                x = (x + 3) % map[y].Count;
            }

            output.WriteLine($"{treeLandingCount}");

            // not 65 included terminator character in map at end of line
            // not 68 included terminator character in map at end of line AND incorrect offset calculation
            // not 251 incorrect offset calculation
            Assert.Equal(250, treeLandingCount);
        }

        [Fact]
        public void Part2()
        {
            var map = LoadMap();

            ulong total = 1UL;
            foreach (ulong v in CountTrees(map))
            {
                total *= v;
            }

            output.WriteLine($"{total}");

            // not 1658256600 (forgot to clone or better yet should not have modified original map just count trees)
            Assert.Equal(1592662500UL, total);
        }

        private static List<List<Day3Items>> LoadMap()
        {
            var stream = InputClient.GetFileStream(2020, 3, "");
            using var reader = new StreamReader(stream);
            var input = reader.ReadLine();
            var lexer = new Day3Lexer();
            var mapBuilder = new MapBuilder();
            while (input != null)
            {
                IEnumerable<Token<Day3Items>> tokens = lexer.Tokenize(input);
                foreach (var item in tokens)
                {
                    mapBuilder.AddEntry(item.TokenType);
                }
                mapBuilder.AddRow();
                input = reader.ReadLine();
            }
            return mapBuilder.Build();
        }

        private static List<List<Day3Items>> Clone (List<List<Day3Items>> map)
        {
            var newMap = new List<List<Day3Items>>(map.Count);
            for (int i = 0; i < map.Count; i++)
            {
                newMap.Add(new List<Day3Items>(map[i].AsEnumerable()));
            }
            return newMap;
        }

        private static ulong CountTreesForSlope(List<List<Day3Items>> baseMap, int slopeX, int slopeY)
        {
            List<List<Day3Items>> map = Clone(baseMap);
            int x = slopeX;
            ulong treeLandingCount = 0UL;
            for (int y = slopeY; y < map.Count; y += slopeY)
            {
                map[y][x] = map[y][x] == Day3Items.Open ? Day3Items.OpenLanding : Day3Items.TreeLanding;

                if (map[y][x] == Day3Items.TreeLanding) treeLandingCount++;

                x = (x + slopeX) % map[y].Count;
            }
            return treeLandingCount;
        }

        private static IEnumerable<ulong> CountTrees(List<List<Day3Items>> baseMap)
        {
            yield return CountTreesForSlope(Clone(baseMap), 1, 1);
            yield return CountTreesForSlope(Clone(baseMap), 3, 1);
            yield return CountTreesForSlope(Clone(baseMap), 5, 1);
            yield return CountTreesForSlope(Clone(baseMap), 7, 1);
            yield return CountTreesForSlope(Clone(baseMap), 1, 2);
        }
    }
}

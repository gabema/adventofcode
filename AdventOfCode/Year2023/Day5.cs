namespace AdventOfCode.Year2023;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;
using Xunit.Abstractions;

/*
seeds: 79 14 55 13

seed-to-soil map:
50 98 2
52 50 48

soil-to-fertilizer map:
0 15 37
37 52 2
39 0 15

fertilizer-to-water map:
49 53 8
0 11 42
42 0 7
57 7 4

water-to-light map:
88 18 7
18 25 70

light-to-temperature map:
45 77 23
81 45 19
68 64 13

temperature-to-humidity map:
0 69 1
1 0 69

humidity-to-location map:
60 56 37
56 93 4

 */

// https://adventofcode.com/2023/day/5
public class Day5
{
    private readonly ITestOutputHelper output;
    public Day5(ITestOutputHelper output)
    {
        this.output = output;
    }

    [Fact]
    public void PartA()
    {
        var (seeds, map) = ReadMap("");
        long lowestLocation = long.MaxValue;

        // Seed 79, soil 81, fertilizer 81, water 81, light 74, temperature 78, humidity 78, location 82
        foreach (var seed in seeds)
        {
            var seedThroughLists = seed;
            foreach(var list in map)
            {
                foreach (var listEntry in list)
                {
                    if (listEntry.Source.Included(seedThroughLists))
                    {
                        seedThroughLists = listEntry.Source.Map(listEntry.Destination, seedThroughLists);
                        break;
                    }
                }
            }
            lowestLocation = Math.Min(lowestLocation, seedThroughLists);
        }
        Assert.Equal(806029445, lowestLocation);
    }

    [Fact]
    public void PartB()
    {
        var (seeds, map) = ReadMap("");
        long lowestLocation = long.MaxValue;

        var seedPairs = seeds.Chunk(2);

        // Seed 79, soil 81, fertilizer 81, water 81, light 74, temperature 78, humidity 78, location 82
        foreach(var pair in seedPairs)
        {
            if (pair is null) continue;

            var end = pair[0] + pair[1];
            for(var seed = pair[0]; seed < end; seed++)
            {
                var seedThroughLists = (long)seed;
                foreach (var sortedList in map)
                {
                    var listLength = sortedList.Count;
                    var finished = false;
                    for(var listIndex = 0; listIndex < listLength / 2 && !finished; listIndex++)
                    {
                        var leftEntry = sortedList[listIndex];
                        if (leftEntry.Source.GreaterThan(seedThroughLists))
                        {
                            finished = true;
                            continue;
                        }
                        var rightEntry = sortedList[listLength - listIndex - 1];
                        if (rightEntry.Source.LessThan(seedThroughLists))
                        {
                            finished = true;
                            continue;
                        }

                        if (leftEntry.Source.Included(seedThroughLists))
                        {
                            seedThroughLists = leftEntry.Source.Map(leftEntry.Destination, seedThroughLists);
                            finished = true;
                            continue;
                        }
                        if (rightEntry.Source.Included(seedThroughLists))
                        {
                            seedThroughLists = rightEntry.Source.Map(rightEntry.Destination, seedThroughLists);
                            finished = true;
                            continue;
                        }
                    }
                    if (!finished && listLength % 2 == 1)
                    {
                        var entry = sortedList[listLength / 2];
                        if (entry.Source.Included(seedThroughLists))
                        {
                            seedThroughLists = entry.Source.Map(entry.Destination, seedThroughLists);
                        }
                    }
                }
                lowestLocation = Math.Min(lowestLocation, seedThroughLists);
            }
            output.WriteLine($"Seed Pair {pair[0]} {end} lowestLocation is {lowestLocation}");
        }
        Assert.Equal(46, lowestLocation);
    }

    /*
 863761171 233338109
+233338109
1198254212
+504239157
1808166864
+294882110
2313929258
 +84595688
2461206486
+133606394
3416930225
  56865175
3491380151
 178996923
3965970270
  15230597
4114335326
  67911591
4245248379
   7142355
     */
    private static readonly Regex SEEDS_REGEX = new Regex(@"seeds: ((\d+\s?)*)");
    private static readonly Regex MAP_RANGE_REGEX = new Regex(@"(\d+) (\d+) (\d+)");

    private (List<long>, List<List<(IntRange Source, IntRange Destination)>>) ReadMap(string variant)
    {
        using var reader = InputClient.GetFileStreamReader(2023, 5, variant);
        string? line;

        var mapList = new List<List<(IntRange Source, IntRange Destination)>>();
        List<(IntRange Source, IntRange Destination)>? currentMap = null;
        List<long>? seeds = null;

        while ((line = reader.ReadLine()) != null)
        {
            if (seeds is null)
            {
                var seedMatch = SEEDS_REGEX.Match(line);
                if (seedMatch.Success) {
                    seeds = seedMatch.Groups[1].Value.Split(' ').Select(long.Parse).ToList();
                }
                continue;
            }

            var m = MAP_RANGE_REGEX.Match(line);
            if (!m.Success)
            {
                if (currentMap is not null)
                {
                    currentMap.Sort((a, b) => (int)(a.Source.Start - b.Source.Start));
                    mapList.Add(currentMap);
                    currentMap = null;
                }
            }
            else
            {
                if (currentMap is null)
                {
                    currentMap = new List<(IntRange Source, IntRange Destination)>();
                }

                var startDestination = long.Parse(m.Groups[1].Value);
                var startSource = long.Parse(m.Groups[2].Value);
                var range = int.Parse(m.Groups[3].Value);
                currentMap.Add(
                    (
                        Source: new IntRange(startSource, startSource + range),
                        Destination: new IntRange(startDestination, startDestination + range)
                    )
                );
            }
        }

        return (seeds, mapList);
    }

    public record IntRange(long Start, long End)
    {
        public bool Included(long value) => value >= Start && value < End;

        public long Map(IntRange destination, long value) => destination.Start + (value - Start);

        public bool GreaterThan(long value) => value < Start;
        public bool LessThan(long value) => value >= End;
    }
}

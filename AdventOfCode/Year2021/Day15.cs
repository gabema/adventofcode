using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Year2021;

// https://adventofcode.com/2021/day/15
public class Day15
{
    private readonly ITestOutputHelper _output;
    public Day15(ITestOutputHelper output)
    {
        _output = output;
    }

    List<int> ReadRiskMap(string variant)
    {
        var reader = InputClient.GetFileStreamReader(2021, 15, variant);
        var map = new List<int>();
        string line;

        while((line = reader.ReadLine()) != null)
        {
            map.AddRange(line.Select(c => int.Parse(c.ToString())));
        }
        return map;
    }

    class Stats
    {
        public static int ShortestPathRisk { get; set; } = int.MaxValue;
        public static int MapWidthHeight { get; set; }

        public int CurrentPosition { get; set; } = 0;
        private HashSet<int> VisitedIndexes { get; init; } = new HashSet<int>();

        public int CurrentPathRisk { get; set; } = 0;

        public Stats()
        {
        }

        public Stats(Stats copy)
        {
            CurrentPosition = copy.CurrentPosition;
            CurrentPathRisk = copy.CurrentPathRisk;
            VisitedIndexes = new HashSet<int>(copy.VisitedIndexes);
        }
    }

    enum Direction
    {
        Right,
        Down,
        Left,
        Up,
    }

    [Fact]
    public void PartA()
    {
        var map = ReadRiskMap("Sample");
        var stats = new Stats();

        foreach(var direction in Enum.GetValues(typeof(Direction)))
        {

        }

        Assert.True(false);
    }

    private static void SolveForShortestPath(List<int> map, Stats stats)
    {
        var risk = map[stats.CurrentPosition];
        stats.CurrentPathRisk += risk;
        if (stats.CurrentPathRisk > Stats.ShortestPathRisk) return; // done - path risk too great
        if (stats.CurrentPosition == map.Count - 1) // found a shorter path to the end
        {
            Stats.ShortestPathRisk = stats.CurrentPathRisk;
            return; // done
        }
        //if (CanMoveRight(stats))
        //{
        //    MoveRight(map, new Stats(stats));
        //    if (stats.CurrentPathRisk > Stats.ShortestPathRisk) return; // done - path risk too great
        //}
    }

    [Fact]
    public void PartB()
    {
        Assert.True(false);
    }
}

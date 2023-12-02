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

        public bool Go(Direction direction)
        {
            var nextIndex = 0;
            switch(direction)
            {
                case Direction.Left:
                    {
                        nextIndex = CurrentPosition - 1;
                        if (nextIndex % MapWidthHeight == MapWidthHeight - 1) return false; // left column
                        if (nextIndex < 0) return false; // 0 position;
                        break;
                    }
                case Direction.Right:
                    {
                        nextIndex = CurrentPosition + 1;
                        if (nextIndex % MapWidthHeight == 0) return false; // right column
                        if (nextIndex == MapWidthHeight * MapWidthHeight) return false; // beyond end
                        break;
                    }
                case Direction.Down:
                    {
                        nextIndex = CurrentPosition + MapWidthHeight;
                        if (nextIndex >= MapWidthHeight * MapWidthHeight) return false;
                        break;
                    }
                case Direction.Up:
                    {
                        nextIndex = CurrentPosition - MapWidthHeight;
                        if (nextIndex < 0) return false;
                        break;
                    }
            }
            if (VisitedIndexes.Contains(nextIndex)) return false; // already visited
            CurrentPosition = nextIndex;
            VisitedIndexes.Add(nextIndex);
            return true;
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

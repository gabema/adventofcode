namespace AdventOfCode.Year2023;

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;
using Xunit.Abstractions;

// https://adventofcode.com/2023/day/2
public class Day2
{
    private readonly ITestOutputHelper output;
    private static readonly Regex gameExp = new Regex(@"Game (\d+): (.*)");
    private static readonly Regex colorExp = new Regex(@"(\d+) (\w+)");
    public Day2(ITestOutputHelper output)
    {
        this.output = output;
    }

    record CubeGame(int GameNumber, List<List<CubeSet>> CubeSets);
    record CubeSet(string color, int count);
    private IEnumerable<string> GameEnumerable()
    {
        using var reader = InputClient.GetFileStreamReader(2023, 2, "");
        string line;

        while ((line = reader.ReadLine()) != null)
        {
            yield return line;
        }
    }

    private IEnumerable<List<CubeSet>> FromString(string v)
    {
        var games = v.Split([';']);
        foreach (var game in games)
        {
            yield return GetCubeList(game).ToList();
        }
    }

    private IEnumerable<CubeSet> GetCubeList(string v)
    {
        var colors = v.Split([',']);
        foreach (var color in colors)
        {
            var m = colorExp.Match(color);
            yield return new CubeSet(count: int.Parse(m.Groups[1].Value), color: m.Groups[2].Value);
        }
    }
    private IEnumerable<CubeGame> GetGame()
    {
        foreach (var gameLine in GameEnumerable())
        {
            var gameMatch = gameExp.Match(gameLine);

            yield return new CubeGame(int.Parse(gameMatch.Groups[1].Value), FromString(gameMatch.Groups[2].Value).ToList());
        }
    }

    [Fact]
    public void PartA()
    {
        var possibleGameIdSums = 0;
        var maxColorCounts = new Dictionary<string, int>()
        {
            ["blue"] = 14,
            ["green"] = 13,
            ["red"] = 12
        };

        var colorCubeCounts = new Dictionary<string, int>()
        {
            ["blue"] = 0,
            ["green"] = 0,
            ["red"] = 0
        };

        foreach (var game in GetGame())
        {
            var gameFails = false;

            foreach (var cubesets in game.CubeSets)
            {
                colorCubeCounts["blue"] = 0;
                colorCubeCounts["green"] = 0;
                colorCubeCounts["red"] = 0;

                foreach (var cs in cubesets)
                {
                    colorCubeCounts[cs.color] += cs.count;
                }

                if (maxColorCounts["red"] < colorCubeCounts["red"]
                    || maxColorCounts["green"] < colorCubeCounts["green"]
                    || maxColorCounts["blue"] < colorCubeCounts["blue"]
                    )
                {
                    //output.WriteLine($"Game {game.GameNumber} failed! R{maxColorCounts["red"]},G{maxColorCounts["green"]},B{maxColorCounts["blue"]} | R{colorCubeCounts["red"]},G{colorCubeCounts["green"]},B{colorCubeCounts["blue"]}");
                    gameFails = true;
                    break;
                }
            }
            if (!gameFails)
            {
                possibleGameIdSums += game.GameNumber;
            }
        }

        // 686 is too low
        // 290 is too low
        Assert.Equal(2541, possibleGameIdSums);
    }

    [Fact]
    public void PartB()
    {
        var possibleGameIdSums = 0L;

        var colorCubeCounts = new Dictionary<string, int>()
        {
            ["blue"] = 0,
            ["green"] = 0,
            ["red"] = 0
        };

        foreach (var game in GetGame())
        {
            var minColorCountsForGame = new Dictionary<string, int>()
            {
                ["blue"] = 0,
                ["green"] = 0,
                ["red"] = 0
            };

            foreach (var cubesets in game.CubeSets)
            {
                colorCubeCounts["blue"] = 0;
                colorCubeCounts["green"] = 0;
                colorCubeCounts["red"] = 0;

                foreach (var cs in cubesets)
                {
                    colorCubeCounts[cs.color] += cs.count;
                }

                if (colorCubeCounts["blue"] > minColorCountsForGame["blue"]) { minColorCountsForGame["blue"] = colorCubeCounts["blue"];  }
                if (colorCubeCounts["green"] > minColorCountsForGame["green"]) { minColorCountsForGame["green"] = colorCubeCounts["green"]; }
                if (colorCubeCounts["red"] > minColorCountsForGame["red"]) { minColorCountsForGame["red"] = colorCubeCounts["red"]; }
            }

            var gamePower = minColorCountsForGame["blue"] * minColorCountsForGame["green"] * minColorCountsForGame["red"];
            possibleGameIdSums += gamePower;
        }

        Assert.Equal(66016, possibleGameIdSums);
    }
}

namespace AdventOfCode.Year2023;

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;
using Xunit.Abstractions;

// https://adventofcode.com/2023/day/2
public class Day2
{
    private const string RED = "red";
    private const string GREEN = "green";
    private const string BLUE = "blue";

    private readonly ITestOutputHelper output;
    private static readonly Regex gameExp = new Regex(@"Game (\d+): (.*)");
    private static readonly Regex colorExp = new Regex(@"(\d+) (\w+)");

    record CubeGame(int GameNumber, List<List<CubeSet>> CubeSets);
    record CubeSet(string color, int count);

    public Day2(ITestOutputHelper output)
    {
        this.output = output;
    }

    private IEnumerable<string> GameEnumerable(string variant)
    {
        using var reader = InputClient.GetFileStreamReader(2023, 2, variant);
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
    private IEnumerable<CubeGame> GetGame(string variant)
    {
        foreach (var gameLine in GameEnumerable(variant))
        {
            var gameMatch = gameExp.Match(gameLine);

            yield return new CubeGame(int.Parse(gameMatch.Groups[1].Value), FromString(gameMatch.Groups[2].Value).ToList());
        }
    }

    [Fact]
    public void PartA()
    {
        var possibleGameIdSums = 0;
        var maxColorCounts = new Dictionary<string, int>();
        var colorCubeCounts = new Dictionary<string, int>();

        SetDictionaryValues(maxColorCounts, red:12, green:13, blue:14);

        foreach (var game in GetGame(string.Empty))
        {
            var gameFails = false;

            foreach (var cubesets in game.CubeSets)
            {
                SetDictionaryValues(colorCubeCounts, 0, 0, 0);

                foreach (var cs in cubesets)
                {
                    colorCubeCounts[cs.color] += cs.count;
                }

                if (maxColorCounts[RED] < colorCubeCounts[RED]
                    || maxColorCounts[GREEN] < colorCubeCounts[GREEN]
                    || maxColorCounts[BLUE] < colorCubeCounts[BLUE]
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

        var colorCubeCounts = new Dictionary<string, int>();
        var minColorCountsForGame = new Dictionary<string, int>();

        foreach (var game in GetGame(string.Empty))
        {
            SetDictionaryValues(minColorCountsForGame, 0, 0, 0);

            foreach (var cubesets in game.CubeSets)
            {
                SetDictionaryValues(colorCubeCounts, 0, 0, 0);

                foreach (var cs in cubesets)
                {
                    colorCubeCounts[cs.color] += cs.count;
                }

                if (colorCubeCounts[BLUE] > minColorCountsForGame[BLUE]) { minColorCountsForGame[BLUE] = colorCubeCounts[BLUE];  }
                if (colorCubeCounts[GREEN] > minColorCountsForGame[GREEN]) { minColorCountsForGame[GREEN] = colorCubeCounts[GREEN]; }
                if (colorCubeCounts[RED] > minColorCountsForGame[RED]) { minColorCountsForGame[RED] = colorCubeCounts[RED]; }
            }

            var gamePower = minColorCountsForGame[BLUE] * minColorCountsForGame[GREEN] * minColorCountsForGame[RED];
            possibleGameIdSums += gamePower;
        }

        Assert.Equal(66016, possibleGameIdSums);
    }

    private static void SetDictionaryValues(Dictionary<string, int> dict, int red, int green, int blue)
    {
        dict[RED] = red;
        dict[GREEN] = green;
        dict[BLUE] = blue;
    }
}

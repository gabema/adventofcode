using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Year2021;

// https://adventofcode.com/2021/day/9
public class Day9
{
    private readonly ITestOutputHelper _output;

    private static int[][] ReadMap(string variant)
    {
        var contents = ReadMapLine(variant).ToList();
        var mapHeight = contents.Count;
        int index = 0;
        return contents.Aggregate(new int[mapHeight][], (acc, line) => {
            acc[index++] = line.Select(v => int.Parse($"{v}")).ToArray();
            return acc;
        });
    }

    private static IEnumerable<string> ReadMapLine(string variant)
    {
        using var reader = InputClient.GetFileStreamReader(2021, 9, variant);
        var line = "";
        while ((line = reader.ReadLine()) != null)
        {
            yield return line;
        }
    }

    public Day9(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void PartA()
    {
        var map = ReadMap("");
        var width = map[0].Length;
        var height = map.Length;
        var riskLevel = 0;

        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                if (IsLowPoint(map, x, y, width, height))
                {
                    riskLevel += map[y][x] + 1;
                }
            }
        }

        // not the right answer: 1508
        //Assert.Equal(15, riskLevel);
        Assert.Equal(478, riskLevel);
    }
    private bool IsLowPoint(int[][] map, int x, int y, int width, int height)
    {
        // Check Top
        if (y - 1 >= 0 && map[y][x] >= map[y - 1][x]) { return false; }

        // Check Bottom
        if (y + 1 < height && map[y][x] >= map[y + 1][x]) { return false; }

        // Check Right
        if (x + 1 < width && map[y][x] >= map[y][x + 1]) { return false; }

        // Check Left
        if ((x - 1) >= 0 && map[y][x] >= map[y][x - 1]) { return false; }
        return true;
    }

    private bool IsLowExcept(int[][] map, int x, int y, int excludeX, int excludeY, int width, int height)
    {
        // Check Top
        if (y - 1 >= 0 && y-1!=excludeY && map[y][x] >= map[y - 1][x]) { return false; }

        // Check Bottom
        if (y + 1 < height && y+1!=excludeY && map[y][x] >= map[y + 1][x]) { return false; }

        // Check Right
        if (x + 1 < width && x+1!=excludeX && map[y][x] >= map[y][x + 1]) { return false; }

        // Check Left
        if ((x - 1) >= 0 && x - 1 != excludeX && map[y][x] >= map[y][x - 1]) { return false; }
        return true;
    }

    [Fact]
    public void PartB()
    {
        var map = ReadMap("");
        var width = map[0].Length;
        var height = map.Length;
        var basins = new List<int>();

        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                if (IsLowPoint(map, x, y, width, height))
                {
                    HashSet<int> inBasin = new HashSet<int>(x + y * width);
                    basins.Add(BasinValue(map, inBasin, x, y, width, height));
                }
            }
        }

        Assert.Equal(1327014, basins.OrderByDescending(b => b).Take(3).Aggregate(1, (agg, v) => agg* v));
    }

    private int BasinValue(int[][] map, HashSet<int> inBasin, int x, int y, int width, int height)
    {
        // Check Top
        if (y - 1 >= 0 && map[y-1][x] != 9 && !inBasin.Contains(x + (y-1)* width)) {
            inBasin.Add(x + (y - 1) * width);
            BasinValue(map, inBasin, x, y - 1, width, height);
        }

        // Check Bottom
        if (y + 1 < height && map[y+1][x] != 9 && !inBasin.Contains(x + (y+1)* width)) {
            inBasin.Add(x + (y + 1) * width);
            BasinValue(map, inBasin, x, y + 1, width, height);
        }

        // Check Right
        if (x + 1 < width && map[y][x + 1] != 9 && !inBasin.Contains(x + 1 + y * width)) {
            inBasin.Add(x + 1 + y * width);
            BasinValue(map, inBasin, x + 1, y, width, height);
        }

        // Check Left
        if ((x - 1) >= 0 && map[y][x - 1] != 9 && !inBasin.Contains(x - 1 + y * width)) {
            inBasin.Add(x - 1 + y * width);
            BasinValue(map, inBasin, x - 1, y, width, height);
        }

        return inBasin.Count();
    }
}

using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Year2021;

// https://adventofcode.com/2021/day/13
public class Day13
{
    class Parts
    {
        public List<Point> Points { get; init; }
        public List<(char Coordinate, int index)> Folds { get; init; }
    }

    private static Parts ReadParts(string variant)
    {
        var parts = new Parts
        {
            Points = new List<Point>(),
            Folds = new List<(char Coordinate, int index)>()
        };

        using var reader = InputClient.GetFileStreamReader(2021, 13, variant);
        string line;

        while ((line = reader.ReadLine()).Length!=0)
        {
            var p = line.Split(',');
            parts.Points.Add(new Point(int.Parse(p[0]), int.Parse(p[1])));
        }

        while ((line = reader.ReadLine()) != null)
        {
            var p = line.Split(' ');
            var v = p[2].Split('=');
            parts.Folds.Add((v[0][0], int.Parse(v[1])));
        }
        return parts;
    }

    private static List<Point> FoldedPoints(List<Point> pts, (char Coordinate, int index) fold)
    {
        IEnumerable<Point> newPoints = Enumerable.Empty<Point>();
        if (fold.Coordinate=='x')
        {
            newPoints = pts.Select(p => {
                if (p.X >= fold.index)
                {
                    var x = fold.index - (p.X - fold.index);
                    return new Point(x, p.Y);
                }
                return p;
            });
        }
        else if (fold.Coordinate == 'y')
        {
            newPoints = pts.Select(p => {
                if (p.Y >= fold.index)
                {
                    var y = fold.index - (p.Y - fold.index);
                    return new Point(p.X, y);
                }
                return p;
            });
        }
        return newPoints.Distinct().ToList();
    }

    private readonly ITestOutputHelper _output;
    public Day13(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void PartA()
    {
        var parts = ReadParts("");
        var newPoints = parts.Folds.Take(1).Aggregate(parts.Points, FoldedPoints);

        Assert.Equal(666, newPoints.Count());
    }

    [Fact]
    public void PartB()
    {
        var parts = ReadParts("");
        var newPoints = parts.Folds.Aggregate(parts.Points, FoldedPoints);

        DrawList(newPoints);

        Assert.Equal(97, newPoints.Count());
    }

    private void DrawList(List<Point> pts)
    {
        int width = pts.Aggregate(0, (max, c) => max > c.X ? max : c.X) + 1;
        int height = pts.Aggregate(0, (max, c) => max > c.Y ? max : c.Y) + 1;

        char[][] grid = new char[height][];
        for(int y = 0; y < height; y++)
        {
            grid[y] = new char[width];
            for (int x = 0; x < width; x++)
            {
                grid[y][x] = '.';
            }
        }

        foreach(var point in pts)
        {
            grid[point.Y][point.X] = 'X';
        }

        for (int y = 0; y < height; y++)
        {
            _output.WriteLine(new string(grid[y]));
        }
    }
}

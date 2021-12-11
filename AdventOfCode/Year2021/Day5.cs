using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Year2021;

// https://adventofcode.com/2021/day/2
public class Day5
{
    //public Point point;

    //public class PointCounter : IEquatable<PointCounter>
    //{
    //    Point Point { get; init; }
    //    int Count { get; set; }

    //    public override int GetHashCode() => Point.GetHashCode();
    //    public override bool Equals(object obj) => Equals(obj as PointCounter);

    //    public bool Equals(PointCounter other)
    //    {
    //        ReferenceEquals(null, other);
    //        ReferenceEquals(this, other);
    //        return Point.Equals(other.Point);
    //    }
    //}

    private static Point XuYd(Point pt) => new Point(pt.X + 1, pt.Y - 1);
    private static Point XdYu(Point pt) => new Point(pt.X - 1, pt.Y + 1);
    private static Point XuYu(Point pt) => new Point(pt.X + 1, pt.Y + 1);
    private static Point XdYd(Point pt) => new Point(pt.X - 1, pt.Y - 1);

    private IDictionary<Point, int> GetPointerCounters(string variant, bool ignoreDiaganols)
    {
        var set = new Dictionary<Point, int>();
        foreach(var line in InputClient.GetRegularExpressionRows(2021, 5, variant, @"(\d+),(\d+) -> (\d+),(\d+)"))
        {
            var x1 = int.Parse(line[1].Value);
            var y1 = int.Parse(line[2].Value);
            var x2 = int.Parse(line[3].Value);
            var y2 = int.Parse(line[4].Value);
            if (x1 == x2)
            {
                // horizontal line
                int yMax = Math.Max(y1, y2);
                for (int i = Math.Min(y1, y2); i <= yMax; i++)
                {
                    var p = new Point(x1, i);
                    if (set.ContainsKey(p))
                    {
                        set[p] = set[p] + 1;
                    }
                    else
                    {
                        set.Add(p, 1);
                    }
                }
            }
            else if (y1 == y2)
            {
                // vertical line
                int xMax = Math.Max(x1, x2);
                for (int i = Math.Min(x1, x2); i <= xMax; i++)
                {
                    var p = new Point(i, y1);
                    if (set.ContainsKey(p))
                    {
                        set[p] = set[p] + 1;
                    }
                    else
                    {
                        set.Add(p, 1);
                    }
                }
            }
            else if (!ignoreDiaganols)
            {
                // diaganol line
                Func<Point, Point> next = (x1 > x2 && y1 > y2) ? XdYd :
                    (x1 > x2 && y1 < y2) ? XdYu :
                    (x1 < x2 && y1 < y2) ? XuYu : XuYd;
                Point start = new(x1, y1);
                Point end = new(x2, y2);

                for (; start != end; start = next(start))
                {
                    if (set.ContainsKey(start))
                    {
                        set[start] = set[start] + 1;
                    }
                    else
                    {
                        set.Add(start, 1);
                    }
                }

                if (set.ContainsKey(start))
                {
                    set[start] = set[start] + 1;
                }
                else
                {
                    set.Add(start, 1);
                }
            }
        }
        return set;
    }

    private readonly ITestOutputHelper _output;
    public Day5(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void PartA()
    {
        var grid = GetPointerCounters("", true);
        Assert.Equal(6267, grid.Values.Where(count => count > 1).Count());
    }

    [Fact]
    public void PartB()
    {
        var grid = GetPointerCounters("", false);
        // Not right: 20194 - your answer is too low
        Assert.Equal(20196, grid.Values.Where(count => count > 1).Count());
    }
}

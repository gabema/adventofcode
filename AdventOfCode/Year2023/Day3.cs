namespace AdventOfCode.Year2023;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Xunit.Abstractions;

// https://adventofcode.com/2023/day/3
public class Day3
{
    enum SpotType
    {
        Number,
        Symbol,
        Blank
    }
    record Point(int X, int Y);
    record Spot(SpotType ST, object? Item);
    record NumPoints(int value, List<Point> points);

    private (Dictionary<Point, Spot>, List<NumPoints>) ReadBoard(string variant)
    {
        using var reader = InputClient.GetFileStreamReader(2023, 3, variant);
        string line;
        var y = 0;
        var numBuilder = new StringBuilder();
        var buildingNumberStartIndex = -1;
        var board = new Dictionary<Point, Spot>();
        var numBoard = new List<NumPoints>();

        while ((line = reader.ReadLine()) != null)
        {
            int l = line.Length;
            for (int i = 0; i < l; i++)
            {
                if (char.IsAsciiDigit(line[i]))
                {
                    numBuilder.Append(line[1]);
                    if (buildingNumberStartIndex == -1)
                    {
                        buildingNumberStartIndex = i;
                    }
                    continue;
                }

                if (buildingNumberStartIndex != -1)
                {
                    var v = int.Parse(line.Substring(buildingNumberStartIndex, i - buildingNumberStartIndex));
                    List<Point> points = new List<Point>(i - buildingNumberStartIndex);
                    for(var j = buildingNumberStartIndex; j < i; j++)
                    {
                        var p = new Point(j, y);
                        points.Add(p);
                        board.Add(p, new Spot(SpotType.Number, v));
                    }
                    numBoard.Add(new NumPoints(v, points));
                    buildingNumberStartIndex = -1;
                    numBuilder.Length = 0;
                }

                if (line[i] == '.')
                {
                    board.Add(new Point(i, y), new Spot(SpotType.Blank, null));
                }
                else
                {
                    board.Add(new Point(i, y), new Spot(SpotType.Symbol, line[i]));
                }
            }

            if (buildingNumberStartIndex != -1)
            {
                var v = int.Parse(line.Substring(buildingNumberStartIndex, l - buildingNumberStartIndex));
                List<Point> points = new List<Point>(l - buildingNumberStartIndex);
                for (var j = buildingNumberStartIndex; j < l; j++)
                {
                    var p = new Point(j, y);
                    points.Add(p);
                    board.Add(p, new Spot(SpotType.Number, v));
                }
                numBoard.Add(new NumPoints(v, points));
                buildingNumberStartIndex = -1;
                numBuilder.Length = 0;
            }
            y++;
        }
        return (board, numBoard);
    }

    private readonly ITestOutputHelper output;
    public Day3(ITestOutputHelper output)
    {
        this.output = output;
    }

    [Fact]
    public void PartA()
    {
        var (board, numBoard) = ReadBoard("");
        int sumPartNumbers = 0;

        foreach (var nb in numBoard)
        {
            bool hasSurroundingSymbol = false;
            foreach(var p in nb.points)
            {
                foreach(var p2 in SurroundingPoints(board, p))
                {
                    if (board[p2].ST == SpotType.Symbol)
                    {
                        hasSurroundingSymbol = true;
                        break;
                    }
                }
                if (hasSurroundingSymbol)
                {
                    break;
                }
            }

            if (hasSurroundingSymbol)
            {
                sumPartNumbers += nb.value;
            }
        }

        Assert.Equal(530495, sumPartNumbers);
    }

    [Fact]
    public void PartB()
    {
        var (board, _) = ReadBoard("");
        var ratioSums = 0L;

        foreach(var kv in board)
        {
            if (kv.Value.ST != SpotType.Symbol || (char)kv.Value.Item! != '*') { continue; }

            var surroundingNums = SurroundingGearSymbolNumberPoints(board, kv.Key).Take(3).ToList();
            if (surroundingNums.Count == 2)
            {
                ratioSums += (int)board[surroundingNums[0]].Item!
                             * (int)board[surroundingNums[1]].Item!;
            }
        }
        Assert.Equal(80253814, ratioSums);
    }
    private static IEnumerable<Point> SurroundingGearSymbolNumberPoints(Dictionary<Point, Spot> schematic, Point startPoint)
    {
        var p = schematic.GetValueOrDefault(startPoint);
        if (p?.ST == SpotType.Symbol && (char)p.Item! == '*')
        {
            var point = startPoint with { X = startPoint.X - 1 };

            p = schematic.GetValueOrDefault(point);
            if (p?.ST == SpotType.Number) { yield return point; }

            point = startPoint with { X = startPoint.X + 1 };
            p = schematic.GetValueOrDefault(point);
            if (p?.ST == SpotType.Number) { yield return point; }

            var topLeftPoint = startPoint with { X = startPoint.X - 1, Y = startPoint.Y - 1 };
            var bottomLeftPoint = startPoint with { X = startPoint.X - 1, Y = startPoint.Y + 1 };
            var topRightPoint = startPoint with { X = startPoint.X + 1, Y = startPoint.Y - 1 };
            var bottomRightPoint = startPoint with { X = startPoint.X + 1, Y = startPoint.Y + 1 };
            var topMiddlePoint = startPoint with { Y = startPoint.Y - 1 };
            var bottomMiddlePoint = startPoint with { Y = startPoint.Y + 1 };
            var tl = schematic.GetValueOrDefault(topLeftPoint);
            var t = schematic.GetValueOrDefault(topMiddlePoint);
            var tr = schematic.GetValueOrDefault(topRightPoint);
            if (tl?.ST == SpotType.Number)
            {
                if (t?.ST != SpotType.Number)
                {
                    if (tr?.ST == SpotType.Number)
                    {
                        yield return topRightPoint;
                    }
                }
                yield return topLeftPoint;
            }
            else if (t?.ST == SpotType.Number)
            {
                yield return topMiddlePoint;
            }
            else if (tr?.ST == SpotType.Number)
            {
                yield return topRightPoint;
            }

            tl = schematic.GetValueOrDefault(bottomLeftPoint);
            t = schematic.GetValueOrDefault(bottomMiddlePoint);
            tr = schematic.GetValueOrDefault(bottomRightPoint);
            if (tl?.ST == SpotType.Number)
            {
                if (t?.ST != SpotType.Number)
                {
                    if (tr?.ST == SpotType.Number)
                    {
                        yield return bottomRightPoint;
                    }
                }

                yield return bottomLeftPoint;
            }
            else if (t?.ST == SpotType.Number)
            {
                yield return bottomMiddlePoint;
            }
            else if (tr?.ST == SpotType.Number)
            {
                yield return bottomRightPoint;
            }

        }
    }

    private static IEnumerable<Point> SurroundingPoints(Dictionary<Point, Spot> schematic, Point p)
    {
        if (schematic[p].ST == SpotType.Number)
        {
            var otherPoint = p with { X = p.X - 1 };
            bool hasNumLeft = false;
            bool hasNumRight = false;
            if (schematic.TryGetValue(otherPoint, out Spot leftSpot))
            {
                hasNumLeft = leftSpot.ST == SpotType.Number;
                if (!hasNumLeft) { yield return otherPoint; }
            }
            otherPoint = p with { X = p.X + 1 };
            if (schematic.TryGetValue(otherPoint, out Spot rightSpot))
            {
                hasNumRight = rightSpot.ST == SpotType.Number;
                if (!hasNumRight) { yield return otherPoint; }
            }
            otherPoint = p with { Y = p.Y - 1 };
            if (schematic.ContainsKey(otherPoint))
            {
                yield return otherPoint;
            }
            otherPoint = p with { Y = p.Y + 1 };
            if (schematic.ContainsKey(otherPoint))
            {
                yield return otherPoint;
            }

            if (!hasNumLeft)
            {
                otherPoint = p with { Y = p.Y - 1, X = p.X - 1 };
                if (schematic.ContainsKey(otherPoint))
                {
                    yield return otherPoint;
                }
                otherPoint = p with { Y = p.Y + 1, X = p.X - 1 };
                if (schematic.ContainsKey(otherPoint))
                {
                    yield return otherPoint;
                }
            }
            if (!hasNumRight)
            {
                otherPoint = p with { Y = p.Y - 1, X = p.X + 1 };
                if (schematic.ContainsKey(otherPoint))
                {
                    yield return otherPoint;
                }
                otherPoint = p with { Y = p.Y + 1, X = p.X + 1 };
                if (schematic.ContainsKey(otherPoint))
                {
                    yield return otherPoint;
                }
            }
        }
    }
}

namespace AdventOfCode.Year2024;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

record Location(int X, int Y)
{
    public static Location Origin => new(0, 0);
    private Location Right => this with { X = X + 1 };
    private Location Down => this with { Y = Y + 1 };
    private Location Up => this with { Y = Y - 1 };
    private Location Left => this with { X = X - 1 };

    public Location NewRow => new(0, Y + 1);

    public Location FromDirection(Direction d) => d switch
    {
        Direction.Left => Left,
        Direction.Right => Right,
        Direction.Up => Up,
        Direction.Down => Down,
        Direction.UpperLeft => Up.Left,
        Direction.UpperRight => Up.Right,
        Direction.LowerLeft => Down.Left,
        Direction.LowerRight => Down.Right,
        _ => throw new InvalidEnumArgumentException()
    };
}

enum Direction
{
    Left,
    Right,
    Up,
    Down,
    UpperLeft,
    UpperRight,
    LowerLeft,
    LowerRight
}

class Value(char? v)
{
    public Value() : this(null) { }

    public char? V
    {
        get => v;
        set
        {
            v = value;
        }
    }

    // override public string ToString() => v?.ToString() ?? ".";
}

record Spot(Location Location, Value Value);

// https://adventofcode.com/2024/day/4
public class Day4(ITestOutputHelper output)
{
    static Spot CachedLocationGetter(Dictionary<Location, Spot> map, Location location)
    {
        if (map.TryGetValue(location, out var spot))
        {
            return spot;
        }
        else
        {
            var newSpot = new Spot(location, new Value());
            map.Add(location, newSpot);
            return newSpot;
        }
    }

    static IEnumerable<Spot> GetLetter(Dictionary<Location, Spot> map, char value)
    {
        foreach (var spot in map.Values)
        {
            if (spot.Value.V == value)
            {
                yield return spot;
            }
        }
    }

    private static bool HasMas(Dictionary<Location, Spot> map, Spot aSpot)
    {
        var upperLeft = map.GetValueOrDefault(aSpot.Location.FromDirection(Direction.UpperLeft));
        var upperRight = map.GetValueOrDefault(aSpot.Location.FromDirection(Direction.UpperRight));
        var lowerLeft = map.GetValueOrDefault(aSpot.Location.FromDirection(Direction.LowerLeft));
        var lowerRight = map.GetValueOrDefault(aSpot.Location.FromDirection(Direction.LowerRight));
        if (upperLeft is null || upperRight is null || lowerLeft is null || lowerRight is null) return false;
        HashSet<char> mas1 = [upperLeft.Value.V!.Value, lowerRight.Value.V!.Value];
        HashSet<char> mas2 = [upperRight.Value.V!.Value, lowerLeft.Value.V!.Value];
        HashSet<char> chars = ['M', 'S'];
        if (chars.SetEquals(mas1) && mas1.SetEquals(mas2)) return true;
        return false;
    }

    private static bool HasLettersFromNeighbors(Dictionary<Location, Spot> map, Spot spot, string letters, Direction direction)
    {
        if (letters.Length == 0)
        {
            return true;
        }

        var checkLocation = spot.Location.FromDirection(direction);
        var nextSpot = map.GetValueOrDefault(checkLocation);

        if (nextSpot is not null && nextSpot.Value.V == letters[0])
        {
            return HasLettersFromNeighbors(map, nextSpot, letters[1..], direction);
        }

        return false;
    }

    [Fact]
    public void PartA()
    {
        using var reader = InputClient.GetFileStreamReader(2024, 4, "a");
        string? line;
        Location loc = Location.Origin;
        Dictionary<Location, Spot> map = [];

        // Read in input
        while ((line = reader.ReadLine()) != null)
        {
            foreach (var c in line)
            {
                var spot = CachedLocationGetter(map, loc);
                spot.Value.V = c;
                loc = loc.FromDirection(Direction.Right);
            }
            loc = loc.NewRow;
        }

        int countXmas = 0;
        foreach(var xSpot in GetLetter(map, 'X'))
        {
            foreach(var d in Enum.GetValues<Direction>())
            {
                if (HasLettersFromNeighbors(map, xSpot, "MAS", d)) countXmas++;
            }
        }

        Assert.Equal(2642, countXmas);
    }

    [Fact]
    public void PartB()
    {
        using var reader = InputClient.GetFileStreamReader(2024, 4, "a");
        string? line;
        Location loc = Location.Origin;
        Dictionary<Location, Spot> map = [];

        // Read in input
        while ((line = reader.ReadLine()) != null)
        {
            foreach (var c in line)
            {
                var spot = CachedLocationGetter(map, loc);
                spot.Value.V = c;
                loc = loc.FromDirection(Direction.Right);
            }
            loc = loc.NewRow;
        }

        int countXmas = 0;
        foreach(var aSpot in GetLetter(map, 'A'))
        {
            if (HasMas(map, aSpot)) countXmas++;
        }

        Assert.Equal(1974, countXmas);
    }
}

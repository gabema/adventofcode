using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Year2021;

// https://adventofcode.com/2021/day/7
public class Day7
{
    private readonly ITestOutputHelper _output;
    public Day7(ITestOutputHelper output)
    {
        _output = output;
    }

    private IEnumerable<ushort> ReadCrabPositions(string variant)
    {
        using var reader = InputClient.GetFileStreamReader(2021, 7, variant);
        return reader.ReadLine().Split(',').Select(v => ushort.Parse(v));
    }

    [Fact]
    public void PartA()
    {
        var crabs = ReadCrabPositions("").ToList();
        var maxMinCrabs = crabs.Aggregate((Max: ushort.MinValue, Min: ushort.MaxValue), (agg, v) => {
            if (agg.Max < v) agg.Max = v;
            if (agg.Min > v) agg.Min = v;
            return agg;
        });

        var minDistance = ulong.MaxValue;
        for (var test = maxMinCrabs.Min; test <= maxMinCrabs.Max; test++)
        {
            var currentDistance = crabs.Aggregate(0UL, (agg, v) => agg + (ulong)Math.Abs(v - test));
            if (currentDistance < minDistance)
            {
                minDistance = currentDistance;
            }
        }

        Assert.Equal(355989UL, minDistance);
    }

    private static ulong StepCost(ushort start, ushort destination)
    {
        var distance = (ushort)Math.Abs(destination - start);
        var total = 0UL;
        for (ushort i = 0; i <= distance; i++)
        {
            total += i;
        }
        return total;
    }

    [Fact]
    public void PartB()
    {
        var crabs = ReadCrabPositions("").ToList();
        var maxMinCrabs = crabs.Aggregate((Max: ushort.MinValue, Min: ushort.MaxValue), (agg, v) => {
            if (agg.Max < v) agg.Max = v;
            if (agg.Min > v) agg.Min = v;
            return agg;
        });

        var minDistance = ulong.MaxValue;
        for (var test = maxMinCrabs.Min; test <= maxMinCrabs.Max; test++)
        {
            var currentDistance = crabs.Aggregate(0UL, (agg, v) => agg + StepCost(v, test));
            if (currentDistance < minDistance)
            {
                minDistance = currentDistance;
            }
        }

        Assert.Equal(102245489UL, minDistance);
    }
}

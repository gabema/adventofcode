namespace AdventOfCode.Year2024;

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Xunit;
using Xunit.Abstractions;

public enum Ops
{
    Mul,
    Do,
    Dont
}

public class OpsScannerLexer : SimpleLexer2<Ops>
{
    private static readonly List<Tuple<Ops, string>> NewList = new List<Tuple<Ops, string>> {
            Tuple.Create(Ops.Mul, @"(mul)\((\d{1,3}),(\d{1,3})\)"),
            Tuple.Create(Ops.Do, @"(do)\(\)"),
            Tuple.Create(Ops.Dont, @"(don)\'t\(\)"),
        };

    public OpsScannerLexer() : base(NewList)
    { }
}

// https://adventofcode.com/2024/day/3
public class Day3(ITestOutputHelper output)
{
    [Fact]
    public void PartA()
    {
        using var reader = InputClient.GetFileStreamReader(2024, 3, "a");
        var inputParser = new Regex(@"mul\((\d{1,3}),(\d{1,3})\)");
        var safeReports = new List<int[]>();

        // Read in input
        var line = reader.ReadToEnd();
        var sum = 0ul;
        var muls = inputParser.Matches(line!);
        foreach(var mul in muls)
        {
            var match = (Match)mul;
            var val1 = match.Groups[1].Value;
            var val2 = match.Groups[2].Value;
            sum += ulong.Parse(val1) * ulong.Parse(val2);
        }

        // 29918137 - too low
        Assert.Equal(180233229ul, sum);
    }

    [Fact]
    public void PartB()
    {
        using var reader = InputClient.GetFileStreamReader(2024, 3, "a");
        var inputParser = new Regex(@"(mul)\((\d{1,3}),(\d{1,3})\)|(do\(\))|(don\'t\(\))");
        var safeReports = new List<int[]>();

        // Read in input
        var line = reader.ReadToEnd();
        var sum = 0ul;
        var enabled = true;
        var muls = inputParser.Matches(line!);
        foreach (var mul in muls)
        {
            var match = (Match)mul;
            var op = match.Groups[1].Success ? "mul" : match.Groups[4].Success ? "do" : "don";
            switch(op)
            {
                case "mul":
                    if (enabled)
                    {
                        var val1 = match.Groups[2].Value;
                        var val2 = match.Groups[3].Value;
                        sum += ulong.Parse(val1) * ulong.Parse(val2);
                    }
                    break;
                case "do":
                    enabled = true;
                    break;
                case "don":
                    enabled = false;
                    break;
            }
        }

        Assert.Equal(95411583ul, sum);
    }
}

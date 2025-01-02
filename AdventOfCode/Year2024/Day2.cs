namespace AdventOfCode.Year2024;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using Xunit;
using Xunit.Abstractions;

// https://adventofcode.com/2024/day/2
public class Day2(ITestOutputHelper output)
{

    IEnumerable<int[]> PossibleReports(int[] report)
    {
        yield return report;

        for (var i = 0; i < report.Length; i++)
        {
            yield return [.. report[0..i], .. report[(i + 1)..^0]];
        }
    }

    [Fact]
    public void TestPossibeReportGenerator()
    {
        var report = new int[] { 1, 2, 3 };
        var reports = PossibleReports(report).ToList();
        Assert.Equal([1, 2, 3], reports[0]);
        Assert.Equal([2, 3], reports[1]);
        Assert.Equal([1, 3], reports[2]);
        Assert.Equal([1, 2], reports[3]);

        report = [1, 2, 3, 4];
        reports = PossibleReports(report).ToList();
        Assert.Equal([1, 2, 3, 4], reports[0]);
        Assert.Equal([2, 3, 4], reports[1]);
        Assert.Equal([1, 3, 4], reports[2]);
        Assert.Equal([1, 2, 4], reports[3]);
        Assert.Equal([1, 2, 3], reports[4]);
    }

    [Fact]
    public void PartA()
    {
        using var reader = InputClient.GetFileStreamReader(2024, 2, "a");
        string line;
        var inputParser = new Regex(@"\d+");
        var safeReports = new List<int[]>();

        // Read in reports
        while ((line = reader.ReadLine()!) != null)
        {
            var data = inputParser.Matches(line);
            var report = data.Select(m => int.Parse(m.Value)).ToArray();
            if (IsSafe(report, output))
            {
                safeReports.Add(report);
            }
        }

        Assert.Equal(321, safeReports.Count);
    }

    [Fact]
    public void PartB()
    {
        using var reader = InputClient.GetFileStreamReader(2024, 2, "a");
        string line;
        var inputParser = new Regex(@"\d+");
        var safeReports = new List<int[]>();

        // Read in reports
        while ((line = reader.ReadLine()!) != null)
        {
            var data = inputParser.Matches(line);
            var report = data.Select(m => int.Parse(m.Value)).ToArray();
            var possibleReports = PossibleReports(report);
            foreach (var r in possibleReports)
            {
                if (IsSafe(r, output))
                {
                    safeReports.Add(r);
                    break;
                }
            }
        }

        // Not correct 365
        Assert.Equal(386, safeReports.Count);
    }

    private static bool IsSafe(int[] report, ITestOutputHelper output)
    {
        var safe = true;
        foreach(var stillSafe in IsSafeInc(report, output))
        {
            if (!stillSafe)
            {
                safe = false;
                break;
            }
        }

        if (safe) return true;
        safe = true;

        foreach(var stillSafe in IsSafeDec(report, output))
        {
            if (!stillSafe)
            {
                safe = false;
                break;
            }
        }

        return safe;
    }

    private static bool AbsDiffRange(int a, int b, int min, int max)
    {
        var diff = Math.Abs(a - b);
        return diff >= min && diff <= max;
    }

    private static IEnumerable<bool> IsSafeInc(int[] report, ITestOutputHelper output)
    {
        int[] reportSlice = [ ..report ];
        for(var i = 1; i < reportSlice.Length - 1; i++)
        {
            if (reportSlice[i] > reportSlice[i-1]
                && reportSlice[i] < reportSlice[i+1]
                && AbsDiffRange(reportSlice[i], reportSlice[i-1], 1, 3)
                && AbsDiffRange(reportSlice[i], reportSlice[i+1], 1, 3))
            {
                yield return true;
            }
            else
            {
                yield return false;
                break;
            }
        }
    }

    private static IEnumerable<bool> IsSafeDec(int[] report, ITestOutputHelper output)
    {
        for(var i = 1; i < report.Length - 1; i++)
        {
            if (report[i] < report[i-1]
                && report[i] > report[i+1]
                && AbsDiffRange(report[i], report[i-1], 1, 3)
                && AbsDiffRange(report[i], report[i+1], 1, 3))
            {
                yield return true;
            }
            else
            {
                yield return false;
                break;
            }
        }
    }
}

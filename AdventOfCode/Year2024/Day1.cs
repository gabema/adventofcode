namespace AdventOfCode.Year2024;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;

// https://adventofcode.com/2024/day/1
public class Day1
{
    [Fact]
    public void PartA()
    {
        using var reader = InputClient.GetFileStreamReader(2024, 1, "a");
        string line;
        var inputParser = new Regex(@"(\d+)\s+(\d+)");
        var listA = new List<int>(1000);
        var listB = new List<int>(1000);

        // Read in list of numbers
        while ((line = reader.ReadLine()!) != null)
        {
            var data = inputParser.Match(line);
            if (data.Success) {
                listA.Add(int.Parse(data.Groups[1].Value));
                listB.Add(int.Parse(data.Groups[2].Value));
            }
        }

        listA = listA.OrderBy(x => x).ToList();
        listB = listB.OrderBy(x => x).ToList();
        var diffOfLists = new List<int>();
        var sumOfDiffs = 0;

        // Calculate differences
        for (var i = 0; i < listA.Count; i++)
        {
            var diff = Math.Abs(listA[i] - listB[i]);
            sumOfDiffs += diff;
            diffOfLists.Add(diff);
        }

        Assert.Equal(2264607, sumOfDiffs);
    }

    [Fact]
    public void PartB()
    {
        using var reader = InputClient.GetFileStreamReader(2024, 1, "a");
        string line;
        var inputParser = new Regex(@"(\d+)\s+(\d+)");
        var listA = new List<int>();
        var listB = new List<int>();

        // Read in list of numbers
        while ((line = reader.ReadLine()!) != null)
        {
            var data = inputParser.Match(line);
            if (data.Success) {
                var valA = int.Parse(data.Groups[1].Value); 
                var valB = int.Parse(data.Groups[2].Value);
                listA.Add(valA);
                listB.Add(valB);                
            }
        }

        var simIndex = listA.ToHashSet().Select(n => (n, 0)).ToDictionary();

        foreach(var item in listB){
            if (simIndex.ContainsKey(item)) {
                simIndex[item] = simIndex[item] + 1;
            }
        }

        var simList = listA.Select(n => n * simIndex[n]);
        var totalAmount = simList.Sum();

        Assert.Equal(19457120, totalAmount);
    }
}

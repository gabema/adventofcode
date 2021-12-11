using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Year2021;

// https://adventofcode.com/2021/day/2
public class Day3
{
    private ushort[][] ReadEntries(string variant)
    {
        var lineList = InputClient.GetRegularExpressionRows(2021, 3, variant, @"[01]+").Select(a => a[0].Value).ToList();
        ushort[][] indexCounts = new ushort[lineList.Count][];
        for(var i = 0; i<lineList.Count; i++)
        {
            for (var ci = 0; ci < lineList[i].Length; ci++)
            {
                if (indexCounts[i] == null)
                {
                    indexCounts[i] = new ushort[lineList[i].Length];
                }
                indexCounts[i][ci] = (ushort)(lineList[i][ci] == '1' ? 1 : 0);
            }
        }
        return indexCounts;
    }

    private static (int Ones, int Zeros)[] ReadContents(ushort[][] entries)
    {
        (int Ones, int Zeros)[] indexCounts = null;
        foreach (var line in entries)
        {
            var len = line.Length;
            if (indexCounts == null)
            {
                indexCounts = new (int Ones, int Zeros)[len];
            }
            for (int i = 0; i < len; i++)
            {
                if (line[i] == 1) indexCounts[i].Ones++;
                else if (line[i] == 0) indexCounts[i].Zeros++;
            }
        }
        return indexCounts;
    }

    private (int Ones, int Zeros)[] ReadContents(string variant)
    {
        var lineReader = InputClient.GetRegularExpressionRows(2021, 3, variant, @"[01]+").Select(a => a[0].Value);
        (int Ones, int Zeros)[] indexCounts = null;
        foreach (var line in lineReader)
        {
            if (indexCounts == null)
            {
                indexCounts = new (int Ones, int Zeros)[line.Length];
            }

            var len = line.Length;
            for (int i = 0; i < len; i++)
            {
                if (line[i] == '1') indexCounts[i].Ones++;
                else if (line[i] == '0') indexCounts[i].Zeros++;
            }
        }
        return indexCounts;
    }
    
    [Fact]
    public void PartA()
    {
        (int Ones, int Zeros)[] indexCounts = ReadContents("");

        int gammaRate = 0;
        int epsilonRate = 0;

        for (var i = 0; i< indexCounts.Length; i++)
        {
            gammaRate = gammaRate << 1;
            epsilonRate = epsilonRate << 1;

            if (indexCounts[i].Ones > indexCounts[i].Zeros)
            {
                gammaRate |= 1;
            }
            else
            {
                epsilonRate |= 1;
            }
        }

        Assert.Equal(2972336, gammaRate * epsilonRate);
    }

    private static (int Ones, int Zeros)[] BitAnalysis(int i, List<ushort[]> entries, (int Ones, int Zeros)[] indexCounts, Func<int, int, int> comparator)
    {
        if (comparator(indexCounts[i].Ones, indexCounts[i].Zeros) > 0)
        {
            for (var j = 0; j < entries.Count; j++)
            {
                if (entries[j][i] == 0)
                {
                    entries.RemoveAt(j);
                    indexCounts = ReadContents(entries.ToArray());
                    j--;
                }
            }
        }
        else
        {
            for (var j = 0; j < entries.Count; j++)
            {
                if (entries[j][i] == 1)
                {
                    entries.RemoveAt(j);
                    indexCounts = ReadContents(entries.ToArray());
                    j--;
                }
            }
        }

        return indexCounts;
    }

    [Fact]
    public void PartB()
    {
        var entries = ReadEntries("");
        var indexCountsO2 = ReadContents(entries);
        var indexCountsCO2 = new (int Ones, int Zeros)[indexCountsO2.Length];
        Array.Copy(indexCountsO2, indexCountsCO2, indexCountsO2.Length);

        var co2ScrubberRating = new List<ushort[]>(entries);
        var o2GeneratorRating = new List<ushort[]>(entries);

        for (var i = 0; o2GeneratorRating.Count > 1 || co2ScrubberRating.Count > 1; i++)
        {
            if (o2GeneratorRating.Count > 1)
            {
                indexCountsO2 = BitAnalysis(i, o2GeneratorRating, indexCountsO2, (ones, zeros) => (ones >= zeros ? 1 : 0));
            }

            if (co2ScrubberRating.Count > 1)
            {
                indexCountsCO2 = BitAnalysis(i, co2ScrubberRating, indexCountsCO2, (ones, zeros) => (ones < zeros ? 1 : 0));
            }
        }

        var co2ScrubberRatingVal = GenerateValue(co2ScrubberRating[0]);
        var o2GeneratorRatingVal = GenerateValue(o2GeneratorRating[0]);

        Assert.Equal(3368358, co2ScrubberRatingVal * o2GeneratorRatingVal);
    }

    private static int GenerateValue(ushort[] list)
    {
        int val = 0;
        for (var i = 0; i < list.Length; i++)
        {
            val = val << 1;
            val = val + (int)list[i];
        }
        return val;
    }
}

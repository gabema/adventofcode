using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Year2023;

// https://adventofcode.com/2023/day/1
public class Day1
{
    private static readonly List<string> VALUES = new List<string> {
        "zero",
        "one",
        "two",
        "three",
        "four",
        "five",
        "six",
        "seven",
        "eight",
        "nine"
    };

    private static readonly List<string> NUM_VALUES = new List<string> { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };

    [Fact]
    public void PartA()
    {
        using var reader = InputClient.GetFileStreamReader(2023, 1, "a");
        string line;

        Func<char, bool> firstNum = c => c >= '0' && c <= '9';

        var sum = 0;
        while ((line = reader.ReadLine()) != null)
        {
            var v = $"{line.First(firstNum)}{line.Reverse().First(firstNum)}";
            sum += int.Parse(v);
        }

        Assert.Equal(54605, sum);
    }

    [Fact]
    public void PartB()
    {
        using var reader = InputClient.GetFileStreamReader(2023, 1, "a");
        string line;

        var sum = 0;
        while ((line = reader.ReadLine()) != null)
        {
            var len = line.Length;

            int index = 0;
            int firstValue = -1;
            while (firstValue == -1)
            {
                for (var i = 0; i < 10; i++)
                {
                    if (line.Substring(index).StartsWith(NUM_VALUES[i]))
                    {
                        firstValue = i;
                        break;
                    }
                }
                if (firstValue == -1)
                {
                    for (var i = 0; i < 10; i++)
                    {
                        if (line.Substring(index).StartsWith(VALUES[i]))
                        {
                            firstValue = i;
                            break;
                        }
                    }
                }
                index++;
            }

            index = line.Length - 1;
            int secondValue = -1;
            while (secondValue == -1)
            {
                for (var i = 0; i < 10; i++)
                {
                    if (line.Substring(index).StartsWith(NUM_VALUES[i]))
                    {
                        secondValue = i;
                        break;
                    }
                }
                if (secondValue == -1 && line.Length - index >= 3)
                {
                    for (var i = 0; i < 10; i++)
                    {
                        if (line.Substring(index).StartsWith(VALUES[i]))
                        {
                            secondValue = i;
                            break;
                        }
                    }
                }
                index--;
            }

            var value = int.Parse(firstValue.ToString() + secondValue.ToString());
            sum += value;
        }

        Assert.Equal(55429, sum);
    }
}

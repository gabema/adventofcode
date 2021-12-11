using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Year2021;

// https://adventofcode.com/2021/day/1
public class Day1
{
    private readonly ITestOutputHelper output;

    public Day1(ITestOutputHelper output)
    {
        this.output = output;
    }

    [Fact]
    public void PartA()
    {
        var inputs = InputClient.GetIntInput(2021, 1);
        var inputArray = inputs.ToList();
        var numIncreasing = 0;
        for (var i = 1; i < inputArray.Count; i++)
        {
            if (inputArray[i - 1] < inputArray[i])
            {
                numIncreasing++;
            }
        }
        Assert.Equal(1502, numIncreasing);
    }

    [Fact]
    public void PartB()
    {
        var inputs = InputClient.GetIntInput(2021, 1, "");
        var inputArray = inputs.ToList();
        var numIncreasing = 0;

        for (var i = 3; i < inputArray.Count; i++)
        {
            var left = inputArray[i - 3] + inputArray[i - 2] + inputArray[i - 1];
            var right = inputArray[i - 2] + inputArray[i - 1] + inputArray[i];
            if (left < right)
            {
                numIncreasing++;
            }
        }

        Assert.Equal(1538, numIncreasing);
    }
}

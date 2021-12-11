using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Year2021;

// https://adventofcode.com/2021/day/2
public class Day2
{
    class Position
    {
        public int Horizontal { get; set; }
        public int Depth { get; set; }
    }

    class PositionWithAim
    {
        public int Horizontal { get; set; }
        public int Depth { get; set; }
        public int Aim { get; set; }
    }

    private readonly ITestOutputHelper output;

    public Day2(ITestOutputHelper output)
    {
        this.output = output;
    }

    [Fact]
    public void PartA()
    {
        var rows = InputClient.GetRegularExpressionRows(2021, 2, "", @"(\w+) (\d+)");
        var position = new Position();
        foreach (var row in rows)
        {
            var direction = row[1].Value;
            var distance = int.Parse(row[2].Value);
            switch(direction)
            {
                case "up":
                    {
                        position.Depth -= distance;
                        break;
                    }
                case "down":
                    {
                        position.Depth += distance;
                        break;
                    }
                case "forward":
                    {
                        position.Horizontal += distance;
                        break;
                    }
            }
        }

        Assert.Equal(1813801, position.Horizontal * position.Depth);
    }

    [Fact]
    public void PartB()
    {
        var rows = InputClient.GetRegularExpressionRows(2021, 2, "", @"(\w+) (\d+)");
        var position = new PositionWithAim();
        foreach (var row in rows)
        {
            var direction = row[1].Value;
            var distance = int.Parse(row[2].Value);
            switch (direction)
            {
                case "up":
                    {
                        position.Aim -= distance;
                        break;
                    }
                case "down":
                    {
                        position.Aim += distance;
                        break;
                    }
                case "forward":
                    {
                        position.Horizontal += distance;
                        position.Depth += (position.Aim * distance);
                        break;
                    }
            }
        }

        Assert.Equal(1960569556, position.Horizontal * position.Depth);
    }
}

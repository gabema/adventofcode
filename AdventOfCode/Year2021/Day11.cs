using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Year2021;

// https://adventofcode.com/2021/day/2
public class Day11
{
    private readonly ITestOutputHelper _output;
    public Day11(ITestOutputHelper output)
    {
        _output = output;
    }

    Octopus[][] ReadGrid(string variant)
    {
        using var reader = InputClient.GetFileStreamReader(2021, 11, variant);
        string line;

        var rows = new List<Octopus[]>();

        while((line =reader.ReadLine()) != null)
        {
            rows.Add(line.Select(c => new Octopus(int.Parse(c.ToString()))).ToArray());
        }
        return rows.ToArray();
    }

    public class Octopus
    {
        public int EnergyStrength { get; private set; }
        public bool HasFlashed { get; private set; }

        public Octopus(int energy)
        {
            EnergyStrength = energy;
        }

        public void ResetFlashed()
        {
            if (HasFlashed)
            {
                HasFlashed = false;
                EnergyStrength = 0;
            }
        }

        public void Increment(Octopus[][] grid, int x, int y, int width, int height)
        {
            if (HasFlashed) return;

            EnergyStrength++;
            if (EnergyStrength > 9)
            {
                HasFlashed = true;
                if (x - 1 >= 0) grid[y][x - 1].Increment(grid, x-1, y, width, height);
                if (x - 1 >= 0 && y - 1 >= 0) grid[y - 1][x - 1].Increment(grid, x - 1, y - 1, width, height);
                if (x - 1 >= 0 && y + 1 < height) grid[y + 1][x - 1].Increment(grid, x - 1, y + 1, width, height);

                if (y - 1 >= 0) grid[y - 1][x].Increment(grid, x, y - 1, width, height);
                if (y + 1 < height) grid[y+1][x].Increment(grid, x, y + 1, width, height);

                if (x + 1 < width) grid[y][x + 1].Increment(grid, x + 1, y, width, height);
                if (x + 1 < width && y - 1 >= 0) grid[y-1][x + 1].Increment(grid, x + 1, y - 1, width, height);
                if (x + 1 < width && y + 1 < height) grid[y+1][x + 1].Increment(grid, x + 1, y + 1, width, height);
            }
        }

        public override string ToString()
        {
            return $"{EnergyStrength}";
        }
    }

    [Fact]
    public void PartA()
    {
        var octoGrid = ReadGrid("");
        var width = octoGrid[0].Length;
        var height = octoGrid.Length;
        int countFlashes = 0;

        for (int step = 1; step <= 100; step ++)
        {
            // Increment
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    octoGrid[y][x].Increment(octoGrid, x, y, width, height);
                }
            }

            // Reset all above 9 to 
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (octoGrid[y][x].HasFlashed) countFlashes++;
                    octoGrid[y][x].ResetFlashed();
                }
            }
        }

        Assert.Equal(1705, countFlashes);
    }

    [Fact]
    public void PartB()
    {
        var octoGrid = ReadGrid("");
        var width = octoGrid[0].Length;
        var height = octoGrid.Length;
        int synchronizedStep = 0;

        for (int step = 1; synchronizedStep == 0; step++)
        {
            int countFlashes = 0;

            // Increment
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    octoGrid[y][x].Increment(octoGrid, x, y, width, height);
                }
            }

            // Reset all above 9 to 
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (octoGrid[y][x].HasFlashed) countFlashes++;
                    octoGrid[y][x].ResetFlashed();
                }
            }

            if (countFlashes == width * height)
            {
                synchronizedStep = step;
            }
        }

        Assert.Equal(265, synchronizedStep);
    }
}

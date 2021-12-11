using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Year2021;

// https://adventofcode.com/2021/day/6
public class Day6
{
    private readonly ITestOutputHelper _output;
    public Day6(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void PartA()
    {
        var dayIndex = 0;
        var fishSpawns = GetFishArray("");

        ulong count = 0;
        for (var day = 0; day <= 80; day++)
        {
            count = fishSpawns.Aggregate(0UL, (sum, value) => sum + value);
            dayIndex = day % 9;
            // _output.WriteLine($"Day {day} total={count}, 0={numFishSpawns[dayIndex]}, 1={numFishSpawns[(dayIndex + 1) % 9]}, 2={numFishSpawns[(dayIndex + 2) % 9]}, 3={numFishSpawns[(dayIndex + 3) % 9]}, 4={numFishSpawns[(dayIndex + 4) % 9]}, 5={numFishSpawns[(dayIndex + 5) % 9]}, 6={numFishSpawns[(dayIndex + 6) % 9]}, 7={numFishSpawns[(dayIndex + 7) % 9]}, 8={numFishSpawns[(dayIndex + 8) % 9]}");
            var spawnedFish = fishSpawns[dayIndex]; // This number of fish will spawn again in 8 days
            fishSpawns[dayIndex] = 0;
            fishSpawns[(dayIndex + 7) % 9] = fishSpawns[(dayIndex + 7) % 9] + spawnedFish;
            fishSpawns[(dayIndex + 9) % 9] = fishSpawns[(dayIndex + 9) % 9] + spawnedFish;
        }
        Assert.Equal(345387UL, count);
    }

    private ulong[] GetFishArray(string v)
    {
        ulong[] pond = new ulong[9];
        using var reader = InputClient.GetFileStreamReader(2021, 6, v);
        return reader.ReadLine().Split(',').Aggregate(new ulong[9], (agg, v) => {
            var d = int.Parse(v);
            agg[d]++;
            return agg;
        });
    }

    [Fact]
    public void PartB()
    {
        var dayIndex = 0;
        var fishSpawns = GetFishArray("");

        ulong count = 0;
        for (var day = 0; day <= 256; day++)
        {
            count = fishSpawns.Aggregate(0UL, (sum, value) => sum + value);
            dayIndex = day % 9;
            // _output.WriteLine($"Day {day} total={count}, 0={numFishSpawns[dayIndex]}, 1={numFishSpawns[(dayIndex + 1) % 9]}, 2={numFishSpawns[(dayIndex + 2) % 9]}, 3={numFishSpawns[(dayIndex + 3) % 9]}, 4={numFishSpawns[(dayIndex + 4) % 9]}, 5={numFishSpawns[(dayIndex + 5) % 9]}, 6={numFishSpawns[(dayIndex + 6) % 9]}, 7={numFishSpawns[(dayIndex + 7) % 9]}, 8={numFishSpawns[(dayIndex + 8) % 9]}");
            var spawnedFish = fishSpawns[dayIndex]; // This number of fish will spawn again in 8 days
            fishSpawns[dayIndex] = 0;
            fishSpawns[(dayIndex + 7) % 9] = fishSpawns[(dayIndex + 7) % 9] + spawnedFish;
            fishSpawns[(dayIndex + 9) % 9] = fishSpawns[(dayIndex + 9) % 9] + spawnedFish;
        }
        Assert.Equal(1574445493136UL, count);
    }
}

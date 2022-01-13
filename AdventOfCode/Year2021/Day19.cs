using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Year2021;

// https://adventofcode.com/2021/day/19
public class Day19
{
    private readonly ITestOutputHelper _output;

    public record Beacon (int X, int Y, int Z);

    public record Scanner (int Number, Beacon[] Points);

    public record ScannerConfigurations(int Number, Beacon[][] OrderedPoints);

    public enum BeaconScannerItems
    {
        Scanner,
        Beacon,
        Terminator
    }

    public class BeaconScannerLexer : SimpleLexer2<BeaconScannerItems>
    {
        private static readonly List<Tuple<BeaconScannerItems, string>> NewList = new List<Tuple<BeaconScannerItems, string>> {
            Tuple.Create(BeaconScannerItems.Scanner, @"^--- scanner (\d+) ---"),
            Tuple.Create(BeaconScannerItems.Beacon, @"^(-?\d+),(-?\d+),(-?\d+)"),
            Tuple.Create(BeaconScannerItems.Terminator, @""),
        };

        public BeaconScannerLexer() : base(NewList)
        { }
    }

    private IEnumerable<Scanner> GetScanners(string variant)
    {
        using var reader = InputClient.GetFileStreamReader(2021, 19, variant);
        string input;
        var lexer = new BeaconScannerLexer();
        var beacons = new List<Beacon>();
        var scannerNumber = 0;
        while ((input = reader.ReadLine()) != null)
        {
            if (input.Trim() == "")
            {
                yield return new Scanner(scannerNumber, beacons.ToArray());
                beacons.Clear();
            }
            else
            {
                IEnumerable<Token2<BeaconScannerItems>> tokens = lexer.Tokenize(input);
                foreach (var item in tokens)
                {
                    switch (item.TokenType)
                    {
                        case BeaconScannerItems.Scanner:
                            scannerNumber = int.Parse(item.Value.Groups[1].Value);
                            break;
                        case BeaconScannerItems.Beacon:
                            beacons.Add(new Beacon(
                                int.Parse(item.Value.Groups[1].Value),
                                int.Parse(item.Value.Groups[2].Value),
                                int.Parse(item.Value.Groups[3].Value)
                            ));
                            break;
                    }
                }
            }
        }
    }

    public Day19(ITestOutputHelper output)
    {
        _output = output;
    }


    [Fact]
    public void PartA()
    {
        var scanners = GetScanners("Sample");
        var scannerConfigs = scanners.Select(s => new ScannerConfigurations(s.Number, PossibleConfigurations(s.Points)));

        Assert.Equal(4, scanners.Count());
    }

    private Beacon[][] PossibleConfigurations(Beacon[] points)
    {
        List<Beacon[]> configs = new List<Beacon[]>();

        for(var x=-1; x<2; x+=2)
        {
            for (var y=-1; y<2; y+=2)
            {
                for (var z = -1; z < 2; z+=2)
                {
                    configs.Add(
                        points.Select(p => new Beacon(p.X * x, p.Y * y, p.Z * z))
                            .OrderBy(p => p.X)
                            .ThenBy(p => p.Y)
                            .ThenBy(p => p.Z).ToArray()
                    );
                }
            }
        }
        return configs.ToArray();
    }

    [Fact]
    public void PartB()
    {
    }
}

using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Year2021;

// https://adventofcode.com/2021/day/10
public class Day10
{
    enum MarkerType {
        Curly,
        Paren,
        Square,
        Carrot
    }

    record Marker(MarkerType marker, bool isOpen);

    Dictionary<char, Marker> Markers = new Dictionary<char, Marker>() {
        { '{', new Marker(MarkerType.Curly, true) },
        { '}', new Marker(MarkerType.Curly, false) },
        { '(', new Marker(MarkerType.Paren, true) },
        { ')', new Marker(MarkerType.Paren, false) },
        { '[', new Marker(MarkerType.Square, true) },
        { ']', new Marker(MarkerType.Square, false) },
        { '<', new Marker(MarkerType.Carrot, true) },
        { '>', new Marker(MarkerType.Carrot, false) },
    };

    Dictionary<MarkerType, long> MarkerValue = new Dictionary<MarkerType, long>() {
        {MarkerType.Paren, 3L },
        {MarkerType.Square, 57L },
        {MarkerType.Curly, 1197L },
        {MarkerType.Carrot, 25137L },
    };

    Dictionary<MarkerType, long> MarkerCompleterValue = new Dictionary<MarkerType, long>() {
        {MarkerType.Paren, 1L },
        {MarkerType.Square, 2L },
        {MarkerType.Curly, 3L },
        {MarkerType.Carrot, 4L },
    };

    private IEnumerable<IEnumerable<Marker>> ReadLines(string variant)
    {
        using var reader = InputClient.GetFileStreamReader(2021, 10, variant);
        string line;
        while ((line = reader.ReadLine()) != null)
        {
            yield return line.Select(c => Markers[c]);
        }
    }

    private readonly ITestOutputHelper _output;
    public Day10(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void PartA()
    {
        var markerScore = 0L;
        var lines = ReadLines("");
        var markers = new Stack<Marker>();
        foreach (var line in lines)
        {
            foreach(var marker in line)
            {
                if (marker.isOpen)
                {
                    markers.Push(marker);
                }
                else
                {
                    var expectedMarker = markers.Pop().marker;
                    if (marker.marker != expectedMarker )
                    {
                        markerScore += MarkerValue[marker.marker];
                        break;
                    }
                }
            }
            markers.Clear();
        }
        Assert.Equal(370407L, markerScore);
    }

    [Fact]
    public void PartB()
    {
        var lines = ReadLines("");
        var markers = new Stack<Marker>();
        var scores = new List<long>();

        foreach (var line in lines)
        {
            foreach (var marker in line)
            {
                if (marker.isOpen)
                {
                    markers.Push(marker);
                }
                else
                {
                    var expectedMarker = markers.Pop().marker;
                    if (marker.marker != expectedMarker)
                    {
                        markers.Clear();
                        break;
                    }
                }
            }
            if (markers.Count > 0)
            {
                // Incomplete
                var score = 0L;
                while(markers.Count > 0)
                {
                    score = score * 5L + MarkerCompleterValue[markers.Pop().marker];
                }
                scores.Add(score);
            }
        }
        var middle = (scores.Count / 2) + 1;
        Assert.Equal(3249889609L, scores.OrderBy(x => x).Take(middle).Last());
    }
}

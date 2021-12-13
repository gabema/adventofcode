using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Year2020;

public class TileImageBuilder
{
    private int _rowAddIndex;
    private int _size;
    private ushort top;
    private ushort left;
    private ushort right;
    private ushort bottom;
    private int _tileNumber;

    public bool Complete { get => _size == _rowAddIndex && _rowAddIndex != 0;  }

    public TileImageBuilder AddRow(string row)
    {
        _size = row.Length;

        // left and right (for each row)
        left = (ushort)(left | ((row[0] == '#' ? 1 : 0) << _rowAddIndex));
        right = (ushort)(right | ((row[_size - 1] == '#' ? 1 : 0) << _rowAddIndex));

        if (_rowAddIndex == 0)
        {
            for (var j = 0; j < _size; j++)
            {
                top = (ushort)(top | ((row[j] == '#' ? 1 : 0) << j));
            }
        }
        else if (_rowAddIndex == _size - 1)
        {
            for (var j = 0; j < _size; j++)
            {
                bottom = (ushort)(bottom | ((row[j] == '#' ? 1 : 0) << j));
            }
        }

        _rowAddIndex++;

        return this;
    }

    public TileImageBuilder SetNumber(int tileNumber)
    {
        _tileNumber = tileNumber;
        return this;
    }

    public TileImage Build()
    {
        if (!Complete) throw new InvalidOperationException("Have not read in an entire tile yet...");

        return new TileImage
        {
            TileNumber = _tileNumber,
            Top = top,
            Bottom = bottom,
            Left = left,
            Right = right,
            Size = _size
        };
    }

    public void Reset()
    {
        _rowAddIndex = 0;
    }
}

public class TileImage : IEquatable<TileImage>
{
    public int TileNumber { get; init; }
    public ushort Top { get; set; }
    public ushort Left { get; set; }
    public ushort Right { get; set; }
    public ushort Bottom { get; set; }
    public int Size { get; init; }

    public bool Equals(TileImage? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;

        return TileNumber == other.TileNumber
            && Top == other.Top
            && Left == other.Left
            && Right == other.Right
            && Bottom == other.Bottom
            && Size == other.Size;
    }

    public override bool Equals(object obj)
    {
        return Equals(obj as TileImage);
    }

    public override int GetHashCode() => HashCode.Combine(Top, Left, Right, Bottom);

    internal void FlipVertical()
    {
        var holdValue = FlipValue(Top, Size);
        Top = FlipValue(Bottom, Size);
        Bottom = holdValue;
        holdValue = Left;
        Left = Right;
        Right = holdValue;
    }

    private static ushort FlipValue(ushort oldValue, int size)
    {
        ushort newValue = 0;
        for (var i=0; i<size; i++)
        {
            var newBit = (ushort)(1 & oldValue);
            newValue |= (ushort)(newBit << (size - i));
            oldValue = (ushort)(oldValue >> 1);
        }
        return newValue;
    }

    public override string ToString()
    {
        return $"Top = {Top:N3}";
    }
}

// https://adventofcode.com/2020/day/20
public class Day20
{
    private readonly ITestOutputHelper _output;
    public Day20(ITestOutputHelper output)
    {
        _output = output;
    }

    private static void AddTileImageToSetDictionary(Dictionary<ushort, ISet<TileImage>>  dictSet, TileImage tile, ushort value)
    {
        if (!dictSet.ContainsKey(value)) dictSet[value] = new HashSet<TileImage>();

        dictSet[value].Add(tile);
    }

    [Fact]
    public void PartA()
    {
        var tiles = GetTiles("Sample").ToList();
        var imageWidth = (int) Math.Sqrt(tiles.Count);
        Dictionary<ushort, ISet<TileImage>> tilesBySideValues = new Dictionary<ushort, ISet<TileImage>>();
        foreach(var tile in tiles)
        {
            Day20.AddTileImageToSetDictionary(tilesBySideValues, tile, tile.Top);
            Day20.AddTileImageToSetDictionary(tilesBySideValues, tile, tile.Bottom);
            Day20.AddTileImageToSetDictionary(tilesBySideValues, tile, tile.Left);
            Day20.AddTileImageToSetDictionary(tilesBySideValues, tile, tile.Right);
        }

        var tileImage = tilesBySideValues.Values.FirstOrDefault(values => values.Count == 1)?.First();
        //while (tileImage != default) // TODO: Do something here
        {
            tilesBySideValues[tileImage.Top].Remove(tileImage);
            tilesBySideValues[tileImage.Bottom].Remove(tileImage);
            tilesBySideValues[tileImage.Left].Remove(tileImage);
            tilesBySideValues[tileImage.Right].Remove(tileImage);

            _output.WriteLine(tileImage.ToString());
            tileImage.FlipVertical();
            _output.WriteLine(tileImage.ToString());

            Day20.AddTileImageToSetDictionary(tilesBySideValues, tileImage, tileImage.Top);
            Day20.AddTileImageToSetDictionary(tilesBySideValues, tileImage, tileImage.Bottom);
            Day20.AddTileImageToSetDictionary(tilesBySideValues, tileImage, tileImage.Left);
            Day20.AddTileImageToSetDictionary(tilesBySideValues, tileImage, tileImage.Right);

            tileImage = tilesBySideValues.Values.FirstOrDefault(values => values.Count == 1)?.First();
        }

        Assert.Equal(10, tilesBySideValues.Count);
    }

    [Fact]
    public void TestProcessInput()
    {
        Assert.Equal(9, GetTiles("a").Count());
    }

    private IEnumerable<TileImage> GetTiles(string ext)
    {
        TileImageBuilder builder = new TileImageBuilder();
        var rows = InputClient.GetRegularExpressionRows(2020, 20, ext, @"((?<row>[\.#]+)|(Tile (?<tileNum>\d+):))");
        foreach (var group in rows)
        {
            if (group["row"].Success)
            {
                builder.AddRow(group["row"].Value);
            }
            else if (group["tileNum"].Success)
            {
                builder.SetNumber(int.Parse(group["tileNum"].Value));
            }

            if (builder.Complete)
            {
                yield return builder.Build();
                builder.Reset();
            }
        }
    }
}

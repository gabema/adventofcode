using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

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
            Right = right
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
    public ushort Top { get; init; }
    public ushort Left { get; init; }
    public ushort Right { get; init; }
    public ushort Bottom { get; init; }

    public bool Equals(TileImage? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;

        return TileNumber == other.TileNumber
            && Top == other.Top
            && Left == other.Left
            && Right == other.Right
            && Bottom == other.Bottom;
    }

    public override bool Equals(object obj)
    {
        return Equals(obj as TileImage);
    }

    public override int GetHashCode() => HashCode.Combine(Top, Left, Right, Bottom);
}

// https://adventofcode.com/2020/day/20
public class Day20
{
    private static void AddTileImageToSetDictionary(Dictionary<ushort, ISet<TileImage>>  dictSet, TileImage tile, ushort value)
    {
        if (!dictSet.ContainsKey(value)) dictSet[value] = new HashSet<TileImage>();

        dictSet[value].Add(tile);
    }

    [Fact]
    public void PartA()
    {
        var tiles = GetTiles("a").ToList();
        var imageWidth = (int) Math.Sqrt(tiles.Count);
        Dictionary<ushort, ISet<TileImage>> tilesBySideValues = new Dictionary<ushort, ISet<TileImage>>();
        foreach(var tile in tiles)
        {
            Day20.AddTileImageToSetDictionary(tilesBySideValues, tile, tile.Top);
            Day20.AddTileImageToSetDictionary(tilesBySideValues, tile, tile.Bottom);
            Day20.AddTileImageToSetDictionary(tilesBySideValues, tile, tile.Left);
            Day20.AddTileImageToSetDictionary(tilesBySideValues, tile, tile.Right);
        }

        bool hasOneMatch = true;
        while(hasOneMatch)
        {
            foreach(var tileSet in tilesBySideValues.Values)
            {
                if (tileSet.Count == 1)
                {
                    var tileImage = tileSet.First();
                    tilesBySideValues[tileImage.Top].Remove(tileImage);
                    tilesBySideValues[tileImage.Bottom].Remove(tileImage);
                    tilesBySideValues[tileImage.Left].Remove(tileImage);
                    tilesBySideValues[tileImage.Right].Remove(tileImage);

                }
            }
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

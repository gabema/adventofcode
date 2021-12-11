using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Year2021;

class BingoSquare
{
    public int Number { get; init; }
    public bool Checked { get; set; }
}

class BingoBoard
{
    private List<BingoSquare> Squares { get; }

    public BingoBoard(IEnumerable<BingoSquare> squares)
    {
        Squares = new List<BingoSquare>(squares);
    }

    public bool ApplyValue(int i)
    {
        for(var tileIndex = 0; tileIndex < Squares.Count; tileIndex++)
        {
            var tile = Squares[tileIndex];
            if (tile.Number == i)
            {
                tile.Checked = true;
                return CheckForBingo(tileIndex);
            }
        }
        return false;
    }

    public int UnmarkedTileSum() => Squares.Aggregate(0, (sum, tile) => tile.Checked ? sum: sum + tile.Number);

    public bool CheckForBingo(int tileIndex)
    {
        var width = (int) Math.Sqrt(Squares.Count);
        var row = tileIndex / width;
        var col = tileIndex % width;

        var rowOffset = row * width;
        var foundBingo = true;

        // Check Row
        for (var checkColumn = 0; checkColumn < width && foundBingo; checkColumn++)
        {
            var index = rowOffset + checkColumn;
            if (!Squares[index].Checked)
            {
                foundBingo = false;
            }
        }

        if (foundBingo) return true;
        foundBingo = true;

        // Check Column
        for (var checkRow = 0; checkRow < width && foundBingo; checkRow++)
        {
            var checkRowOffset = checkRow * width;
            var index = checkRowOffset + col;
            if (!Squares[index].Checked)
            {
                foundBingo = false;
            }
        }

        return foundBingo;
    }
}

class BingoBoardBuilder
{
    private IEnumerable<BingoSquare> squares = Enumerable.Empty<BingoSquare>();

    public bool IsComplete { get => _rowCount > 0 && (_rowCount * _rowCount) == squares.Count(); }

    private int _rowCount = 0;

    public BingoBoard Build()
    {
        return new BingoBoard(squares);
    }

    public void AddRow(IEnumerable<int> enumerable)
    {
        if (_rowCount == default)
        {
            _rowCount = enumerable.Count();
        }
        squares =  squares.Concat(enumerable.Select(i => new BingoSquare { Number = i }));
    }

    public void Reset()
    {
        squares = Enumerable.Empty<BingoSquare>();
    }
}

// https://adventofcode.com/2021/day/2
public class Day4
{
    private readonly ITestOutputHelper _output;

    public Day4(ITestOutputHelper output)
    {
        _output = output;
    }

    private IEnumerable<BingoBoard> GetBoards(string variant, Queue<int> inputs)
    {
        using var reader = InputClient.GetFileStreamReader(2021, 4, variant);
        var builder = new BingoBoardBuilder();

        string line;
        while((line = reader.ReadLine()) != null)
        {
            line = line.Trim();
            if (line.Length == 0 && builder.IsComplete)
            {
                yield return builder.Build();
                builder.Reset();
            }
            else if (line.Contains(','))
            {
                line.Split(',').Aggregate(inputs, (agg, e) => {
                    agg.Enqueue(int.Parse(e));
                    return agg;
                });
            }
            else if (line.Contains(' '))
            {
                builder.AddRow(line.Split(' ').Where(v => v.Trim().Length > 0).Select(
                    v => {
                        return int.Parse(v.Trim());
                    }));
            }
        }
        if (builder.IsComplete)
        {
            yield return builder.Build();
        }
    }

    [Fact]
    public void PartA()
    {
        var tilesOrder = new Queue<int>();
        var boards = GetBoards("", tilesOrder).ToList();
        BingoBoard? bingoBoard = default;
        var currentTileValue = 0;

        foreach(var i in tilesOrder)
        {
            if (bingoBoard != default) break;

            foreach(var board in boards)
            {
                if (board.ApplyValue(i))
                {
                    currentTileValue = i;
                    bingoBoard = board;
                    break;
                }
            }
        }

        Assert.Equal(32844, bingoBoard.UnmarkedTileSum() * currentTileValue);
    }

    [Fact]
    public void PartB()
    {
        var tilesOrder = new Queue<int>();
        var boards = GetBoards("", tilesOrder).ToList();
        BingoBoard? bingoBoard = default;
        var currentTileValue = 0;

        foreach (var i in tilesOrder)
        {
            if (boards.Count == 0) break;

            for(var boardIndex=0; boardIndex < boards.Count; boardIndex++)
            {
                if (boards[boardIndex].ApplyValue(i)) {
                    currentTileValue = i;
                    bingoBoard = boards[boardIndex];
                    boards.RemoveAt(boardIndex);
                    boardIndex--;
                }
            }
        }

        Assert.Equal(4920, bingoBoard.UnmarkedTileSum() * currentTileValue);
    }
}

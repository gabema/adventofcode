namespace AdventOfCode.Year2023;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;
using Xunit.Abstractions;

// https://adventofcode.com/2023/day/4
public class Day4
{
    private static readonly Regex scratchGame = new Regex(@"Card\s+(\d+):([\d ]+)\|([\d ]+)");
    private readonly ITestOutputHelper output;

    public Day4(ITestOutputHelper output)
    {
        this.output = output;
    }

    [Fact]
    public void PartA()
    {
        var winnerSum = 0L;
        foreach (var card in ReadCards(""))
        {
            var numWinners = card.YourNumbers.ToHashSet().Intersect(card.WinningNumbers).Count();
            if (numWinners > 0)
            {
                winnerSum += 1 << numWinners-1;
            }
        }
        Assert.Equal(25571, winnerSum);
    }

    [Fact]
    public void PartB()
    {
        var winnerSum = 0L;
        var scratchCards = ReadCards("").ToDictionary(card => card.cardNum);
        var scrachCardCounts = new Dictionary<int, ScratchCardCounter>() {
        };

        int scratchCardCount = scratchCards.Count;

        for (var i = 1; i <= scratchCardCount; i++)
        {
            if (!scrachCardCounts.ContainsKey(i))
            {
                scrachCardCounts[i] = new ScratchCardCounter() { Value = 0, Count = 1 };
            }
            var cardCount = scrachCardCounts[i];
            var card = scratchCards[i];
            var numWinners = card.YourNumbers.ToHashSet().Intersect(card.WinningNumbers).Count();

            for (var j = i + 1; j <= i + numWinners; j++)
            {
                if (scrachCardCounts.TryGetValue(j, out var cardCounter))
                {
                    cardCounter.Count += cardCount.Count;
                }
                else
                {
                    scrachCardCounts.Add(j, new ScratchCardCounter { Count = 1 + cardCount.Count });
                }
            }
        }

        foreach(var kv in scrachCardCounts)
        {
            winnerSum += kv.Value.Count;
        }

        Assert.Equal(8805731, winnerSum);
    }

    record ScratchCardCounter
    {
        public int Count { get; set; }
        public int Value { get; set; }
    }

    record ScratchCard(int cardNum, List<int> YourNumbers, List<int> WinningNumbers);

    IEnumerable<ScratchCard> ReadCards(string variant)
    {
        using var reader = InputClient.GetFileStreamReader(2023, 4, variant);
        string? line;

        while ((line = reader.ReadLine()) != null)
        {
            var m = scratchGame.Match(line);
            var gameNumber = int.Parse(m.Groups[1].Value);
            var winningNumbers = m.Groups[2].Value.Trim().Split(' ').Where(v => v.Length > 0).Select(int.Parse).ToList();
            var yourNumbers = m.Groups[3].Value.Trim().Split(' ').Where(v => v.Length > 0).Select(int.Parse).ToList();
            yield return new ScratchCard(gameNumber, yourNumbers, winningNumbers);
        }
    }
}

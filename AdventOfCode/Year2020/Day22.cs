using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Year2020
{
    public class CombatGame
    {
        private LinkedList<int> player1;
        private LinkedList<int> player2;
        private int cardCount = default;
        private ITestOutputHelper output;

        public void AddPlayerOneCards(IEnumerable<int> cards)
        {
            cardCount += cards.Count();
            player1 = new LinkedList<int>(cards);
        }

        public void AddPlayerTwoCards(IEnumerable<int> cards)
        {
            cardCount += cards.Count();
            player2 = new LinkedList<int>(cards);
        }

        public CombatGame(ITestOutputHelper output)
        {
            this.output = output;
        }

        internal bool PlayRound()
        {
            if (player1.First == null || player2.First == null) return false;

            var card1 = player1.First;
            var card2 = player2.First;

            // output.WriteLine($"Player 1 plays {card1.Value}. Player 2 plays {card2.Value}");

            player1.RemoveFirst();
            player2.RemoveFirst();

            if (card1.Value > card2.Value)
            {
                player1.AddLast(card1);
                player1.AddLast(card2);
            }
            else
            {
                player2.AddLast(card2);
                player2.AddLast(card1);
            }
            return true;
        }

        public int WinnerScore()
        {
            var winner = player1.First != null ? player1.First : player2.First;
            int currentScoreValue = cardCount;
            int totalScore = default;
            while (winner != null)
            {
                totalScore += currentScoreValue * winner.Value;
                currentScoreValue--;
                winner = winner.Next;
            }
            return totalScore;
        }
    }
    // https://adventofcode.com/2020/day/22
    public class Day22
    {
        private ITestOutputHelper output;

        private IEnumerable<int> ReadHand(StreamReader reader)
        {
            string input;
            while (!string.IsNullOrWhiteSpace(input = reader.ReadLine()))
            {
                if (input.StartsWith("Player")) continue;

                yield return int.Parse(input);
            }
        }

        private CombatGame ReadGame()
        {
            using var reader = new StreamReader(InputClient.GetFileStream(2020, 22, ""));
            var game = new CombatGame(output);
            game.AddPlayerOneCards(ReadHand(reader).ToList());
            game.AddPlayerTwoCards(ReadHand(reader).ToList());
            return game;
        }

        public Day22(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void Part1()
        {
            var game = ReadGame();
            while (game.PlayRound()) { };
            // NOT 27555
            Assert.Equal(32783, game.WinnerScore());
        }

        [Fact]
        public void Part2()
        {
            Assert.Equal(1, 0);
        }
    }
}

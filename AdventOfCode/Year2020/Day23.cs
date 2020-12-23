using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Year2020
{
    public class CrabGame
    {
        private const int MARKER_GAP = 50000;
        private LinkedList<int> cups;
        private LinkedListNode<int> current;
        private int maxCup;
        private ITestOutputHelper output;
        public CrabGame(IEnumerable<int> startingCups, ITestOutputHelper output)
        {
            this.output = output;
            cups = new LinkedList<int>(startingCups);
            current = cups.First;
            maxCup = startingCups.Max();
        }

        public void Move()
        {
            List<LinkedListNode<int>> removedCups = new List<LinkedListNode<int>>(3);

            // Remove clockwise cups
            var nextCup = CircularNext(current);
            removedCups.Add(nextCup);
            cups.Remove(nextCup);
            nextCup = CircularNext(current);
            removedCups.Add(nextCup);
            cups.Remove(nextCup);
            nextCup = CircularNext(current);
            removedCups.Add(nextCup);
            cups.Remove(nextCup);

            int findValue = current.Value > 1 ? current.Value - 1 : maxCup;
            while (removedCups.FirstOrDefault(rc => rc.Value == findValue) != null)
            {
                findValue = findValue > 1 ? findValue - 1 : maxCup;
            }

            LinkedListNode<int> destinationCup = CircularNext(current);
            while (destinationCup.Value != findValue)
            {
                destinationCup = CircularNext(destinationCup);
            }

            cups.AddAfter(destinationCup, removedCups[0]);
            cups.AddAfter(removedCups[0], removedCups[1]);
            cups.AddAfter(removedCups[1], removedCups[2]);
            removedCups.Clear();
            current = CircularNext(current);
        }

        private LinkedListNode<int> CircularNext(LinkedListNode<int> current) => current.Next != null ? current.Next : cups.First;

        public string Label()
        {
            LinkedListNode<int> oneCup = cups.First;
            while (oneCup.Value != 1) oneCup = oneCup.Next;

            var builder = new StringBuilder();
            oneCup = CircularNext(oneCup);
            while (oneCup.Value != 1)
            {
                builder.Append(oneCup.Value);
                oneCup = CircularNext(oneCup);
            }
            return builder.ToString();
        }

        public string TwoNextMultiple()
        {
            LinkedListNode<int> oneCup = cups.First;
            while (oneCup.Value != 1) oneCup = oneCup.Next;
            ulong val = 1UL;
            val = val * (ulong)oneCup.Value;
            oneCup = CircularNext(oneCup);
            val = val * (ulong)oneCup.Value;
            return val.ToString();
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            
            for (var t = cups.First; t != null; t = t.Next)
            {
                if (t == current) { builder.Append($" ({t.Value})"); }
                else { builder.Append(' ').Append(t.Value.ToString()); };
            }
            return builder.ToString();
        }
    }

    // https://adventofcode.com/2020/day/23
    public class Day23
    {
        private ITestOutputHelper output;

        public Day23(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void Part1()
        {
            var game = new CrabGame(new List<int> { 6,2,4,3,9,7,1,5,8 }, this.output);
            for (int i = 0; i < 100; i++)
            {
                game.Move();
            }
            Assert.Equal("74698532", game.Label());
        }

        [Fact]
        public void Part2()
        {
            var crabGameInput = new List<int> { 3,8,9,1,2,5,4,6,7 };
            for (int i = 10; i <= 100000/*1000000*/; i++) crabGameInput.Add(i);
            var game = new CrabGame(crabGameInput, this.output);
            //for (int i = 0; i < 100000 /*10000000*/; i++)
            //{
            //    game.Move();
            //}
            Assert.Equal("149245887792", game.TwoNextMultiple());
        }
    }
}

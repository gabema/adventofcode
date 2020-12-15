using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Year2020
{
    public class TwoItem
    {
        private uint[] items;
        private uint nextIndex;

        public TwoItem(uint val)
        {
            items = new uint[] { 0, 0 };
            Add(val);
        }

        public bool HasInitialized => items[1] != 0;

        public void Add(uint val)
        {
            items[nextIndex] = val;
            nextIndex = (nextIndex + 1) % 2;
        }

        public uint Diff {
            get
            {
                if (!HasInitialized) return 0;
                return items[(nextIndex + 1) % 2] - items[nextIndex % 2];
            }
        }
    }

    public class SpokenNumberGame
    {
        public uint SpokenValue { get; private set; }
        public uint Turn { get; private set; }

        public Dictionary<uint, TwoItem> numberMemorizer;
        public uint nextTurn;

        public SpokenNumberGame()
        {
            numberMemorizer = new Dictionary<uint, TwoItem>();
        }

        public void Speak(uint num)
        {
            SpokenValue = num;
            Turn++;
            numberMemorizer.Add(num, new TwoItem(Turn));
        }

        public void Speak()
        {
            Turn++;
            TwoItem lastSpoken = numberMemorizer[SpokenValue];

            if (!lastSpoken.HasInitialized)
            {
                uint newSpoken = 0;
                if (numberMemorizer.TryGetValue(newSpoken, out lastSpoken)) {
                    lastSpoken.Add(Turn);
                } else {
                    numberMemorizer.Add(newSpoken, new TwoItem(Turn));
                }

                SpokenValue = newSpoken;
            } else {
                uint newSpoken = lastSpoken.Diff;

                if (numberMemorizer.TryGetValue(newSpoken, out lastSpoken)) {
                    lastSpoken.Add(Turn);
                }
                else
                {
                    numberMemorizer.Add(newSpoken, new TwoItem(Turn));
                }

                SpokenValue = newSpoken;
            }
        }
    }

    // https://adventofcode.com/2020/day/14
    public class Day15
    {
        private ITestOutputHelper output;

        public Day15(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void Part1()
        {
            var game = new SpokenNumberGame();
            game.Speak(20U);
            game.Speak(9U);
            game.Speak(11U);
            game.Speak(0U);
            game.Speak(1U);
            game.Speak(2U);
            for (; game.Turn != 2020; game.Speak()) { }

            // NOT 1930
            Assert.Equal(1111U, game.SpokenValue);
        }

        [Fact]
        public void Part2()
        {
            var game = new SpokenNumberGame();
            game.Speak(20U);
            game.Speak(9U);
            game.Speak(11U);
            game.Speak(0U);
            game.Speak(1U);
            game.Speak(2U);
            for (; game.Turn != 30000000U; game.Speak()) { }

            // NOT 1930
            Assert.Equal(48568U, game.SpokenValue);
        }

    }
}


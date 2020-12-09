using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Year2020
{
    public class BoundedCollection
    {
        private readonly int maxSize;
        private readonly List<ulong> collection;
        private int nextIndex;

        public BoundedCollection(int size)
        {
            maxSize = size;
            nextIndex = 0;
            collection = new List<ulong>(25);
        }

        /**
         * returns true if value was a valid number. False if it fails the checksum
         */
        public bool Add(ulong value)
        {
            var foundSum = collection.Count < maxSize;
            var endVal = Math.Min(collection.Count, maxSize);
            if (!foundSum)
            {
                for (int i = 0; i < endVal && !foundSum; i++)
                {
                    for (int j = i + 1; j < endVal && !foundSum; j++)
                    {
                        foundSum = collection[i] + collection[j] == value;
                    }
                }
            }

            if (endVal != maxSize)
            {
                collection.Add(value);
            } else
            {
                collection[nextIndex] = value;
            }
            nextIndex = (nextIndex + 1) % maxSize;

            return foundSum;
        }

        public IEnumerable<ulong> ToEnumerable()
        {
            return collection;
        }
    }

    // https://adventofcode.com/2020/day/9
    public class Day9
    {
        private readonly ITestOutputHelper output;

        public Day9(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void Part1()
        {
            var collection = new BoundedCollection(25);
            var badValue = 0UL;
            string input;
            foreach (var numVal in GetNumbers())
            {
                if (!collection.Add(numVal))
                {
                    badValue = numVal;
                    break;
                }
            }

            Assert.Equal(15353384UL, badValue);
        }

        [Fact]
        public void Part2()
        {
            var collection = new BoundedCollection(25);
            var fullCollection = new List<ulong>();
            var invalidNumberCollection = new HashSet<ulong>();
            string input;
            ulong invalidContiguousSum = 0UL;
            foreach (var numVal in GetNumbers())
            {
                if (!collection.Add(numVal))
                {
                    invalidNumberCollection.Add(numVal);
                }
                fullCollection.Add(numVal);
            }

            var maxInvalidNumber = invalidNumberCollection.Max();
            var sizeOfCollection = fullCollection.Count;
            var foundCheckedInvalid = false;
            for (var i = 0; i < sizeOfCollection && !foundCheckedInvalid; i++)
            {
                var sumContiguous = fullCollection[i];
                var j = i + 1;
                for (; j < sizeOfCollection && !foundCheckedInvalid && sumContiguous < maxInvalidNumber ; j++)
                {
                    sumContiguous += fullCollection[j];
                    foundCheckedInvalid = invalidNumberCollection.Contains(sumContiguous);
                }

                if (foundCheckedInvalid)
                {
                    var continuousRange = fullCollection.GetRange(i, j - i - 1);
                    invalidContiguousSum = continuousRange.Max() + continuousRange.Min();
                }
            }

            Assert.Equal(2466556UL, invalidContiguousSum);
        }

        public IEnumerable<ulong> GetNumbers()
        {
            using var reader = new StreamReader(InputClient.GetFileStream(2020, 9, ""));
            string input;
            while ((input = reader.ReadLine()) != null)
            {
                yield return ulong.Parse(input);
            }
        }
    }
}

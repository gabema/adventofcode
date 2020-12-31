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
    // https://adventofcode.com/2020/day/24
    public class Day25
    {
        private ITestOutputHelper output;

        public Day25(ITestOutputHelper output)
        {
            this.output = output;
        }

        private ulong loopValue(ulong subjectNumber, int loopCount)
        {
            ulong value = 1UL;
            for(; loopCount > 0; loopCount--)
            {
                value *= subjectNumber;
                value %= 20201227UL;
            }
            return value;
        }

        private int calculateLoopSize(ulong subjectNumber, ulong finalValue)
        {
            int loopSize = 0;
            ulong value = 1UL;
            while (finalValue != value)
            {
                loopSize++;
                value *= subjectNumber;
                value %= 20201227UL;
            }
            return loopSize;
        }

        [Fact]
        public void Part1()
        {
            ulong cardPublicKey = 5099500UL;
            ulong doorPublicKey = 7648211UL;
            int cardLoopSize = calculateLoopSize(7UL, cardPublicKey);
            int doorLoopSize = calculateLoopSize(7UL, doorPublicKey);
            ulong encryptionKey = loopValue(doorPublicKey, cardLoopSize);

            Assert.Equal(11288669UL, encryptionKey);
            Assert.Equal(encryptionKey, loopValue(cardPublicKey, doorLoopSize));
        }

        [Fact]
        public void Part2()
        {
            Assert.Equal(1, 1);
        }
    }
}

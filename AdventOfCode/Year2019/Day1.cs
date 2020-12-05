using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Year2019
{
    public class Day1
    {
        private readonly ITestOutputHelper output;

        public Day1(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void PartA()
        {
            ulong totalVal = 0;
            foreach (int val in InputClient.GetIntInput(2019, 1))
            {
                totalVal += (ulong)(val / 3 - 2);
            }

            output.WriteLine($"{totalVal}");
            Assert.Equal(3512133UL, totalVal);
        }

        [Fact]
        public void PartB()
        {
            output.WriteLine("Starting");
            ulong totalVal = 0;
            foreach (int val in InputClient.GetIntInput(2019, 1))
            {
                totalVal += (ulong)GetEnumerable(val).Sum();
            }

            Assert.Equal(5265294UL, totalVal);
        }

        IEnumerable<int> GetEnumerable(int value)
        {
            value = value / 3 - 2;
            while (value > 0)
            {
                yield return value;
                value = value / 3 - 2;
            }
        }
    }
}

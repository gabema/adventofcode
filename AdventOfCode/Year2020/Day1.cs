using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Year2020
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
            int totalVal = 0;
            IList<int> intValues = InputClient.GetIntInput(2020, 1, "").ToList();

            for(int a = 0; a < intValues.Count; a++)
            {
                for (int b = a + 1; b < intValues.Count; b++)
                {
                    if (intValues[a] + intValues[b] == 2020)
                    {
                        totalVal = intValues[a] * intValues[b];
                        break;
                    }
                }

                if (totalVal != 0) break;
            }

            output.WriteLine($"{totalVal}");
            Assert.Equal(692916, totalVal);
        }

        [Fact]
        public void PartB()
        {
            ulong totalVal = 0UL;
            IList<int> intValues = InputClient.GetIntInput(2020, 1, "").ToList();

            for (int a = 0; a < intValues.Count; a++)
            {
                for (int b = a + 1; b < intValues.Count; b++)
                {
                    for (int c = b + 1; c < intValues.Count; c++)
                    {

                        if (intValues[a] + intValues[b] + intValues[c] == 2020)
                        {
                            totalVal = (ulong)intValues[a] * (ulong)intValues[b] * (ulong)intValues[c];
                            break;
                        }
                    }
                    if (totalVal != 0) break;
                }
                if (totalVal != 0) break;
            }

            output.WriteLine($"{totalVal}");
            Assert.Equal(289270976UL, totalVal);
        }
    }
}

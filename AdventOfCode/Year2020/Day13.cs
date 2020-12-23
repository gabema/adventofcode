using System.Collections.Generic;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Year2020
{
    public class Bus
    {
        private readonly int num;

        public int BusNum { get => num; }

        public Bus(int num)
        {
            this.num = num;
        }

        public ulong EarliestTimestampOfOrder(ulong when)
        {
            ulong numRounds = (when / (ulong)num) + 1;
            ulong waitTime = numRounds * (ulong)num - when;
            return waitTime;
        }

        public bool BusArrived(ulong when)
        {
            return when % (ulong)num == 0;
        }
    }

    public class BusScanner
    {
        public ulong TimestampOfOrder(List<Bus> order)
        {
            int nextBusIndex = 0;
            ulong timestamp = 1;
            while (nextBusIndex < order.Count)
            {
                if (timestamp == 3417UL)
                {
                    timestamp = timestamp;
                }
                var bus = order[nextBusIndex];
                if (bus == null || bus.BusArrived(timestamp)) {
                    nextBusIndex++;
                    if (nextBusIndex < order.Count) timestamp++;
                } else {
                    timestamp += order[0].EarliestTimestampOfOrder(timestamp);
                    nextBusIndex = 0;
                }
            }
            return timestamp - (ulong)order.Count + 1UL;
        }
    }

    // https://adventofcode.com/2020/day/11
    public class Day13
    {
        private readonly ITestOutputHelper output;

        public Day13(ITestOutputHelper output)
        {
            this.output = output;
        }

        public IEnumerable<Bus> ReadBuses(StreamReader reader, bool includeNulls)
        {
            var input = reader.ReadLine().Split(',');
            foreach (var possibleVal in input)
            {
                int busNum;
                if (int.TryParse(possibleVal, out busNum))
                {
                    yield return new Bus(busNum);
                } else if (includeNulls)
                {
                    yield return null;
                }
            }
        }

        [Fact]
        public void Part1()
        {
            using var reader = new StreamReader(InputClient.GetFileStream(2020, 13, ""));
            var time = ulong.Parse(reader.ReadLine());

            var buses = new List<Bus>(ReadBuses(reader, false));

            buses.Sort(( a, b ) => a.EarliestTimestampOfOrder(time) > b.EarliestTimestampOfOrder(time) ? 1 : -1);
            var quickestBus = buses[0];

            Assert.Equal(259UL, (ulong)quickestBus.BusNum * quickestBus.EarliestTimestampOfOrder(time));
        }

        [Fact]
        public void Part2()
        {
            using var reader = new StreamReader(InputClient.GetFileStream(2020, 13, ""));
            reader.ReadLine(); // unused line
            var buses = new List<Bus>(ReadBuses(reader, true));

            var scanner = new BusScanner();
            // var timestamp = scanner.TimestampOfOrder(buses);
            var timestamp = 0UL;

            Assert.Equal(1068781UL, timestamp);
        }

    }
}

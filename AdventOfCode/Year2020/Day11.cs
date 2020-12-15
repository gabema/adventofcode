using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Year2020
{
    public enum Spot
    {
        SeatEmpty = 'L',
        Floor = '.',
        SeatOccupied = '#'
    }

    public class FerrySeats
    {
        private List<List<Spot>> seats;

        public FerrySeats()
        {
            seats = new List<List<Spot>>();
        }

        public FerrySeats(int height)
        {
            seats = new List<List<Spot>>(height);
        }

        public static FerrySeats ReadFromFile(StreamReader reader)
        {
            string input;

            var newSeats = new FerrySeats();

            while ((input = reader.ReadLine()) != null)
            {
                newSeats.seats.Add(
                    input.ToCharArray().Select(c => (Spot)c).ToList()
                );
            }

            return newSeats;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            foreach(var row in seats)
            {
                builder.Append(row.Select(s => (char)s).ToArray());
                builder.AppendLine();
            }
            return builder.ToString();
        }

        public override int GetHashCode()
        {
            var newHashCode = new HashCode();
            foreach(var row in seats)
            {
                foreach (var seat in row)
                {
                    newHashCode.Add(seat);
                }
            }
            return newHashCode.ToHashCode();
        }

        public FerrySeats NextSeen()
        {
            int width = seats[0].Count;
            int height = seats.Count;
            var newFerry = new FerrySeats(height);
            for (int y = 0; y < height; y++)
            {
                List<Spot> row = new List<Spot>(width);
                for (int x = 0; x < width; x++)
                {
                    row.Add(NextSeen(x, y, height, width));
                }
                newFerry.seats.Add(row);
            }
            return newFerry;
        }

        private bool IsOccupied(int x, int y, int xAdd, int yAdd, int height, int width)
        {
            int endX = xAdd == 0 ? x + 1 : xAdd > 0 ? width  : -1;
            int endY = yAdd == 0 ? y + 1 : yAdd > 0 ? height : -1;

            for (int i = x + xAdd, j = y + yAdd;
                endX - (x + i) != 0 && endY - (y + j) != 0;
                i += xAdd, j += yAdd)
            {
                if (seats[j][i] == Spot.SeatOccupied) return true;
            }

            return false;
        }

        private Spot NextSeen(int x, int y, int height, int width)
        {
            var currentSpot = seats[y][x];
            int numOccupied = 0;

            if (currentSpot == Spot.Floor) return currentSpot;

            if (IsOccupied(x, y, -1, -1, height, width)) numOccupied++;
            if (IsOccupied(x, y, -1, 0, height, width)) numOccupied++;
            if (IsOccupied(x, y, -1, 1, height, width)) numOccupied++;

            if (IsOccupied(x, y, 0, -1, height, width)) numOccupied++;
            if (IsOccupied(x, y, 0, 1, height, width)) numOccupied++;

            if (IsOccupied(x, y, +1, -1, height, width)) numOccupied++;
            if (IsOccupied(x, y, +1, 0, height, width)) numOccupied++;
            if (IsOccupied(x, y, +1, 1, height, width)) numOccupied++;

            if (currentSpot == Spot.SeatEmpty && numOccupied == 0) currentSpot = Spot.SeatOccupied;
            else if (currentSpot == Spot.SeatOccupied && numOccupied >= 5) currentSpot = Spot.SeatEmpty;

            return currentSpot;
        }

        public FerrySeats Next()
        {
            int width = seats[0].Count;
            int height = seats.Count;
            var newFerry = new FerrySeats(height);
            for (int y = 0; y < height; y++)
            {
                List<Spot> row = new List<Spot>(width);
                for (int x = 0; x < width; x++)
                {
                    row.Add(Next(x, y, height, width));
                }
                newFerry.seats.Add(row);
            }
            return newFerry;
        }

        public int OccupiedSeatCount()
        {
            int numOccupied = 0;
            foreach(Spot s in ToEnumerable())
            {
                if (s == Spot.SeatOccupied)
                {
                    numOccupied++;
                }
            }
            return numOccupied;
        }

        public IEnumerable<Spot> ToEnumerable()
        {
            foreach (var row in seats)
            {
                foreach (var seat in row)
                {
                    yield return seat;
                }
            }
        }

        private Spot Next(int x, int y, int height, int width)
        {
            var currentSpot = seats[y][x];
            int numOccupied = 0;

            if (currentSpot == Spot.Floor) return currentSpot;

            if (y != 0 && seats[y - 1][x] == Spot.SeatOccupied) numOccupied++;
            if (y != 0 && x != 0 && seats[y - 1][x - 1] == Spot.SeatOccupied) numOccupied++;
            if (y != 0 && x != width - 1 && seats[y - 1][x + 1] == Spot.SeatOccupied) numOccupied++;

            if (x != 0 && seats[y][x - 1] == Spot.SeatOccupied) numOccupied++;
            if (x != width - 1 && seats[y][x + 1] == Spot.SeatOccupied) numOccupied++;

            if (y != height - 1 && seats[y + 1][x] == Spot.SeatOccupied) numOccupied++;
            if (y != height - 1 && x != 0 && seats[y + 1][x - 1] == Spot.SeatOccupied) numOccupied++;
            if (y != height - 1 && x != width - 1 && seats[y + 1][x + 1] == Spot.SeatOccupied) numOccupied++;

            if (currentSpot == Spot.SeatEmpty && numOccupied == 0) currentSpot = Spot.SeatOccupied;
            else if (currentSpot == Spot.SeatOccupied && numOccupied >= 4) currentSpot = Spot.SeatEmpty;

            return currentSpot;
        }
    }

    // https://adventofcode.com/2020/day/11
    public class Day11
    {
        private readonly ITestOutputHelper output;

        public Day11(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void Part1()
        {
            using var reader = new StreamReader(InputClient.GetFileStream(2020, 11, ""));
            var ferrySeats = FerrySeats.ReadFromFile(reader);
            int lastHash = ferrySeats.GetHashCode();
            int generation = 0;
            int nextHash;
            output.WriteLine($"Generation {generation} Layout is: \n" + ferrySeats + $"\n code is {ferrySeats.GetHashCode()}\n");
            while (lastHash != (nextHash = (ferrySeats = ferrySeats.Next()).GetHashCode()))
            {
                generation++;
                lastHash = nextHash;
                output.WriteLine($"Generation {generation} Layout is: \n" + ferrySeats + $"\n code is {ferrySeats.GetHashCode()}\n");
            }
            Assert.Equal(2424, ferrySeats.OccupiedSeatCount());
        }

        [Fact]
        public void Part2()
        {
            using var reader = new StreamReader(InputClient.GetFileStream(2020, 11, ""));
            var ferrySeats = FerrySeats.ReadFromFile(reader);
            int lastHash = ferrySeats.GetHashCode();
            int generation = 0;
            int nextHash;
            output.WriteLine($"Generation {generation} Layout is: \n" + ferrySeats + $"\n code is {ferrySeats.GetHashCode()}\n");
            while (lastHash != (nextHash = (ferrySeats = ferrySeats.NextSeen()).GetHashCode()))
            {
                generation++;
                lastHash = nextHash;
                output.WriteLine($"Generation {generation} Layout is: \n" + ferrySeats + $"\n code is {ferrySeats.GetHashCode()}\n");
            }
            Assert.Equal(2424, ferrySeats.OccupiedSeatCount());
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace AdventOfCode.Year2019
{
    public enum Day3Tokens
    {
        Left,
        Right,
        Down,
        Up,
        Value,
        Seperator,
        SequenceTerminator
    };

    public class Day3Lexer : SimpleLexer<Day3Tokens>
    {
        private static readonly List<Tuple<Day3Tokens, string>> NewList = new List<Tuple<Day3Tokens, string>> {
            Tuple.Create(Day3Tokens.Left, @"^l\d+"),
            Tuple.Create(Day3Tokens.Right, @"^r\d+"),
            Tuple.Create(Day3Tokens.Down, @"^d\d+"),
            Tuple.Create(Day3Tokens.Up, @"^u\d+"),
            Tuple.Create(Day3Tokens.Seperator, @"^,"),
            Tuple.Create(Day3Tokens.SequenceTerminator, @""),
        };

        public Day3Lexer() : base(NewList)
        { }
    }

    // Use C# 9 records
    public class Spot
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public int Distance { get; private set; }

        public Spot(int x, int y, int distance = 0)
        {
            X = x;
            Y = y;
            Distance = distance;
        }

        public int ManhattanDistanceFrom(Spot s)
        {
            return Math.Abs(X - s.X) + Math.Abs(Y - s.Y);
        }

        public int SignalDistance(Spot s)
        {
            return s.Distance + Distance + 2;
        }

        public override int GetHashCode()
        {
            // TODO Attempt to replace with C# 9 Record classes
            return HashCode.Combine(X, Y);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != typeof(Spot)) return false;

            return X == ((Spot)obj).X && Y == ((Spot)obj).Y;
        }
    }

    public class Square
    {
        private Dictionary<int, Spot> spots;
        private Spot currentSpot;

        public Square()
        {
            currentSpot = new Spot(0, 0);
            spots = new Dictionary<int, Spot>
            {
                [currentSpot.GetHashCode()] = currentSpot
            };
        }

        public Spot GetSpot(Spot s)
        {
            return spots[s.GetHashCode()];
        }

        public IEnumerable<Spot> Intersect(Square other)
        {
            return spots.Values.Intersect(other.spots.Values);
        }

        public void Move(Day3Tokens direction, int distance)
        {
            switch (direction)
            {
                case Day3Tokens.Down:
                    {
                        Spot newSpot = new Spot(currentSpot.X, currentSpot.Y - distance, currentSpot.Distance + distance);
                        AddSpotsBetween(currentSpot, newSpot);
                        spots.TryAdd(newSpot.GetHashCode(), newSpot);
                        currentSpot = newSpot;
                        break;
                    }
                case Day3Tokens.Up:
                    {
                        Spot newSpot = new Spot(currentSpot.X, currentSpot.Y + distance, currentSpot.Distance + distance);
                        AddSpotsBetween(currentSpot, newSpot);
                        spots.TryAdd(newSpot.GetHashCode(), newSpot);
                        currentSpot = newSpot;
                        break;
                    }
                case Day3Tokens.Left:
                    {
                        Spot newSpot = new Spot(currentSpot.X - distance, currentSpot.Y, currentSpot.Distance + distance);
                        AddSpotsBetween(currentSpot, newSpot);
                        spots.TryAdd(newSpot.GetHashCode(), newSpot);
                        currentSpot = newSpot;
                        break;
                    }
                case Day3Tokens.Right:
                    {
                        Spot newSpot = new Spot(currentSpot.X + distance, currentSpot.Y, currentSpot.Distance + distance);
                        AddSpotsBetween(currentSpot, newSpot);
                        spots.TryAdd(newSpot.GetHashCode(), newSpot);
                        currentSpot = newSpot;
                        break;
                    }
            }
        }

        private void AddSpotsBetween(Spot currentSpot, Spot newSpot)
        {
            int distance = newSpot.Distance;
            int y = newSpot.Y;
            while (currentSpot.Y < y)
            {
                Spot ns = new Spot(currentSpot.X, y--, --distance);
                spots.TryAdd(ns.GetHashCode(), ns);
            }
            while (currentSpot.Y > y)
            {
                Spot ns = new Spot(currentSpot.X, y++, --distance);
                spots.TryAdd(ns.GetHashCode(), ns);
            }
            int x = newSpot.X;
            while (currentSpot.X < x)
            {
                Spot ns = new Spot(x--, currentSpot.Y, --distance);
                spots.TryAdd(ns.GetHashCode(), ns);
            }
            while (currentSpot.X > x)
            {
                Spot ns = new Spot(x++, currentSpot.Y, --distance);
                spots.TryAdd(ns.GetHashCode(), ns);
            }
        }
    }

    public class Day3
    {
        private static Square PopulateSquare(IEnumerable<Token<Day3Tokens>> iterable)
        {
            var square = new Square();
            foreach (Token<Day3Tokens> token in iterable)
            {
                switch (token.TokenType)
                {
                    case Day3Tokens.Down:
                    case Day3Tokens.Up:
                    case Day3Tokens.Left:
                    case Day3Tokens.Right:
                        square.Move(token.TokenType, int.Parse(token.Value.Substring(1)));
                        break;
                }
            }
            return square;
        }

        [Fact]
        void PartA()
        {
            var stream = InputClient.GetFileStream(2019, 3, "");
            using var reader = new StreamReader(stream);
            var input = reader.ReadLine();
            var lexer = new Day3Lexer();
            IEnumerable<Token<Day3Tokens>> left = lexer.Tokenize(input);
            var leftSquare = PopulateSquare(left);

            input = reader.ReadLine();
            IEnumerable<Token<Day3Tokens>> right = lexer.Tokenize(input);
            var rightSquare = PopulateSquare(right);

            var overlaps = leftSquare.Intersect(rightSquare);
            var originSpot = new Spot(0, 0);
            int minDistance = overlaps.Except(new List<Spot> { originSpot }).Min(s => s.ManhattanDistanceFrom(originSpot));
            Assert.Equal(557, minDistance);
        }

        [Fact]
        void PartB()
        {
            var stream = InputClient.GetFileStream(2019, 3, "");
            using var reader = new StreamReader(stream);
            var input = reader.ReadLine();
            var lexer = new Day3Lexer();
            IEnumerable<Token<Day3Tokens>> left = lexer.Tokenize(input);
            var leftSquare = PopulateSquare(left);

            input = reader.ReadLine();
            IEnumerable<Token<Day3Tokens>> right = lexer.Tokenize(input);
            var rightSquare = PopulateSquare(right);

            var overlaps = leftSquare.Intersect(rightSquare);
            var originSpot = new Spot(0, 0);
            int minDistance = overlaps.Except(new List<Spot> { originSpot })
                .Select(s => leftSquare.GetSpot(s).SignalDistance(rightSquare.GetSpot(s)))
                .Min();
            Assert.Equal(56410, minDistance);
        }
    }
}

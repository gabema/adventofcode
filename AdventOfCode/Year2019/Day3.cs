using System;
using System.Collections.Generic;
using System.IO;
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
            Tuple.Create(Day3Tokens.Left, @"^l"),
            Tuple.Create(Day3Tokens.Right, @"^r"),
            Tuple.Create(Day3Tokens.Down, @"^d"),
            Tuple.Create(Day3Tokens.Up, @"^u"),
            Tuple.Create(Day3Tokens.Seperator, @"^,"),
            Tuple.Create(Day3Tokens.Value, @"^\d+"),
            Tuple.Create(Day3Tokens.SequenceTerminator, @""),
        };

        public Day3Lexer() : base(NewList)
        { }
    }

    public class Square
    {
        public int Left { get; private set; }
        public int Right { get; private set; }
        public int Top { get; private set; }
        public int Bottom { get; private set; }

        public int X { get; private set; }
        public int Y { get; private set; }

        public Square()
        {
            Left = Right = Top = Bottom = X = Y = 0;
        }

        public Square Overlap(Square other)
        {
            var newSquare = new Square();
            newSquare.Left = Math.Max(Left, other.Left);
            newSquare.Right = Math.Min(Right, other.Right);
            newSquare.Top = Math.Min(Top, other.Top);
            newSquare.Bottom = Math.Max(Bottom, other.Bottom);
            return newSquare;
        }

        public void Move(Day3Tokens direction, int distance)
        {
            switch (direction)
            {
                case Day3Tokens.Down:
                {
                    Y -= distance;
                    if (Bottom > Y)
                    {
                        Bottom = Y;
                    }
                        break;
                }
                case Day3Tokens.Up:
                    {
                        Y += distance;
                        if (Y > Top)
                        {
                            Top = Y;
                        }
                        break;
                    }
                case Day3Tokens.Left:
                    {
                        X -= distance;
                        if (Left > X)
                        {
                            Left = X;
                        }
                        break;
                    }
                case Day3Tokens.Right:
                    {
                        X += distance;
                        if (X > Right)
                        {
                            Right = X;
                        }
                        break;
                    }
            }
        }
    }

    public class Day3
    {
        private static Square PopulateSquare(IEnumerable<Token<Day3Tokens>> iterable)
        {
            Day3Tokens currentToken = Day3Tokens.Seperator;
            var square = new Square();
            foreach (Token<Day3Tokens> token in iterable)
            {
                switch (token.TokenType)
                {
                    case Day3Tokens.Down:
                    case Day3Tokens.Up:
                    case Day3Tokens.Left:
                    case Day3Tokens.Right:
                        currentToken = token.TokenType;
                        break;
                    case Day3Tokens.Value:
                        square.Move(currentToken, int.Parse(token.Value));
                        break;
                }
            }
            return square;
        }

        [Fact]
        void TestSample1()
        {
            var stream = InputClient.GetFileStream(2019, 3, "a");
            using var reader = new StreamReader(stream);
            var input = reader.ReadLine();
            var lexer = new Day3Lexer();
            IEnumerable<Token<Day3Tokens>> left = lexer.Tokenize(input);
            var leftSquare = PopulateSquare(left);

            input = reader.ReadLine();
            IEnumerable<Token<Day3Tokens>> right = lexer.Tokenize(input);
            var rightSquare = PopulateSquare(left);

        }
    }
}

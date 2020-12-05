using Advent2020;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Year2020
{
    public enum Day5Items
    {
        Front,
        Back,
        Left,
        Right,
        Terminator
    }

    public class Day5Lexer : SimpleLexer<Day5Items>
    {
        private static readonly List<Tuple<Day5Items, string>> NewList = new List<Tuple<Day5Items, string>> {
            Tuple.Create(Day5Items.Front, @"^f"),
            Tuple.Create(Day5Items.Back, @"^b"),
            Tuple.Create(Day5Items.Left, @"^l"),
            Tuple.Create(Day5Items.Right, @"^r"),
            Tuple.Create(Day5Items.Terminator, @""),
        };

        public Day5Lexer() : base(NewList)
        { }
    }

    public class SeatRow
    {
        public Range CurrentRange { get; private set; }

        public SeatRow()
        {
            CurrentRange = new Range(0, 128);
        }

        public void Front()
        {
            CurrentRange = new Range(CurrentRange.Start,
                CurrentRange.Start.Value + (CurrentRange.End.Value - CurrentRange.Start.Value) / 2);
        }

        public void Back()
        {
            CurrentRange = new Range(CurrentRange.End.Value - (CurrentRange.End.Value - CurrentRange.Start.Value) / 2,
                CurrentRange.End);
        }
    }

    public class SeatColumn
    {
        public Range CurrentRange { get; private set; }

        public SeatColumn()
        {
            CurrentRange = new Range(0, 8);
        }

        public void Left()
        {
            CurrentRange = new Range(CurrentRange.Start,
                CurrentRange.Start.Value + (CurrentRange.End.Value - CurrentRange.Start.Value) / 2);
        }

        public void Right()
        {
            CurrentRange = new Range(CurrentRange.End.Value - (CurrentRange.End.Value - CurrentRange.Start.Value) / 2,
                CurrentRange.End);
        }
    }

    public class SeatAssignment
    {
        public SeatColumn Column { get; private set; }

        public SeatRow Row { get; private set; }

        public SeatAssignment()
        {
            Column = new SeatColumn();
            Row = new SeatRow();
        }

        public void Front() => Row.Front();

        public void Back() => Row.Back();

        public void Left() => Column.Left();

        public void Right() => Column.Right();

        public int SeatID() => Row.CurrentRange.Start.Value * 8 + Column.CurrentRange.Start.Value;
    }

    // https://adventofcode.com/2020/day/4
    public class Day5
    {
        private readonly ITestOutputHelper output;

        public Day5(ITestOutputHelper output)
        {
            this.output = output;
        }

        public IEnumerable<int> ReadSeatAssignments()
        {
            var stream = InputClient.GetFileStream(2020, 5, "");
            using var reader = new StreamReader(stream);
            string input;
            var lexer = new Day5Lexer();
            while ((input = reader.ReadLine())!= null)
            {
                var assigment = new SeatAssignment();
                foreach (Token<Day5Items> item in lexer.Tokenize(input))
                {
                    switch(item.TokenType)
                    {
                        case Day5Items.Front:
                            assigment.Front();
                            break;
                        case Day5Items.Back:
                            assigment.Back();
                            break;
                        case Day5Items.Left:
                            assigment.Left();
                            break;
                        case Day5Items.Right:
                            assigment.Right();
                            break;
                    }
                }
                yield return assigment.SeatID();
            }
        }

        [Fact]
        public void Part1()
        {
            int maxSeatAssignment = ReadSeatAssignments().Max();
            Assert.Equal(953, maxSeatAssignment);
        }

        [Fact]
        public void Part2()
        {
            var seatList = ReadSeatAssignments().OrderBy(v => v).ToList();
            ISet<int> possibleSeats = new HashSet<int>();
            int yourSeatID = 0;
            for(var i = 1; i < seatList.Count; i++)
            {
                if (seatList[i - 1] + 2 == seatList[i])
                {
                    yourSeatID = seatList[i] - 1;
                    break;
                }
            }

            Assert.Equal(615, yourSeatID);
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Year2019
{
    public enum OpCodes
    {
        Add = 1,
        Multiply = 2,
        Halt = 99
    }

    public enum Day2Item
    {
        Value,
        Terminator
    }

    public class Day2Lexer : SimpleLexer<Day2Item> {
        private static readonly List<Tuple<Day2Item, string>> Items = new List<Tuple<Day2Item, string>>{
            Tuple.Create(Day2Item.Value, @"^\d+"),
            Tuple.Create(Day2Item.Terminator, @"")
        };

        public Day2Lexer() : base(Items)
        { }
    }

    public class OpCodeComputer
    {
        private IList<int> content;
        private int instructionPointerIndex;
        public OpCodeComputer(IList<int> input)
        {
            this.content = input;
            instructionPointerIndex = 0;
        }

        // Runs the program until it reaches a halt instruction.
        public void Run()
        {
            OpCodes opCode;

            while ((opCode = (OpCodes)content[instructionPointerIndex]) != OpCodes.Halt)
            {
                int inputA = content[content[instructionPointerIndex + 1]];
                int inputB = content[content[instructionPointerIndex + 2]];
                switch (opCode)
                {
                    case OpCodes.Add:
                        {
                            content[content[instructionPointerIndex + 3]] = inputA + inputB;
                            break;
                        }
                    case OpCodes.Multiply:
                        {
                            content[content[instructionPointerIndex + 3]] = inputA * inputB;
                            break;
                        }
                }
                instructionPointerIndex += 4;
            }
        }

        public int Index { get => instructionPointerIndex; }

        public int ValueAt(int index) => content[index];

        public void SetValueAt(int index, int value) {
            content[index] = value;
        }
    }

    public class Day2
    {
        private readonly ITestOutputHelper output;

        private IEnumerable<int> ReadComputer()
        {
            var inputReader = new StreamReader(InputClient.GetFileStream(2019, 2, ""));

            Day2Lexer lexer = new Day2Lexer();

            var tokens = lexer.Tokenize(inputReader.ReadToEnd());
            return tokens.Where(t => t.TokenType == Day2Item.Value).Select(t => int.Parse(t.Value));
        }

        public Day2(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void PartA()
        {
            var computer = new OpCodeComputer(ReadComputer().ToList());
            computer.SetValueAt(1, 12);
            computer.SetValueAt(2, 2);
            computer.Run();

            Assert.Equal(9706670, computer.ValueAt(0));
        }

        [Fact]
        public void PartB()
        {
            var initialState = ReadComputer();
            OpCodeComputer computer = new OpCodeComputer(initialState.ToList());
            int findOutput = 19690720;
            for (int opCode1 = 0; opCode1 < 100 && computer.ValueAt(0) != findOutput; opCode1++)
            {
                for (int opCode2 = 0; opCode2 < 100 && computer.ValueAt(0) != findOutput; opCode2++)
                {
                    computer = new OpCodeComputer(initialState.ToList());
                    computer.SetValueAt(1, opCode1);
                    computer.SetValueAt(2, opCode2);
                    computer.Run();
                }
            }
            int val = 100 * computer.ValueAt(1) + computer.ValueAt(2);
            Assert.Equal(2552, val);
        }
    }
}

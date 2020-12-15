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
        Input = 3,
        Output = 4,
        Halt = 99
    }

    public enum Modes
    {
        Position = 0,
        Immediate = 1
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

        private (OpCodes code, Modes[] paramModes) ProcessInstruction(int input)
        {
            var inputStr = input.ToString("00000");
            var paramModes = new Modes[3]
            {
                (Modes)int.Parse(inputStr[^3..^2]),
                (Modes)int.Parse(inputStr[^4..^3]),
                (Modes)int.Parse(inputStr[^5..^4])
            };
            return ((OpCodes)int.Parse(inputStr[^2..]), paramModes);
        }

        // Runs the program until it reaches a halt instruction.
        public void Run(Queue<int> inputs, Queue<int> outputs)
        {
            int instructionCounter = 0;
            (OpCodes code, Modes[] paramModes) previousInstruction = default;
            (OpCodes code, Modes[] paramModes) instruction;

            while ((instruction = ProcessInstruction(content[instructionPointerIndex])).code != OpCodes.Halt)
            {
                int nextInstruction = 1;
                switch (instruction.code)
                {
                    case OpCodes.Add:
                        {
                            int inputA = instruction.paramModes[0] == Modes.Position ? content[content[instructionPointerIndex + 1]] : content[instructionPointerIndex + 1];
                            int inputB = instruction.paramModes[1] == Modes.Position ? content[content[instructionPointerIndex + 2]] : content[instructionPointerIndex + 2];
                            if (instruction.paramModes[2] == Modes.Position)
                            {
                                content[content[instructionPointerIndex + 3]] = inputA + inputB;
                            }
                            else
                            {
                                content[instructionPointerIndex + 3] = inputA + inputB;
                            }
                            nextInstruction += 3;
                            break;
                        }
                    case OpCodes.Multiply:
                        {
                            int inputA = instruction.paramModes[0] == Modes.Position ? content[content[instructionPointerIndex + 1]] : content[instructionPointerIndex + 1];
                            int inputB = instruction.paramModes[1] == Modes.Position ? content[content[instructionPointerIndex + 2]] : content[instructionPointerIndex + 2];
                            if (instruction.paramModes[2] == Modes.Position)
                            {
                                content[content[instructionPointerIndex + 3]] = inputA * inputB;
                            }
                            else
                            {
                                content[instructionPointerIndex + 3] = inputA * inputB;
                            }
                            nextInstruction += 3;
                            break;
                        }
                    case OpCodes.Input:
                        {
                            int input = inputs.Dequeue();
                            if (instruction.paramModes[0] == Modes.Position)
                            {
                                content[content[instructionPointerIndex + 1]] = input;
                            }
                            else
                            {
                                content[instructionPointerIndex + 1] = input;
                            }
                            nextInstruction += 1;
                            break;
                        }
                    case OpCodes.Output:
                        {
                            var outputVal = instruction.paramModes[0] == Modes.Position ? content[content[instructionPointerIndex + 1]] : content[instructionPointerIndex + 1];
                            outputs.Enqueue(outputVal);
                            if (outputVal != 0)
                            {
                                outputVal += 1;
                            }
                            nextInstruction += 1;
                            break;
                        }
                }
                instructionPointerIndex += nextInstruction;
                previousInstruction = instruction;
                instructionCounter++;
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
            computer.Run(new Queue<int>(), new Queue<int>());

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
                    computer.Run(new Queue<int>(), new Queue<int>());
                }
            }
            int val = 100 * computer.ValueAt(1) + computer.ValueAt(2);
            Assert.Equal(2552, val);
        }
    }
}

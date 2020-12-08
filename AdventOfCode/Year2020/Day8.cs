using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Year2020
{
    public enum Operation
    {
        NOP,
        ACC,
        JMP,
        TERM
    }

    public class Instruction
    {
        public Instruction(Operation op, int argument)
        {
            Op = op;
            Argument = argument;
            RunCount = 0;
        }

        public Operation Op { get; }
        public int Argument { get; }
        public int RunCount { get; set; }
    }

    public class Computer
    {
        private List<Instruction> instructions;
        private int index;

        public int Accumulator { get; private set; }

        public Instruction Instruction { get; private set; }

        public Computer(IEnumerable<Instruction> instructions)
        {
            this.instructions = instructions.ToList();
            Instruction = this.instructions[index];
        }

        public void RunUntilLoop()
        {
            Instruction head;
            while((head = instructions[index]).RunCount < 1)
            {
                Instruction = head;
                switch (head.Op)
                {
                    case Operation.ACC:
                        {
                            Accumulator += head.Argument;
                            index++;
                            head.RunCount++;
                            break;
                        }
                    case Operation.NOP:
                        {
                            index++;
                            head.RunCount++;
                            break;
                        }
                    case Operation.JMP:
                        {
                            head.RunCount++;
                            index += head.Argument;
                            break;
                        }
                }
            }
        }

        public void RunUntilLoopOrTerm()
        {
            Instruction head;
            while ((head = instructions[index]).RunCount < 1)
            {
                Instruction = head;
                switch (head.Op)
                {
                    case Operation.ACC:
                        {
                            Accumulator += head.Argument;
                            index++;
                            head.RunCount++;
                            break;
                        }
                    case Operation.NOP:
                        {
                            index++;
                            head.RunCount++;
                            break;
                        }
                    case Operation.JMP:
                        {
                            head.RunCount++;
                            index += head.Argument;
                            break;
                        }
                    case Operation.TERM:
                        { 
                            head.RunCount++;
                            break;
                        }
                }
            }
        }
    }

    // https://adventofcode.com/2020/day/8
    public class Day8
    {
        private static Regex instructionParser = new Regex(@"(\w\w\w) ([+\-]\d+)", RegexOptions.Compiled);
        private readonly ITestOutputHelper output;

        public Day8(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void Part1()
        {
            var computer = new Computer(ReadInstructions());

            computer.RunUntilLoop();

            Assert.Equal(1810, computer.Accumulator);
        }

        [Fact]
        public void Part2()
        {
            var instructions = ReadInstructions().ToList();
            Computer computer = null;
            var stop = false;
            for (int index = 0; !stop && index < instructions.Count - 1; index++)
            {
                switch(instructions[index].Op)
                {
                    case Operation.JMP:
                        {
                            var holdInstruction = instructions[index];
                            instructions[index] = new Instruction(Operation.NOP, holdInstruction.Argument);
                            computer = new Computer(instructions);
                            computer.RunUntilLoopOrTerm();
                            stop = computer.Instruction.Op == Operation.TERM;
                            instructions[index] = holdInstruction;
                            ResetRunCount(instructions);
                            break;
                        }
                    case Operation.NOP:
                        {
                            var holdInstruction = instructions[index];
                            instructions[index] = new Instruction(Operation.JMP, holdInstruction.Argument);
                            computer = new Computer(instructions);
                            computer.RunUntilLoopOrTerm();
                            stop = computer.Instruction.Op == Operation.TERM;
                            instructions[index] = holdInstruction;
                            ResetRunCount(instructions);
                            break;
                        }
                }
            }

            Assert.Equal(969, computer.Accumulator);
        }

        private static void ResetRunCount(IEnumerable<Instruction> instructions)
        {
            foreach(var instruction in instructions)
            {
                instruction.RunCount = 0;
            }
        }

        IEnumerable<Instruction> ReadInstructions()
        {
            List<Bag> bags = new List<Bag>();
            var stream = InputClient.GetFileStream(2020, 8, "");
            using var reader = new StreamReader(stream);
            string input;

            while ((input = reader.ReadLine()) != null)
            {
                var match = instructionParser.Match(input);
                if (match.Success && match.Groups.Count == 3)
                {
                    yield return new Instruction(
                        Enum.Parse<Operation>(match.Groups[1].Value.ToUpper()),
                        int.Parse(match.Groups[2].Value)
                    );
                }
            }

            yield return new Instruction(Operation.TERM, 0);
        }
    }
}

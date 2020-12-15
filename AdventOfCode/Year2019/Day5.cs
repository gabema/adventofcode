using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Year2019
{
    public class Day5
    {
        private readonly ITestOutputHelper output;

        public Day5(ITestOutputHelper output)
        {
            this.output = output;
        }

        private IEnumerable<int> ReadComputer()
        {
            var inputReader = new StreamReader(InputClient.GetFileStream(2019, 5, ""));

            Day2Lexer lexer = new Day2Lexer();

            var tokens = lexer.Tokenize(inputReader.ReadToEnd());
            return tokens.Where(t => t.TokenType == Day2Item.Value).Select(t => int.Parse(t.Value));
        }

        [Fact]
        public void PartA()
        {
            var computer = new OpCodeComputer(ReadComputer().ToList());
            var inputs = new Queue<int>();
            var outputs = new Queue<int>();
            inputs.Enqueue(1);
            computer.Run(inputs, outputs);

            Assert.Equal(9706670, computer.ValueAt(0));
        }

        [Fact]
        public void PartB()
        {
        }
    }
}

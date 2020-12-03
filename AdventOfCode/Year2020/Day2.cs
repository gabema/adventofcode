using Advent2020;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Year2020
{
    public class PasswordChecker
    {
        public int LowNum { get; set; }
        public int HighNum { get; set; }
        public char PasswordChar { get; set; }
        public string Password { get; set; }
        public bool Check()
        {
            int numChars = Password.ToCharArray().Where(c => c == PasswordChar).Count();
            return numChars >= LowNum && numChars <= HighNum;
        }
    }

    public class PasswordCheckerTwo
    {
        public int PositionA { get; set; }
        public int PositionB { get; set; }
        public char PasswordChar { get; set; }
        public string Password { get; set; }
        public bool Check()
        {
            if (Password.Length < PositionA) return false;

            return ((Password[PositionA - 1] == PasswordChar && Password[PositionB - 1] != PasswordChar)
                || (Password[PositionA - 1] != PasswordChar && Password[PositionB - 1] == PasswordChar));
        }
    }

    public class Day2
    {
        private readonly ITestOutputHelper output;

        public Day2(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void PartA()
        {
            var groups = InputClient.GetRegularExpressionRows(2020, 2, "", @"(\d+)-(\d+) (.): (.*)");
            var validPasswordCounter = 0;
            foreach (var group in groups)
            {
                var passwordChecker = new PasswordChecker();
                passwordChecker.LowNum = int.Parse(group[1].Value);
                passwordChecker.HighNum = int.Parse(group[2].Value);
                passwordChecker.PasswordChar = group[3].Value[0];
                passwordChecker.Password = group[4].Value;
                if (passwordChecker.Check()) validPasswordCounter++;
            }
            Assert.Equal(582, validPasswordCounter);
        }

        [Fact]
        public void PartB()
        {
            var groups = InputClient.GetRegularExpressionRows(2020, 2, "", @"(\d+)-(\d+) (.): (.*)");
            var validPasswordCounter = 0;
            foreach (var group in groups)
            {
                var passwordChecker = new PasswordCheckerTwo();
                passwordChecker.PositionA = int.Parse(group[1].Value);
                passwordChecker.PositionB = int.Parse(group[2].Value);
                passwordChecker.PasswordChar = group[3].Value[0];
                passwordChecker.Password = group[4].Value;
                if (passwordChecker.Check()) validPasswordCounter++;
            }

            // Not right 609
            // Not right 640
            Assert.Equal(729, validPasswordCounter);
        }
    }
}

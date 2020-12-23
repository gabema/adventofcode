using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Year2020
{
    public enum Day18Tokens
    {
        Number,
        Plus,
        Multiply,
        LeftParen,
        RightParen,
        Terminator
    }

    public class ExpressionLexer : SimpleLexer<Day18Tokens>
    {
        private static readonly Tuple<Day18Tokens, string>[] Exp = new Tuple<Day18Tokens, string>[] {
            Tuple.Create(Day18Tokens.Number, @"^\d+"),
            Tuple.Create(Day18Tokens.Plus, @"^\+"),
            Tuple.Create(Day18Tokens.Multiply, @"^\*"),
            Tuple.Create(Day18Tokens.LeftParen, @"^\("),
            Tuple.Create(Day18Tokens.RightParen, @"^\)"),
            Tuple.Create(Day18Tokens.Terminator, @""),
        };

        public ExpressionLexer() : base(Exp)
        { }
    }

    public class NewMathCalculator
    {
        public ulong Memory { get; private set; }

        public ulong Evaluation { get; private set; }

        public ulong NewMath(IList<Token<Day18Tokens>> infixExpression)
        {
            var postfix = ConvertToPostFix(infixExpression, false);
            Evaluation = CalculatePostfix(postfix);
            return Evaluation;
        }

        public ulong NewMathWithPrecendence(IList<Token<Day18Tokens>> infixExpression)
        {
            var postfix = ConvertToPostFix(infixExpression, true);
            Evaluation = CalculatePostfix(postfix);
            return Evaluation;
        }

        public void Clear() { Evaluation = default; }

        public void AddToMemory()
        {
            Memory += Evaluation;
        }

        private ulong CalculatePostfix(IList<Token<Day18Tokens>> postfix)
        {
            Stack<ulong> values = new Stack<ulong>();
            foreach(var token in postfix)
            {
                switch(token.TokenType)
                {
                    case Day18Tokens.Number:
                        {
                            values.Push(ulong.Parse(token.Value));
                            break;
                        }
                    case Day18Tokens.Multiply:
                        {
                            values.Push(values.Pop() * values.Pop());
                            break;
                        }
                    case Day18Tokens.Plus:
                        {
                            values.Push(values.Pop() + values.Pop());
                            break;
                        }
                }
            }

            return values.Pop();
        }

        private IList<Token<Day18Tokens>> ConvertToPostFix(IList<Token<Day18Tokens>> infix, bool precedence)
        {
            var postfix = new List<Token<Day18Tokens>>();
            var stack = new Stack<Token<Day18Tokens>>();
            infix.Insert(0, new Token<Day18Tokens>(Day18Tokens.LeftParen));
            infix.Add(new Token<Day18Tokens>(Day18Tokens.RightParen));
            foreach (var token in infix)
            {
                switch(token.TokenType)
                {
                    case Day18Tokens.Number:
                        {
                            // operand encountered
                            postfix.Add(token);
                            break;
                        }
                    case Day18Tokens.LeftParen:
                        {
                            stack.Push(token);
                            break;
                        }
                    case Day18Tokens.Multiply:
                        {
                            // operator lower precedence
                            var a = stack.Count == 0 ? default : stack.Pop();
                            while(a != null && (a.TokenType == Day18Tokens.Multiply || a.TokenType == Day18Tokens.Plus))
                            {
                                postfix.Add(a);
                                a = stack.Count == 0 ? default : stack.Pop();
                            }

                            if (a != default) stack.Push(a);
                            stack.Push(token);
                            break;
                        }
                    case Day18Tokens.Plus:
                        {
                            // operator higher precedence
                            var a = stack.Count == 0 ? default : stack.Pop();
                            while (a != null && 
                                ((precedence && a.TokenType == Day18Tokens.Plus)
                                || (!precedence && a.TokenType == Day18Tokens.Multiply || a.TokenType == Day18Tokens.Plus)))
                            {
                                postfix.Add(a);
                                a = stack.Count == 0 ? default : stack.Pop();
                            }
                            if (a != default) stack.Push(a);
                            stack.Push(token);
                            break;
                        }
                    case Day18Tokens.RightParen:
                        {
                            Token<Day18Tokens> t;
                            while((t = stack.Pop()).TokenType != Day18Tokens.LeftParen)
                            {
                                postfix.Add(t);
                            }
                            break;
                        }
                }
            }
            return postfix;
        }
    }

    // https://adventofcode.com/2020/day/18
    public class Day18
    {
        private ITestOutputHelper output;

        public Day18(ITestOutputHelper output)
        {
            this.output = output;
        }

        private IEnumerable<IList<Token<Day18Tokens>>> ReadExpressions()
        {
            using var reader = new StreamReader(InputClient.GetFileStream(2020, 18, ""));

            var lexer = new ExpressionLexer();
            string input;
            while((input = reader.ReadLine()) != null)
            {
                yield return lexer.Tokenize(input).ToList();
            }
        }

        [Fact]
        public void Part1()
        {
            var expressions = ReadExpressions();
            var calculator = new NewMathCalculator();
            foreach(var expression in expressions)
            {
                calculator.NewMath(expression);
                calculator.AddToMemory();
            }
            Assert.Equal(31142189909908UL, calculator.Memory);
        }

        [Fact]
        public void Part2()
        {
            var expressions = ReadExpressions();
            var calculator = new NewMathCalculator();
            foreach (var expression in expressions)
            {
                calculator.NewMathWithPrecendence(expression);
                calculator.AddToMemory();
            }
            Assert.Equal(323912478287549UL, calculator.Memory);
        }
    }
}

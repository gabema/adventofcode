using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Year2021;

// https://adventofcode.com/2021/day/2
public class DayLexer
{
    private readonly ITestOutputHelper _output;

    public enum DayNItems
    {
        Item1,
        Item2,
        Terminator
    }

    public class DayNLexer : SimpleLexer<DayNItems>
    {
        private static readonly List<Tuple<DayNItems, string>> NewList = new List<Tuple<DayNItems, string>> {
            Tuple.Create(DayNItems.Item1, @"^pid:\S+"),
            Tuple.Create(DayNItems.Item2, @"^cid:\d+"),
            Tuple.Create(DayNItems.Terminator, @""),
        };

        public DayNLexer() : base(NewList)
        { }
    }

    public class LexerObject
    {
        public string Item1 { get; set; }
        public int Item2 { get; set; }
    }

    private IEnumerable<LexerObject> GetObjects(int year, int day, string s)
    {
        using var reader = InputClient.GetFileStreamReader(year, day, s);
        var input = reader.ReadLine();
        var lexer = new DayNLexer();
        var obj = new LexerObject();
        while (input != null)
        {
            if (input.Trim() == "")
            {
                yield return obj;
                obj = new LexerObject();
            }
            else
            {
                IEnumerable<Token<DayNItems>> tokens = lexer.Tokenize(input);
                foreach (var item in tokens)
                {
                    switch (item.TokenType)
                    {
                        case DayNItems.Item1:
                            obj.Item1 = item.Value;
                            break;
                        case DayNItems.Item2:
                            obj.Item2 = int.Parse(item.Value);
                            break;
                    }
                }
            }
        }
    }

    public DayLexer(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void PartA()
    {
        var objects = GetObjects(2021,1,"Sample");
        Assert.True(false);
    }

    [Fact]
    public void PartB()
    {
        Assert.True(false);
    }
}

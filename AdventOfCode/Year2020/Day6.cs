using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Year2020
{
    public class Person
    {
        private HashSet<char> answers;

        public Person()
        {
            answers = new HashSet<char>();
        }

        public void AddAnswer(char answer)
        {
            answers.Add(answer);
        }

        public void AddAnswer(IEnumerable<char> a)
        {
            answers.UnionWith(a);
        }

        public IEnumerable<char> Answers { get => answers; }
    }

    public class Group
    {
        private HashSet<char> collectiveAnswers;
        private HashSet<char> commonAnswers;

        public Group()
        {
            collectiveAnswers = new HashSet<char>();
            commonAnswers = null;
        }

        public void AddPerson(Person p)
        {
            collectiveAnswers.UnionWith(p.Answers);

            if (commonAnswers == null)
            {
                commonAnswers = new HashSet<char>(p.Answers);
            }
            else
            {
                commonAnswers.IntersectWith(p.Answers);
            }
        }

        public IEnumerable<char> Answers { get => collectiveAnswers; }
        public IEnumerable<char> CommonAnswers { get => commonAnswers ?? new HashSet<char>(); }
    }

    // https://adventofcode.com/2020/day/6
    public class Day6
    {
        private readonly ITestOutputHelper output;

        public Day6(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void Part1()
        {
            var groups = ReadGroups();
            var numAnswers = groups.Sum(g => g.Answers.Count());
            Assert.Equal(7027, numAnswers);
        }

        [Fact]
        public void Part2()
        {
            var groups = ReadGroups();
            var numAnswers = groups.Sum(g => g.CommonAnswers.Count());
            Assert.Equal(3579, numAnswers);
        }

        private IEnumerable<Group> ReadGroups()
        {
            using var inputStream = new StreamReader(InputClient.GetFileStream(2020, 6, ""));

            string input;
            Group g = new Group();
            while ((input = inputStream.ReadLine()) != null)
            {
                if (input.Length == 0)
                {
                    yield return g;
                    g = new Group();
                }
                else
                {
                    Person p = new Person();
                    p.AddAnswer(input);
                    g.AddPerson(p);
                }
            }
            yield return g;
        }
    }
}

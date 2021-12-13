using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Year2021;

// https://adventofcode.com/2021/day/12
public class Day12
{
    private readonly ITestOutputHelper _output;
    public Day12(ITestOutputHelper output)
    {
        _output = output;
    }

    class Node
    {
        public Node(string name)
        {
            Name = name;
            IsLargeCave = name[0] < 'a';
            Nodes = new List<Node>();
        }

        public string Name { get; private set; }
        public bool IsLargeCave { get; private set; }

        public List<Node> Nodes { get; private set; }

        public override bool Equals(object obj)
        {
            return (obj as Node)?.Name == Name;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name);
        }
    }

    class Passage
    {
        public Node Start { get; private set; }
        public Node End { get; private set; }

        public Passage()
        {
            Start = GetOrCreateNode("start");
            End = GetOrCreateNode("end");
        }

        public Dictionary<string, Node> NodeNames { get; private init; } = new Dictionary<string, Node>();

        public Node GetOrCreateNode(string name)
        {
            if (NodeNames.ContainsKey(name)) return NodeNames[name];
            var node = new Node(name);
            NodeNames.Add(name, node);
            return node;
        }

        internal int NumPathsFromStartToEnd()
        {
            var numPaths = 0;
            foreach(var node in Start.Nodes)
            {
                var visitedSmallCaves = (new Node[] { Start }).ToHashSet();
                numPaths += NumPathsFromNodeToEnd(node, visitedSmallCaves);
            }
            return numPaths;
        }

        private int NumPathsFromNodeToEnd(Node node, HashSet<Node> visitedSmallCaves)
        {
            if (!node.IsLargeCave && visitedSmallCaves.Contains(node)) return 0;
            if (node == End) return 1;

            if (!node.IsLargeCave) visitedSmallCaves.Add(node);

            var numPaths = 0;
            foreach (var innerNode in node.Nodes)
            {
                numPaths += NumPathsFromNodeToEnd(innerNode, new HashSet<Node>(visitedSmallCaves));
            }

            return numPaths;
        }

        internal int NumPathsFromStartToEndOneDouble()
        {
            var numPaths = 0;
            foreach (var node in Start.Nodes)
            {
                var visitedSmallCaves = (new Node[] { Start }).ToHashSet();
                numPaths += NumPathsFromNodeToEndDouble(node, visitedSmallCaves, null);
            }
            return numPaths;
        }

        private int NumPathsFromNodeToEndDouble(Node node, HashSet<Node> visitedSmallCaves, Node? doubleSmallCave)
        {
            if (node == End) return 1;

            if (!node.IsLargeCave)
            {
                if (!visitedSmallCaves.Contains(node))
                {
                    visitedSmallCaves.Add(node);
                }
                else if (doubleSmallCave is null && node != Start)
                {
                    doubleSmallCave = node;
                }
                else
                {
                    // small cave that has already been visited AND we've already visited one small cave twice
                    return 0;
                }
            }

            var numPaths = 0;
            foreach (var innerNode in node.Nodes)
            {
                numPaths += NumPathsFromNodeToEndDouble(innerNode, new HashSet<Node>(visitedSmallCaves), doubleSmallCave);
            }

            return numPaths;
        }
    }

    private Passage ReadPassage(string variant)
    {
        var passage = new Passage();

        var groups = InputClient.GetRegularExpressionRows(2021, 12, variant, @"(\w+)-(\w+)");
        foreach (var group in groups)
        {
            var left = passage.GetOrCreateNode(group[1].Value);
            var right = passage.GetOrCreateNode(group[2].Value);
            left.Nodes.Add(right);
            right.Nodes.Add(left);
        }

        return passage;
    }

    [Fact]
    public void PartA()
    {
        var passage = ReadPassage("");
        // 226 is too low (duh my ReadPassage had Sample hardcoded)
        Assert.Equal(4549, passage.NumPathsFromStartToEnd());
    }

    [Fact]
    public void PartB()
    {
        var passage = ReadPassage("");
        Assert.Equal(120535, passage.NumPathsFromStartToEndOneDouble());
    }
}

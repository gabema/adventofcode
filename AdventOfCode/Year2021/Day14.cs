using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Year2021;

// https://adventofcode.com/2021/day/14
public class Day14
{
    private readonly ITestOutputHelper _output;
    public Day14(ITestOutputHelper output)
    {
        _output = output;
    }

    record Pair(char Left, char Right);

    record Data(char[] Template, Dictionary<Pair, char> InsertionRules);

    record Data2(Dictionary<string, ulong> Counts, Dictionary<string, char> InsertionRules);

    private static Data ReadContent(string variant)
    {
        using var reader = InputClient.GetFileStreamReader(2021, 14, variant);
        var polymerTemplate = reader.ReadLine().ToCharArray();

        // blank line
        reader.ReadLine();

        string pairInsertion;
        var pairs = new Dictionary<Pair, char>();
        while((pairInsertion = reader.ReadLine()) != null)
        {
            var parts = pairInsertion.Split(' ');
            pairs.Add(new Pair(parts[0][0], parts[0][1]), parts[2][0]);
        }
        return new Data(polymerTemplate, pairs);
    }

    private static Data2 ReadContent2(string variant)
    {
        using var reader = InputClient.GetFileStreamReader(2021, 14, variant);
        var counts = reader.ReadLine().Chunk(2).ToArray().Aggregate(new Dictionary<string, ulong>(), (agg, v) => {
            var str = new string(v);
            if (agg.ContainsKey(str)) agg[str]++;
            else agg.Add(str, 1UL);
            return agg;
        });

        // blank line
        reader.ReadLine();

        string pairInsertion;
        var pairs = new Dictionary<string, char>();
        while ((pairInsertion = reader.ReadLine()) != null)
        {
            var parts = pairInsertion.Split(' ');
            pairs.Add(parts[0], parts[2][0]);
        }
        return new Data2(counts, pairs);
    }

    [Fact]
    public void PartA()
    {
        var data = ReadContent("");
        IEnumerable<char> polymer = data.Template;

        for (int generations = 0; generations < 10; generations++)
        {
            polymer = ApplyInsertion(polymer, data.InsertionRules);
        }
        Dictionary<char, int> charCounter = polymer.Aggregate(new Dictionary<char, int>(), (counter, v) => {
            if (counter.ContainsKey(v))
            {
                counter[v] = counter[v] + 1;
            } else
            {
                counter.Add(v, 1);
            }
            return counter;
        });
        var min = charCounter.Min(kv => kv.Value);
        var max = charCounter.Max(kv => kv.Value);
        // Your answer is too high: 19457
        Assert.Equal(2447, max - min);
    }

    private static IEnumerable<char> ApplyInsertion(IEnumerable<char> polymer, Dictionary<Pair, char> insertionRules)
    {
        char? left = null;
        foreach(var e in polymer)
        {
            if (left == null)
            {
                left = e;
                continue;
            }
            var right = e;
            char addChar;
            if (insertionRules.TryGetValue(new Pair(left.Value, right), out addChar))
            {
                yield return left.Value;
                yield return addChar;
            }
            else
            {
                yield return left.Value;
            }
            left = right;
        }
        if (left != null)
        {
            yield return left.Value;
        }
    }

    [Fact]
    public void PartB()
    {
        var data = ReadContent2("Sample");
        var counts = data.Counts;

        for (int generations = 0; generations < 10; generations++)
        {
            counts = ApplyInsertion2(counts, data.InsertionRules);
        }

        var singleCharCounts = counts.Aggregate(new Dictionary<char, ulong>(), (agg, v) => {
            var c = v.Key[0];
            var num = v.Value;
            if (agg.ContainsKey(c)) agg[c] += num;
            else agg.Add(c, num);

            c = v.Key[1];
            if (agg.ContainsKey(c)) agg[c]+= num;
            else agg.Add(c, num);

            return agg;
        });

        var min = singleCharCounts.Min(kv => kv.Value);
        var max = singleCharCounts.Max(kv => kv.Value);

        Assert.Equal((ulong)2188189693529, max - min);
    }

    private Dictionary<string, ulong> ApplyInsertion2(Dictionary<string, ulong> counts, Dictionary<string, char> insertionRules)
    {
        var builder = new StringBuilder();
        var newCounts = new Dictionary<string, ulong>();
        char? spair = null;
        foreach(var pair in counts)
        {
            var resultChar = insertionRules[pair.Key];

            if (spair != null)
            {

            }

            var str = builder.Clear().Append(pair.Key[0]).Append(resultChar).ToString();
            if (newCounts.ContainsKey(str)) newCounts[str] += pair.Value;
            else newCounts.Add(str, pair.Value);

            str = builder.Clear().Append(resultChar).Append(pair.Key[1]).ToString();
            if (newCounts.ContainsKey(str)) newCounts[str] += pair.Value;
            else newCounts.Add(str, pair.Value);
        }
        return newCounts;
    }
}

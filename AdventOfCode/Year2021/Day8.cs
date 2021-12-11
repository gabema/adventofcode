using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Year2021;

// https://adventofcode.com/2021/day/8
public class Day8
{
    public class Content
    {
        public List<string> Signals { get; init; } = new List<string>();
        public List<string> Output { get; init; } = new List<string>();
    }

    private readonly ITestOutputHelper _output;
    public Day8(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void PartA()
    {
        var content = ReadContent("");
        Assert.Equal(375, content.Aggregate(0, (agg, v) => {
            var count = v.Output.Where(e => e.Length == 2 || e.Length == 3 || e.Length == 4 || e.Length == 7).Count();
            return agg + count;
        }));
    }

    /*
  0:      1:      2:      3:      4:
 aaaa    ....    aaaa    aaaa    ....
b    c  .    c  .    c  .    c  b    c
b    c  .    c  .    c  .    c  b    c
 ....    ....    dddd    dddd    dddd
e    f  .    f  e    .  .    f  .    f
e    f  .    f  e    .  .    f  .    f
 gggg    ....    gggg    gggg    ....
abcefg   cf      acdeg    acdfg   bcdf

  5:      6:      7:      8:      9:
 aaaa    aaaa    aaaa    aaaa    aaaa
b    .  b    .  .    c  b    c  b    c
b    .  b    .  .    c  b    c  b    c
 dddd    dddd    ....    dddd    dddd
.    f  e    f  .    f  e    f  .    f
.    f  e    f  .    f  e    f  .    f
 gggg    gggg    ....    gggg    gggg
abdfg   abdefg   acf   abcdefg  abcdfg

7 8 abcdefg (len 7)

2 1   c f   (len 2) (learn c & f)
3 7 a c f   (len 3) (learn a)
4 4  bcdf   (len 4) (learn b & d)

5 2 a cde g (len 5) (diff ce) (know c from 1, learn e)
  3 a cd fg (len 5) (diff cf) (know from 2)
  5 ab d fg (len 5) (diff bf) (know b from 4, learn b)

6 0 abc efg (len 6) (diff d) (know b & d from 4 missing one of those)
  6 ab defg (len 6) (diff c) (all remaining)
  9 abcd fg (len 6) (diff e) (know e from 2)

0 = (input) => len 6 & IN SET(B&D)
1 = (input) => len 2
2 = (input) => len 5 && Remove ADG AND ONE IN SET(C&F) AND NONE IN SET(B&D)
3 = (input) => len 5 && Remove ADG AND BOTH IN SET(C&F)
4 = (input) => len 4
5 = (input) => len 5 && Remove ADG AND ONE IN SET(C&F) AND ONE IN SET(B&D)
6 = (input) => len 6 && NOT IN SET(D).
7 = (input) => len 3
8 = (input) => len 7
9 = (input) => len 6 & NOT IN SET(B&D) AND NOT IN SET(C & F)
     */
    [Fact]
    public void PartB()
    {
        var content = ReadContent("");
        Assert.Equal(1019355, content.Aggregate(0, (agg, entry) => {
            HashSet<char>? BD = null;
            HashSet<char>? ADG = null;
            HashSet<char>? A = null;
            HashSet<char>? ABFG = null;
            HashSet<char>? CF = null;

            List<string> sampleContent = entry.Signals;

            int i = 0;
            while (BD == null || A == null || ADG == null || CF == null || ABFG == null || ADG == null || ABFG.Count != 4 || ADG.Count != 3)
            {
                if (CF == null && sampleContent[i].Length == 2) { CF = sampleContent[i].ToHashSet(); }
                if (A == null && CF != null && sampleContent[i].Length == 3) { A = sampleContent[i].ToHashSet(); A.ExceptWith(CF); }
                if (BD == null && CF != null && sampleContent[i].Length == 4) { BD = sampleContent[i].ToHashSet(); BD.ExceptWith(CF); }
                if (sampleContent[i].Length == 6)
                    if (ABFG == null) ABFG = sampleContent[i].ToHashSet();
                    else if (ABFG.Count != 4) { ABFG.IntersectWith(sampleContent[i]); }
                if (sampleContent[i].Length == 5)
                    if (ADG == null) ADG = sampleContent[i].ToHashSet();
                    else if (ADG.Count != 3) { ADG.IntersectWith(sampleContent[i]); }

                i = (i + 1) % sampleContent.Count;
            }

            var CheckForNumbers = new List<IsNumber>{
                v => v.Count == 6 && BD.Intersect(v).Count() == 1, // 0
                v => v.Count == 2, // 1
                v => { // 2
                    if (v.Count != 5) return false;
                    var leftovers = v.Except(ADG);
                    return leftovers.Intersect(BD).Count() == 0 && leftovers.Intersect(CF).Count() == 1;
                },
                v => v.Count == 5 && v.Except(ADG).Intersect(CF).Count() == 2,  // 3
                v => v.Count == 4, // 4
                v => { // 5
                    if (v.Count != 5) return false;
                    var leftovers = v.Except(ADG);
                    return leftovers.Except(BD).Count() == 1 && leftovers.Except(CF).Count() == 1;
                },
                v => v.Count == 6 && CF.Intersect(v).Count() == 1, // 6
                v => v.Count == 3, // 7
                v => v.Count == 7, // 8
                v => { // 9
                    if (v.Count != 6) return false;
                    var leftovers = v.Except(ABFG).ToHashSet();
                    return leftovers.Intersect(BD).Count() == 1 && leftovers.Intersect(CF).Count() == 1;
                }
            };

            int val = 0;
            for(i =0; i<entry.Output.Count; i++)
            {
                var v = entry.Output[i];
                bool found = false;
                ISet<char> numSet = v.ToHashSet();
                for (var n=0; n<10 && !found; n++)
                {
                    if (CheckForNumbers[n](numSet))
                    {
                        found = true;
                        val = val * 10 + n;
                    }
                }
                //if (!found)
                //{
                //    _output.WriteLine($"Not deciphered: <{v}>");
                //}
            }

            return agg + val;
        }));
    }

    delegate bool IsNumber(ISet<char> value);

    private IEnumerable<Content> ReadContent(string v)
    {
        using var reader = InputClient.GetFileStreamReader(2021, 8, v);
        string line;
        while ((line = reader.ReadLine()) != null)
        {
            var parts = line.Split('|');
            yield return new Content
            {
                Signals = parts[0].Trim().Split(' ').ToList(),
                Output = parts[1].Trim().Split(' ').ToList(),
            };
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode2018.Utils;
using JetBrains.Annotations;
using TypeParser;

namespace AdventOfCode2018.Days.Day01;

[UsedImplicitly]
public class Day01 : AdventOfCode<long, IReadOnlyList<long>>
{
    public override IReadOnlyList<long> Parse(string input) => input
        .Lines().Select(line => Convert.ToInt64(line)).ToList();

    // [TestCase(Input.Example, 24000)]
    [TestCase(Input.File, 513)]
    public override long Part1(IReadOnlyList<long> input)
    {
        return input.Sum();
    }

    // [TestCase(Input.Example, 45000)]
    [TestCase(Input.File, 287)]
    public override long Part2(IReadOnlyList<long> input)
    {
        var current = 0L;
        var seen = new HashSet<long>();
        while (true)
        {
            foreach (var item in input)
            {
                current += item;
                if (!seen.Add(current))
                {
                    return current;
                }
            }
        }
    }
}
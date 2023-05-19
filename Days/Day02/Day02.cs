using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode2018.Utils;
using JetBrains.Annotations;
using TypeParser;

namespace AdventOfCode2018.Days.Day02;

[UsedImplicitly]
public class Day02 : AdventOfCode<string, IReadOnlyList<string>>
{
    public override IReadOnlyList<string> Parse(string input) => input
        .Lines();

    [TestCase(Input.Example, "12")]
    [TestCase(Input.File, "6916")]
    public override string Part1(IReadOnlyList<string> input)
    {
        return $"{input.Count(line => line.GroupToDictionary().Any(item => item.Value.Count == 2)) *
            input.Count(line => line.GroupToDictionary().Any(item => item.Value.Count == 3))}";
    }

    [TestCase(Input.Raw, "fgij", Raw=@"abcde
fghij
klmno
pqrst
fguij
axcye
wvxyz")]
    [TestCase(Input.File, "oeylbtcxjqnzhgyylfapviusr")]
    public override string Part2(IReadOnlyList<string> input)
    {
        foreach(var item in input)
        {
            foreach(var item2 in input)
            {
                var mismatches = item.Zip(item2).WithIndices().Where(it => it.Value.Item1 != it.Value.Item2).ToList();
                if (mismatches.Count == 1)
                {
                    return item.Substring(0, mismatches.First().Index) + item.Substring(mismatches.First().Index + 1);
                }
            }
        }
        throw new ApplicationException();
    }
}
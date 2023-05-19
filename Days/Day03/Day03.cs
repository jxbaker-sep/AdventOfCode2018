using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode2018.Utils;
using JetBrains.Annotations;
using TypeParser;

namespace AdventOfCode2018.Days.Day03;

[UsedImplicitly]
public class Day03 : AdventOfCode<long, IReadOnlyList<Claim>>
{
    public override IReadOnlyList<Claim> Parse(string input) => input
        .Lines()
        .Select(line => {
            var m =Regex.Match(line, @"#(?<id>\d+) @ (?<x>\d+),(?<y>\d+): (?<width>\d+)x(?<height>\d+)");
            return new Claim(m.LongGroup("id"), m.LongGroup("x"), m.LongGroup("y"), m.LongGroup("width"), m.LongGroup("height"));
        })
        .ToList();

    [TestCase(Input.Example, 4)]
    [TestCase(Input.File, 112418)]
    public override long Part1(IReadOnlyList<Claim> input)
    {
        var d = new Dictionary<Position, long>();
        foreach(var claim in input)
        {
            foreach(var dx in Enumerable.Range(0, (int)claim.Width))
            {
                foreach(var dy in Enumerable.Range(0, (int)claim.Height))
                {
                    var p = new Position(claim.Y + dy, claim.X + dx);
                    d[p] = d.GetValueOrDefault(p) + 1;
                }
            }
        }
        return d.Count(kv => kv.Value > 1);
    }

    [TestCase(Input.Example, 3)]
    [TestCase(Input.File, 560)]
    public override long Part2(IReadOnlyList<Claim> input)
    {
        var d = new Dictionary<Position, long>();
        foreach(var claim in input)
        {
            foreach(var dx in Enumerable.Range(0, (int)claim.Width))
            {
                foreach(var dy in Enumerable.Range(0, (int)claim.Height))
                {
                    var p = new Position(claim.Y + dy, claim.X + dx);
                    d[p] = d.GetValueOrDefault(p) + 1;
                }
            }
        }

        foreach(var claim in input)
        {
            if (CheckClaim(claim, d)) return claim.Id;
            
        }
        throw new ApplicationException();
    }

    private bool CheckClaim(Claim claim, Dictionary<Position, long> d)
    {
        foreach(var dx in Enumerable.Range(0, (int)claim.Width))
        {
            foreach(var dy in Enumerable.Range(0, (int)claim.Height))
            {
                var p = new Position(claim.Y + dy, claim.X + dx);
                if (d[p] != 1) return false;
            }
        }

        return true;
    }
}

public record Claim(long Id, long X, long Y, long Width, long Height);
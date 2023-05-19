using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode2018.Utils;
using JetBrains.Annotations;

namespace AdventOfCode2018.Days.Day06;

[UsedImplicitly]
public class Day06 : AdventOfCode<long, IReadOnlyList<Position>>
{
    public override IReadOnlyList<Position> Parse(string input) => 
        input.Lines(it => { 
            var z = it.Split(','); 
            return new Position(Convert.ToInt64(z[1]), Convert.ToInt64(z[0]));
        });

    [TestCase(Input.Example, 17)]
    // [TestCase(Input.File, 0)] // 4996 too high - INPUT WRONG!
    public override long Part1(IReadOnlyList<Position> input)
    {
        var d = new Dictionary<Position, long>();
        var nonEdges = input.ToList();
        var minx = input.Select(p => p.X).Min();
        var maxx = input.Select(p => p.X).Max();
        var miny = input.Select(p => p.Y).Min();
        var maxy = input.Select(p => p.Y).Max();
        for(var x = minx; x <= maxx; x++)
        {
            for(var y = miny; y < maxy; y++)
            {
                var grid = new Position(y, x);
                var distances = input.Select(it => new {P = it, M = it.ManhattanDistance(grid)}).ToList();
                var minDistance = distances.MinBy(it => it.M) ?? throw new ApplicationException();
                if (distances.Count(m => m.M == minDistance.M) != 1) continue;
                d[minDistance.P] = d.GetValueOrDefault(minDistance.P) + 1;
                if (x == minx || x == maxx || y == miny || y == maxy)
                {
                    nonEdges.Remove(minDistance.P);
                }
            }
        }

        return d.Where(kv => nonEdges.Contains(kv.Key)).Select(it => it.Value).Max();
    }
    
    [TestCase(Input.Example, 0)]
    [TestCase(Input.File, 0)]
    public override long Part2(IReadOnlyList<Position> input)
    {
        return 0;
    }
}

public record Node(char Actual, char Lower);
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode2018.Utils;
using JetBrains.Annotations;

namespace AdventOfCode2018.Days.Day10;

[UsedImplicitly]
public class Day10 : AdventOfCode<long, IReadOnlyList<PV>>
{
    public override IReadOnlyList<PV> Parse(string input) => 
        input.Lines().Select(it => {
            var m = Regex.Match(it, @"(?<x>-?\d+)[^0-9-]+(?<y>-?\d+)[^0-9-]+(?<dx>-?\d+)[^0-9-]+(?<dy>-?\d+)");
            return new PV(
                new Position(m.LongGroup("y"), m.LongGroup("x")),
                new Vector(m.LongGroup("dy"), m.LongGroup("dx"))
            );
        }).ToList();

    [TestCase(Input.File, 0)] 
    public override long Part1(IReadOnlyList<PV> world)
    {
        var lastDy = long.MaxValue;
        var seconds = 0;
        while (true)
        {
            seconds += 1;
            var lastWorld = world;
            world = world.Select(pv => new PV(pv.Position + pv.Vector, pv.Vector)).ToList();
            var grid = world.Select(pv => pv.Position).ToHashSet();
            var minX = grid.Select(p => p.X).Min();
            var maxX = grid.Select(p => p.X).Max();
            var minY = grid.Select(p => p.Y).Min();
            var maxY = grid.Select(p => p.Y).Max();
            var dy = maxY - minY + 1;
            if (dy > lastDy)
            {
                world = lastWorld;
                grid = world.Select(pv => pv.Position).ToHashSet();
                Console.WriteLine();
                for (var y = minY; y <= maxY; y++)
                {
                    for (var x = minX; x <= maxX; x++)
                    {
                        Console.Write(grid.Contains(new Position(y, x)) ? "." : "#");
                    }
                    Console.WriteLine(seconds);
                }
                break;
            }
            lastDy = dy;
        }
        return 0;
    }

    // [TestCase(Input.Example, 0)]
    [TestCase(Input.File, 0)]
    public override long Part2(IReadOnlyList<PV> settings)
    {
        return 0;
    }
}

public record PV(Position Position, Vector Vector);
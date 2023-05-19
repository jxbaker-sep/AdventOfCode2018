using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode2018.Utils;
using JetBrains.Annotations;

namespace AdventOfCode2018.Days.Day11;

[UsedImplicitly]
public class Day11 : AdventOfCode<string, long>
{
    public override long Parse(string input) => 
        input.Lines().Select(it => Convert.ToInt64(it)).Single();

    [TestCase(Input.Raw, "33,45", Raw = "18")] 
    [TestCase(Input.Raw, "21,61", Raw = "42")] 
    [TestCase(Input.Raw, "243,17", Raw = "7347")] 
    public override string Part1(long gridSerialNumber)
    {
        var grid = new Dictionary<Position, long>();
        for(var y = 1; y <= 300; y++)
        {
            for (var x = 1; x <= 300; x++)
            {                
                var pl = PowerLevel(x, y, gridSerialNumber);
                for (var dx = 0; dx >= -2; dx--)
                {
                    for (var dy = 0; dy >= -2; dy--)
                    {
                        var p = new Position(y + dy, x + dx);
                        grid[p] = grid.GetValueOrDefault(p) + pl;
                    }
                }
            }
        }
        var item = grid.Where(p => p.Key.X is >= 1 and <= 300 && p.Key.Y is >= 1 and <= 300).MaxBy(kv => kv.Value);
        return $"{item.Key.X},{item.Key.Y}";
    }

    [TestCase(Input.Raw, "90,269,16", Raw = "18")] 
    [TestCase(Input.Raw, "232,251,12", Raw = "42")] 
    [TestCase(Input.Raw, "233,228,12", Raw = "7347")]
    public override string Part2(long gridSerialNumber)
    {
        var grid = new Dictionary<PositionAndGridSize, long>();
        var foundPl = long.MinValue;
        var foundPosition = new PositionAndGridSize(new Position(-1, -1), 0);

        for(var y = 1; y <= 300; y++)
        {
            for (var x = 1; x <= 300; x++)
            {
                var pl = 0L;
                for (var gridSize = 1; gridSize <= Math.Min(300 -y + 1, 300 -x + 1); gridSize++)
                {
                    // Add right column
                    for (var dy = 0; dy < gridSize-1; dy++) pl += PowerLevel(x + gridSize - 1, y + dy, gridSerialNumber);
                    // Add bottom row
                    for (var dx = 0; dx < gridSize-1; dx++) pl += PowerLevel(x + dx, y + gridSize - 1, gridSerialNumber);
                    // Add corner
                    pl += PowerLevel(x + gridSize -1, y + gridSize -1, gridSerialNumber);
                    if (pl > foundPl)
                    {
                        foundPl = pl;
                        foundPosition = new PositionAndGridSize(new Position(y, x), gridSize);
                    }
                }
            }
        }

        return $"{foundPosition.Position.X},{foundPosition.Position.Y},{foundPosition.GridSize}";
    }

    private long PowerLevel(long x, long y, long gridSerialNumber)
    {
        var rackId = x + 10;
        var pl = rackId * y;
        pl += gridSerialNumber;
        pl *= rackId;
        pl = (pl % 1000) / 100;
        pl -= 5;
        return pl;
    }
}

public record PositionAndGridSize(Position Position, int GridSize);
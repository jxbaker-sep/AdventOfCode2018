using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode2018.Utils;
using JetBrains.Annotations;

namespace AdventOfCode2018.Days.Day09;

[UsedImplicitly]
public class Day09 : AdventOfCode<long, GameSettings>
{
    public override GameSettings Parse(string input) => 
        input.Lines().Select(it => {
            var m = Regex.Match(it, @"(?<players>\d+)[^0-9]+(?<points>\d+)");
            return new GameSettings(m.LongGroup("players"), m.LongGroup("points"));
        }).Single();

    [TestCase(Input.Raw, 32, Raw = "9 25")]
    [TestCase(Input.Raw, 8317, Raw = "10 1618")]
    [TestCase(Input.Raw, 146373, Raw = "13 7999")]
    [TestCase(Input.Raw, 2764, Raw = "17 1104")]
    [TestCase(Input.Raw, 54718, Raw = "21 6111")]
    [TestCase(Input.Raw, 37305, Raw = "30 5807")]
    [TestCase(Input.File, 393229)] 
    public override long Part1(GameSettings settings)
    {
        return PlayGame(settings);
    }

    // [TestCase(Input.Example, 0)]
    [TestCase(Input.File, 3273405195L)]
    public override long Part2(GameSettings settings)
    {
        return PlayGame(settings with { LastMarble = settings.LastMarble * 100 });
    }

    private static long PlayGame(GameSettings settings)
    {
        var l = new LinkedList<long>();
        l.AddFirst(0);
        var current = l.First ?? throw new ApplicationException();
        var player = 0L;
        var scores = Enumerable.Repeat(0L, (int)settings.Players).ToArray();
        foreach (var marble in Enumerable.Range(1, (int)settings.LastMarble))
        {
            player = (player + 1) % settings.Players;
            if (marble % 23 == 0)
            {
                scores[player] += marble;
                var temp = current.RollBack(7);
                scores[player] += temp.Value;
                current = temp.RollForward(1);
                l.Remove(temp);
            }
            else
            {
                var newNode = new LinkedListNode<long>(marble);
                l.AddAfter(current.RollForward(1), newNode);
                current = newNode;
            }
            // Console.WriteLine(l.Select(t => $"{t}").Join(" "));
        }
        return scores.Max();
    }

}

public record GameSettings(long Players, long LastMarble);
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode2018.Utils;
using JetBrains.Annotations;
using TypeParser;

namespace AdventOfCode2018.Days.Day05;

[UsedImplicitly]
public class Day05 : AdventOfCode<long, LinkedList<Node>>
{
    public override LinkedList<Node> Parse(string input) => new LinkedList<Node>(input.Trim().Select(it => new Node(it, $"{it}".ToLower()[0])));

    [TestCase(Input.Example, 10)]
    [TestCase(Input.File, 11298)]
    public override long Part1(LinkedList<Node> input)
    {
        input = Polymerize(input);
        return input.Count;
    }
    
    [TestCase(Input.Example, 4)]
    [TestCase(Input.File, 5148)]
    public override long Part2(LinkedList<Node> input)
    {
        return Enumerable.Range('a', 'z' - 'a' + 1)
            .Select(c => Polymerize(new LinkedList<Node>(input.Where(node => node.Lower != c))))
            .Select(p => p.Count)
            .Min();
    }

    private static LinkedList<Node> Polymerize(LinkedList<Node> input)
    {
        input = new LinkedList<Node>(input);
        var current = input.First;
        while (current is {})
        {
            var next = current.Next;
            if (next is {} && current.Value.Lower == next.Value.Lower && current.Value.Actual != next.Value.Actual)
            {
                var previous = current.Previous;
                input.Remove(next);
                input.Remove(current);
                current = previous ?? input.First;
            }
            else 
            {
                current = next;
            }
        }

        return input;
    }
}

public record Node(char Actual, char Lower);
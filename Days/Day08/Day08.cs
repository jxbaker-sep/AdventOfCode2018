using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode2018.Utils;
using JetBrains.Annotations;

namespace AdventOfCode2018.Days.Day08;

[UsedImplicitly]
public class Day08 : AdventOfCode<long, IReadOnlyList<long>>
{
    public override IReadOnlyList<long> Parse(string input) => input.Lines().Single().Split(" ").Select(it => Convert.ToInt64(it)).ToList();

    [TestCase(Input.Example, 138)]
    [TestCase(Input.File, 46829)] 
    public override long Part1(IReadOnlyList<long> input)
    {
        var array = input.ToArray();
        return Sum(ParseNode(array, out var _));
    }
    
    [TestCase(Input.Example, 66)]
    [TestCase(Input.File, 0)]
    public override long Part2(IReadOnlyList<long> input)
    {
        var array = input.ToArray();
        return Sum2(ParseNode(array, out var _));
    }

    private Node ParseNode(ArraySegment<long> data, out ArraySegment<long> remainder)
    {
        var nChildren = data[0];
        var nMetadata = data[1];
        data = data.Slice(2);
        var children = new List<Node>();
        foreach(var _ in Enumerable.Range(0, (int)nChildren))
        {
            children.Add(ParseNode(data, out data));
        }
        var metadata = data.Take((int)nMetadata).ToList();
        remainder = data.Slice((int)nMetadata);
        return new Node(children, metadata);
    }

    private long Sum(Node node)
    {
        return node.Children.Select(child => Sum(child)).Sum() + node.Metadata.Sum();
    }

    private long Sum2(Node node)
    {
        if (!node.Children.Any()) return node.Metadata.Sum();
        return node.Metadata.Sum(md => md <= node.Children.Count ? Sum2(node.Children[(int)md-1]) : 0);
    }
}

public record Node(IReadOnlyList<Node> Children, IReadOnlyList<long> Metadata);
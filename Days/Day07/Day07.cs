using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode2018.Utils;
using JetBrains.Annotations;

namespace AdventOfCode2018.Days.Day07;

[UsedImplicitly]
public class Day07 : AdventOfCode<string, IReadOnlyList<Instruction>>
{
    public override IReadOnlyList<Instruction> Parse(string input) => 
        input.Lines(line => { 
            var m = Regex.Match(line, @"Step (?<pre>.) must be finished before step (?<post>.) can begin.");
            return new Instruction(m.StringGroup("pre")[0], m.StringGroup("post")[0]);
        });

    [TestCase(Input.Example, "CABDFE")]
    [TestCase(Input.File, "ADEFKLBVJQWUXCNGORTMYSIHPZ")] 
    public override string Part1(IReadOnlyList<Instruction> input)
    {
        var ruleToDependencies = new Dictionary<char, List<char>>();
        var distinct = input.SelectMany(i => new[]{i.Rule, i.Dependency}).Distinct().ToList();
        foreach(var item in input)
        {
            ruleToDependencies.AddToList(item.Rule, item.Dependency);
            distinct.Remove(item.Rule);
        }
        var result = "";
        var open = new PriorityQueue<char>(c => c);
        foreach(var item in distinct) open.Enqueue(item);
        while (open.TryDequeue(out var current))
        {
            result += current;
            foreach (var kv in ruleToDependencies.ToList())
            {
                kv.Value.Remove(current);
                if (!kv.Value.Any()) {
                    open.Enqueue(kv.Key);
                    ruleToDependencies.Remove(kv.Key);
                }
            }
        }
        return result;
    }
    
    [TestCase(Input.Example, "15")]
    [TestCase(Input.File, "1120")]
    public override string Part2(IReadOnlyList<Instruction> input)
    {
        var isExample = input[0].Dependency == 'C';
        var delta = isExample ? 0 : 60;
        var ruleToDependencies = new Dictionary<char, List<char>>();
        var distinct = input.SelectMany(i => new[]{i.Rule, i.Dependency}).Distinct().ToList();
        foreach(var item in input)
        {
            ruleToDependencies.AddToList(item.Rule, item.Dependency);
            distinct.Remove(item.Rule);
        }
        var open = new PriorityQueue<(char Rule, long TimeFinished)>(c => c.TimeFinished * 1000 + c.Rule );
        var availableWorkers = isExample ? 2 : 5;
        var maxWorkers = isExample ? 2 : 5;
        if (distinct.Count > maxWorkers) throw new ApplicationException();
        foreach(var rule in distinct) {
            open.Enqueue((rule, rule - 'A' + 1 + delta ));
            availableWorkers -= 1;
        }
        while (open.TryDequeue(out var current))
        {
            availableWorkers += 1;

            if (!ruleToDependencies.Any() && availableWorkers == maxWorkers) return current.TimeFinished.ToString();

            foreach (var kv in ruleToDependencies.ToList())
            {
                kv.Value.Remove(current.Rule);
            }

            var availableRules = ruleToDependencies.Where(kv => !kv.Value.Any()).Select(it => it.Key).OrderBy(it => it).ToQueue();

            while (availableWorkers > 0 && availableRules.TryDequeue(out var rule))
            {
                open.Enqueue((rule, current.TimeFinished + rule - 'A' + 1 + delta));
                availableWorkers -= 1;
                ruleToDependencies.Remove(rule);
            }
        }
        throw new ApplicationException();
    }
}

public record Instruction(char Dependency, char Rule);
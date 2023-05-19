using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode2018.Utils;
using JetBrains.Annotations;

namespace AdventOfCode2018.Days.Day12;

[UsedImplicitly]
public class Day12 : AdventOfCode<long, World>
{
    public override World Parse(string input)
    {
        var lines = input.Lines();
        var initialState = lines[0].Substring("initial state: ".Length);
        var rules = new List<Rule>();
        foreach(var line in lines.Skip(2))
        {
            var items = line.Split(" => ");
            rules.Add(new Rule(items[0], items[1][0]));
        }
        return new World(rules, new State(initialState, 0));
    }

    [TestCase(Input.Example, 325)] 
    [TestCase(Input.File, 1991)] 
    public override long Part1(World world)
    {
        var currentState = Run(world, 20);
        return currentState.Plants.WithIndices().Sum(item => item.Value == '#' ? currentState.Left + item.Index : 0);
    }

    // [TestCase(Input.Example, 0)] 
    [TestCase(Input.File, 1100000000511)] 
    public override long Part2(World world)
    {
        var currentState = Run(world, 50_000_000_000);
        return currentState.Plants.WithIndices().Sum(item => item.Value == '#' ? currentState.Left + item.Index : 0);
    }

    private State Run(World world, long generations)
    {
        var rules = world.Rules.ToDictionary(it => it.Input, it => it.Output);
        var currentState = world.State;
        var closed = new Dictionary<string, (long Left, long Generation)>();
        var found = false;
        for (var generation = 0L ; generation < generations; generation++)
        {
            if (!found && closed.TryGetValue(currentState.Plants, out var previous))
            {
                var deltaGenerations = generation - previous.Generation;
                var deltaLeft = currentState.Left - previous.Left;
                var iterations = (generations - generation) / deltaGenerations;
                generation = generation + iterations * deltaGenerations - 1;
                currentState = new State(currentState.Plants, currentState.Left + iterations * deltaLeft);
                found = true;
                continue;
            }
            closed.Add(currentState.Plants, (currentState.Left, generation));
            var currentPlants = "....." + currentState.Plants + ".....";
            var left = currentState.Left - 5;
            var newState = "..";
            for(var n = 0; n < currentPlants.Length - 5; n++)
            {
                var slice = currentPlants.Skip(n).Take(5).Join();
                newState += rules.TryGetValue(slice, out var c) ? c : ".";
            }
            left += newState.TakeWhile(c => c == '.').Count();
            currentState = new State(newState.TrimStart('.').TrimEnd('.'), left);
        }
        return currentState;
    }
}

public record Rule(string Input, char Output);
public record State(string Plants, long Left);

public record World(IReadOnlyList<Rule> Rules, State State);
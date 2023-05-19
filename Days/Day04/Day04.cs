using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode2018.Utils;
using JetBrains.Annotations;
using TypeParser;

namespace AdventOfCode2018.Days.Day04;

[UsedImplicitly]
public class Day04 : AdventOfCode<long, IReadOnlyList<Record>>
{
    public override IReadOnlyList<Record> Parse(string input) => input
        .Lines()
        .Select(line => {
            var m = Regex.Match(line, @"\[(?<year>\d\d\d\d)-(?<month>\d\d)-(?<day>\d\d) (?<hour>\d\d):(?<minute>\d\d)] (Guard #(?<id>\d+) begins shift)?");
            var timestamp = m.LongGroup("minute") + 60 * m.LongGroup("hour") + 24 * 60  * m.LongGroup("day") + 31 * 24 * 60 * m.LongGroup("month");
            return new Record(timestamp, m.LongGroup("minute"), m.OptionalLongGroup("id"), line.Contains("asleep"), line.Contains("wakes"));
        })
        .OrderBy(it => it.Timestamp)
        .ToList();

    [TestCase(Input.Example, 240)]
    [TestCase(Input.File, 67558)]
    public override long Part1(IReadOnlyList<Record> input)
    {
        var d = Crunch(input);

        var it = d.Select(kv => new {GuardId = kv.Key, MinutesAsleep = kv.Value.Sum(), LargestMinute = kv.Value.WithIndices().MaxBy(it => it.Value).Index})
            .MaxBy(it => it.MinutesAsleep) ?? throw new ApplicationException();
        return it.GuardId * it.LargestMinute;
    }

    [TestCase(Input.Example, 4455)]
    [TestCase(Input.File, 78990)]
    public override long Part2(IReadOnlyList<Record> input)
    {
        var d = Crunch(input);

        var it = d.Select(kv => new {GuardId = kv.Key, LargestMinute = kv.Value.Max(), LargestMinuteIndex = kv.Value.WithIndices().MaxBy(it => it.Value).Index})
            .MaxBy(it => it.LargestMinute) ?? throw new ApplicationException();
        return it.GuardId * it.LargestMinuteIndex;
    }

    private Dictionary<long, long[]> Crunch(IReadOnlyList<Record> input)
    {
        var d = new Dictionary<long, long[]>();
        var id = 0L;
        var asleep = 0L;
        foreach(var item in input)
        {
            if (item.GuardId is {} guardId) id = guardId;
            else if (item.FallsAsleep) asleep = item.Minute;
            else if (item.WakesUp)
            {
                var array = Enumerable.Repeat(0L, 60).ToArray();
                if (d.ContainsKey(id))
                {
                    array = d[id];
                }
                else{
                    d[id] = array;
                }
                foreach(var x in Enumerable.Range((int)asleep, (int)item.Minute - (int)asleep))
                {
                    array[x] += 1;
                }
            }
        }
        return d;
    }
}

public record Record(long Timestamp, long Minute, long? GuardId, bool FallsAsleep, bool WakesUp);
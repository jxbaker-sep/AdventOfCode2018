using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode2018.Utils;
using FluentAssertions;
using JetBrains.Annotations;

namespace AdventOfCode2018.Days.Day14;

[UsedImplicitly]
public class Day14 : AdventOfCode<long, long>
{
    public override long Parse(string input)
    {
        return 0;
    }

    [TestCase(Input.Example, 0)]
    public override long Part1(long world)
    {
        Recipes(9).Should().Be("5158916779");
        Recipes(5).Should().Be("0124515891");
        Recipes(18).Should().Be("9251071085");
        Recipes(2018).Should().Be("5941429882");
        Recipes(320851).Should().Be("7116398711");
        return 0;
    }


    [TestCase(Input.File, 0)]
    public override long Part2(long world)
    {
        Recipes2("51589").Should().Be(9);
        Recipes2("01245").Should().Be(5);
        Recipes2("92510").Should().Be(18);
        Recipes2("59414").Should().Be(2018);
        Recipes2("320851").Should().Be(20316365L);
        return 0;
    }

    private string Recipes(int count)
    {
        var recipes = new List<int> { 3, 7 };
        var current1 = 0;
        var current2 = 1;
        for (var i = 2; i < count + 10; i++)
        {
            var newRecipe = recipes[current1] + recipes[current2];
            if (newRecipe >= 10)
            {
                recipes.Add(1);
                recipes.Add(newRecipe % 10);
            }
            else recipes.Add(newRecipe);
            current1 = (current1 + (recipes[current1] + 1)) % recipes.Count;
            current2 = (current2 + (recipes[current2] + 1)) % recipes.Count;
        }

        return recipes.Skip(count).Take(10).Join("");
    }

    private long Recipes2(string needleAsString)
    {
        var recipes = new List<int> { 3, 7 };
        var current1 = 0;
        var current2 = 1;
        IReadOnlyList<int> needle = needleAsString.Select(c => Convert.ToInt32($"{c}")).ToList();
        while (true)
        {
            var newRecipe = recipes[current1] + recipes[current2];
            if (newRecipe >= 10)
            {
                recipes.Add(1);
                if (Check(recipes, needle)) return recipes.Count - needle.Count;
            }
            recipes.Add(newRecipe % 10);
            if (Check(recipes, needle)) return recipes.Count - needle.Count;
            current1 = (current1 + (recipes[current1] + 1)) % recipes.Count;
            current2 = (current2 + (recipes[current2] + 1)) % recipes.Count;
        }
    }

    private bool Check(IReadOnlyList<int> haystack, IReadOnlyList<int> needle)
    {
        if (haystack.Count < needle.Count) return false;
        var x = haystack.Count - needle.Count;
        for (var i = 0; i < needle.Count; i++)
        {
            if (haystack[x + i] != needle[i]) return false;
        }
        return true;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode2018.Utils;
using JetBrains.Annotations;

namespace AdventOfCode2018.Days.Day13;

[UsedImplicitly]
public class Day13 : AdventOfCode<string, World>
{
    public override World Parse(string input)
    {
        var carts = new List<Cart>();
        var grid = new Dictionary<Position, char>();
        foreach(var (line, y) in input.Split("\n").Select(l => l.TrimEnd()).WithIndices())
        {
            foreach(var (c, x) in line.WithIndices())
            {
                switch (c)
                {
                    case '/':
                    case '\\':
                    case '+':
                    case '-':
                    case '|':
                        grid.Add(new Position(y, x), c);
                        break;
                    case '>':
                        grid.Add(new Position(y, x), '-');
                        carts.Add(new Cart(new Position(y, x), Vector.East));
                        break;
                    case '<':
                        grid.Add(new Position(y, x), '-');
                        carts.Add(new Cart(new Position(y, x), Vector.West));
                        break;
                    case '^':
                        grid.Add(new Position(y, x), '|');
                        carts.Add(new Cart(new Position(y, x), Vector.North));
                        break;
                    case 'v':
                        grid.Add(new Position(y, x), '|');
                        carts.Add(new Cart(new Position(y, x), Vector.South));
                        break;
                    case ' ':
                        break;
                    default:
                        throw new ApplicationException();
                }
            }
        }
        return new World(grid, carts);
    }

    [TestCase(Input.Example, "7,3")] 
    [TestCase(Input.File, "136,36")] 
    public override string Part1(World world)
    {
        var carts = world.Carts.ToList();
        var n = -1;
        while (true)
        {
            n = (n + 1) % carts.Count;
            var cart = carts[n];
            var newCart = GetNewFacing(world, cart);
            
            if (carts.Any(cart2 => cart2.Position == newCart.Position))
            {
                return $"{newCart.Position.X},{newCart.Position.Y}";
            }
            carts[n] = newCart;
        }
    }


    [TestCase(Input.Raw, "6,4", Raw=@"/>-<\  
|   |  
| /<+-\
| | | v
\>+</ |
  |   ^
  \<->/")] 
    [TestCase(Input.File, "53,111")] 
    public override string Part2(World world)
    {
        var carts = world.Carts.ToList();
        var n = 0;
        if (world.Carts.Count % 2 == 0) throw new ApplicationException("Even number of carts!");
        while (carts.Count > 1)
        {
            var cart = carts[n % carts.Count];
            var newCart = GetNewFacing(world, cart);
            
            var collider = carts.WithIndices().Where(cart2 => cart2.Value.Position == newCart.Position).SingleOrDefault();

            if (collider.Value is {} )
            {
                var n2 = collider.Index;
                carts.RemoveAt(Math.Max(n, n2));
                carts.RemoveAt(Math.Min(n, n2));
                if (n2 > n) continue;
                n -= 1;
                continue;
            }

            carts[n] = newCart;
            n = (n + 1) % carts.Count;
        }
        var p2 = carts[0].Position + carts[0].Facing;
        return $"{p2.X},{p2.Y}";
    }

    private Cart GetNewFacing(World world, Cart cart)
    {
        var p2 = cart.Position + cart.Facing;
        var facing = cart.Facing;
        var track = world.Grid[p2];
        cart = cart with { Position = p2 };

        if (facing == Vector.North && track == '|') return cart with { Facing = Vector.North };
        if (facing == Vector.North && track == '/') return cart with { Facing = Vector.East };
        if (facing == Vector.North && track == '\\') return cart with { Facing = Vector.West };

        if (facing == Vector.South && track == '|') return cart with { Facing = Vector.South };
        if (facing == Vector.South && track == '/') return cart with { Facing = Vector.West };
        if (facing == Vector.South && track == '\\') return cart with { Facing = Vector.East };

        if (facing == Vector.East && track == '-') return cart with { Facing = Vector.East };
        if (facing == Vector.East && track == '/') return cart with { Facing = Vector.North };
        if (facing == Vector.East && track == '\\') return cart with { Facing = Vector.South };

        if (facing == Vector.West && track == '-') return cart with { Facing = Vector.West };
        if (facing == Vector.West && track == '/') return cart with { Facing = Vector.South };
        if (facing == Vector.West && track == '\\') return cart with { Facing = Vector.North };

        if (track == '+')
        {
            var turnCount = cart.TurnCount;
            cart = cart with { TurnCount = (cart.TurnCount + 1) % 3 };
            if (turnCount == 0) return cart with { Facing = cart.Facing.RotateLeft() };
            if (turnCount == 1) return cart with { Facing = cart.Facing };
            if (turnCount == 2) return cart with { Facing = cart.Facing.RotateRight() };
        }

        throw new ApplicationException();
    }
}

public record Cart(Position Position, Vector Facing, int TurnCount = 0);

public record World(IReadOnlyDictionary<Position, char> Grid, IReadOnlyList<Cart> Carts);
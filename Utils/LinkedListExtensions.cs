using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2018.Utils;

public static class LinkedListExtensions
{
    public static LinkedListNode<T> RollForward<T>(this LinkedListNode<T> self, int count)
    {
        foreach(var _ in Enumerable.Range(0, count))
        {
            if (self.Next is {})
            {
                self = self.Next;
            }
            else 
            {
                self = self.List?.First ?? throw new ApplicationException();
            }
        }
        return self;
    }

    public static LinkedListNode<T> RollBack<T>(this LinkedListNode<T> self, int count)
    {
        foreach(var _ in Enumerable.Range(0, count))
        {
            if (self.Previous is {})
            {
                self = self.Previous;
            }
            else 
            {
                self = self.List?.Last ?? throw new ApplicationException();
            }
        }
        return self;
    }
}
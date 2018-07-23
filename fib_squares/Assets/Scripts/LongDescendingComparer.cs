
using System.Collections.Generic;

public class LongDescendingComparer : IComparer<long>
{
    public int Compare(long x, long y)
    {
        return y.CompareTo(x);
    }
}
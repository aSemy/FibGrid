using System;
using System.Collections.Generic;

public class FibonacciSequenceChecker : ISequenceChecker
{
    public bool IsNumInSequence(long num)
    {
        return isFibonacci(num);
    }

    public long IsNumPreviousSequenceTerm(long num, List<long> currentSequence)
    {
        throw new NotImplementedException();
    }

    private bool isFibonacci(long num)
    {
        if (num < 1)
        {
            return false;
        }
        else
        {
            long f = Convert.ToInt64(5 * Math.Pow(num, 2d));

            return isPerfectSquare(f + 4) || isPerfectSquare(f - 4);
        }
    }

    private bool isPerfectSquare(long num)
    {
        double result = Math.Sqrt(num);
        return result % 1 == 0;
    }
}

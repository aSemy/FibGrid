using System;
using System.Collections.Generic;

public class FibonacciSequenceChecker : ISequenceChecker
{
    public static readonly double phi = 1.6180339887498948482d;

    public bool IsNumInSequence(long num)
    {
        return IsFibonacci(num);
    }

    /// <summary>
    /// Given an existing, descending sequence of Fib numbers, work out if the given number is the n-1 th in the sequence
    /// 
    /// Assume that <paramref name="potentialFib"/> has already been vetted as a Fib number!!!
    /// </summary>
    /// <returns><c>true</c>, if fib NM inus1 was ised, <c>false</c> otherwise.</returns>
    /// <param name="potentialFib">Potential fib.</param>
    /// <param name="currentSequence">Current sequence.</param>
    public bool IsNumPreviousSequenceTerm(long potentialFib, List<long> currentSequence)
    {
        if (currentSequence.Count >= 2)
        {
            // we've got a couple of existing fibs to go from
            // we can calculate the next one directly
            long prev = currentSequence[currentSequence.Count - 1];
            long prevprev = currentSequence[currentSequence.Count - 2];
            return prevprev - prev == potentialFib;
        }
        else if (currentSequence.Count == 1)
        {
            // only have one fib so far
            // predict it by dividing by phi, and rounding
            long predictedPrev = Convert.ToInt64(Math.Round(currentSequence[0] / phi));
            return potentialFib == predictedPrev;
        }
        else
        {
            // true by assumption, 
            // it's already been vetted as a fib
            return true;
        }
    }

    private bool IsFibonacci(long num)
    {
        if (num < 1)
        {
            return false;
        }
        else
        {
            long f = Convert.ToInt64(5 * Math.Pow(num, 2d));

            return IsPerfectSquare(f + 4) || IsPerfectSquare(f - 4);
        }
    }

    private bool IsPerfectSquare(long num)
    {
        double result = Math.Sqrt(num);
        return result % 1 == 0;
    }
}

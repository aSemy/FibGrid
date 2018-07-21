using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class FibTools
{

    public static double phi = 1.6180339887498948482d;

    static bool IsFibSequence(List<long> sequence)
    {

        if (sequence == null || sequence.Count < 1)
            return false;

        foreach (long num in sequence)
        {
            if (!isFibonacci(num))
                return false;
        }

        if (sequence.Count == 1)
            return true;
        else
        {
            for (int i = 1; i < sequence.Count; i++)
            {
                if (!IsNextAFib(sequence[i - 1], sequence[i]))
                    return false;
            }
            return true;
        }
    }


    static public bool isFibonacci(long num)
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

    static bool isPerfectSquare(long num)
    {
        double result = Math.Sqrt(num);
        return result % 1 == 0;
    }

    /// http://www.maths.surrey.ac.uk/hosted-sites/R.Knott/Fibonacci/fibFormula.html#section4
    static bool IsNextAFib(long current, long next)
    {

        if (current > 1)
        {
            long predictedNext = Convert.ToInt64(Math.Round(current * phi));
            return predictedNext == next;
        }
        else if (current == 1)
        {
            return next == 1 || next == 2;
        }
        else
        {
            return false;
        }
    }

    /// http://www.maths.surrey.ac.uk/hosted-sites/R.Knott/Fibonacci/fibFormula.html#section4
    static bool IsPrevAFib(long current, long prev)
    {

        if (current > 1)
        {
            long predictedPrev = Convert.ToInt64(Math.Round(current / phi));
            return predictedPrev == prev;
        }
        //else if (current == 1)
        //{
        //    return next == 1 || next == 2;
        //}
        else
        {
            return false;
        }
    }

}

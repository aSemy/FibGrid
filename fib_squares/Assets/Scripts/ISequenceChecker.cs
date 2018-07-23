using System;
using System.Collections.Generic;

public interface ISequenceChecker
    {

    bool IsNumInSequence(long num);

    long IsNumPreviousSequenceTerm(long num, List<long> currentSequence);

    }

using System;
using System.Collections.Generic;

public interface ISequenceChecker
    {

    bool IsNumInSequence(long num);

    bool IsNumPreviousSequenceTerm(long num, List<long> currentSequence);

    }

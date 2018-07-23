using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

// calculate scoring tiles
public class ScoreCalculator
{
    private long minimumSequenceLength = 5;

    public HashSet<QuadController> FindScoringQuads(GameObject[,] quads)
    {
        quads = (GameObject[,])quads.Clone();
        foreach (GameObject go in quads)
            go.GetComponent<QuadController>().scoreMesh.color = Color.white;


        HashSet<QuadController> scoringQuads = new HashSet<QuadController>();

        // find fib numbers
        SortedList<long, HashSet<Vector2Int>> mapFibNumsToLocations = FindFibNumbersAndLocations(quads);

        // step 2 
        // check for fib sequences

        // only worth checking out if there are more than 4 distinct potential fibs
        // e.g. if the board is covered with 1s, then there's not going to be a sequence
        if (mapFibNumsToLocations.Keys.Count >= minimumSequenceLength - 1)
        {
            List<long> fibNums = new List<long>(mapFibNumsToLocations.Keys);
            // sort it, so we check the highest fibs first
            fibNums.Sort(new LongDescendingComparer());

            // for each found fib number,
            // check for sequences
            foreach (long fibNum in fibNums)
            {
                // for each location of this fib number
                // check for fib sequences
                foreach (Vector2Int locationOfSequenceNumber in mapFibNumsToLocations[fibNum])
                {
                    HashSet<QuadController> scoringQuadsForLocation = GetScoringQuads(quads, locationOfSequenceNumber);

                    // add the scoring quads of this location to the total list
                    scoringQuads.UnionWith(scoringQuadsForLocation);

                    // we're done with this quad now
                    // remove it
                    // (maybe this improves performance?)
                    //quads[loc.x, loc.y].GetComponent<QuadController>().scoreMesh.color = Color.red;
                    quads[locationOfSequenceNumber.x, locationOfSequenceNumber.y] = null;
                }
            }
        }

        return scoringQuads;
    }

    SortedList<long, HashSet<Vector2Int>> FindFibNumbersAndLocations(GameObject[,] quads)
    {
        // sort from high to low
        SortedList<long, HashSet<Vector2Int>> mapFibNumsToLocations
        = new SortedList<long, HashSet<Vector2Int>>(new LongDescendingComparer());

        // step 1
        // remove tiles which aren't fib numbers
        for (int x = 0; x < GameController.gameWidth; x++)
        {
            for (int y = 0; y < GameController.gameHeight; y++)
            {
                if (quads[x, y] != null)
                {
                    QuadController qc = quads[x, y].GetComponent<QuadController>();

                    if (FibTools.isFibonacci(qc.cellValue))
                    {
                        // we've found a Fib number!

                        // if this is the first Fib num we've found, create an array
                        if (!mapFibNumsToLocations.ContainsKey(qc.cellValue))
                            mapFibNumsToLocations.Add(qc.CellValue, new HashSet<Vector2Int>());
                        
                        // store the location so we can investigate 
                        // to see if it's part of a sequence later
                        mapFibNumsToLocations[qc.cellValue].Add(new Vector2Int(x, y));
                    }
                    else
                    {
                        // it's not a Fib number, so nullify it
                        quads[x, y] = null;
                    }
                }
            }
        }
        return mapFibNumsToLocations;
    }

    /// <summary>
    /// Given an existing, descending sequence of Fib numbers, work out if the given number is the n-1 th in the sequence
    /// 
    /// Assume that <paramref name="potentialFib"/> has already been vetted as a Fib number!!!
    /// </summary>
    /// <returns><c>true</c>, if fib NM inus1 was ised, <c>false</c> otherwise.</returns>
    /// <param name="potentialFib">Potential fib.</param>
    /// <param name="currentSequence">Current sequence.</param>
    bool IsFibNMinus1(long potentialFib, List<long> currentSequence)
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
            long predictedPrev = Convert.ToInt64(Math.Round(currentSequence[0] / FibTools.phi));
            return potentialFib == predictedPrev;
        }
        else
        {
            // true by default, 
            // it's already been vetted as a fib
            return true;
        }
    }

    private HashSet<QuadController> GetScoringQuads(GameObject[,] quads, Vector2Int loc) {
        
        // create iterators for up, down, left, right
        Tiles tilesXDown = new Tiles(quads, loc, -1, 0);
        Tiles tilesXUp = new Tiles(quads, loc, 1, 0);
        Tiles tilesYDown = new Tiles(quads, loc, 0, -1);
        Tiles tilesYUp = new Tiles(quads, loc, 0, 1);
        Tiles[] tilesArray = {
                        tilesXDown,
                        tilesXUp,
                        tilesYDown,
                        tilesYUp
                    };

        // for each direction, check to see if there's a Fib seq.
        foreach (Tiles tiles in tilesArray)
        {
            HashSet<QuadController> potentialScoringQuads = GetValidSequenceQuads(tiles);

            //if (potentialScoringQuads.Count > 0 && fibNum >= 5)
            //    Debug.Log("potential " + potentialScoringQuads.Count + ", fibNum: " + fibNum + ", loc " + loc);

            if (potentialScoringQuads.Count >= minimumSequenceLength)
            {
                // add to scoring list
                return potentialScoringQuads;
            }
        }
        return new HashSet<QuadController>();
    }

    HashSet<QuadController> GetValidSequenceQuads(Tiles tiles)
    {
        List<long> potentialSeq = new List<long>();
        HashSet<QuadController> potentialScoringQuads = new HashSet<QuadController>();

        // get adjacent quads, sequentially
        // (direction specified during Tiles creation)
        foreach (GameObject quad in tiles)
        {
            if (quad != null)
            {
                long cellValue = quad.GetComponent<QuadController>().cellValue;

                // is this cell value the n-1th in the sequence?
                if (IsFibNMinus1(cellValue, potentialSeq)) // TODO replace
                {
                    // We've got another fib for our sequence!
                    // great!
                    potentialSeq.Add(cellValue);
                    potentialScoringQuads.Add(quad.GetComponent<QuadController>());
                }
                else
                {
                    // it's not a fib, nevermind
                    // break and return what we have
                    //Debug.Log("End of sequence because " + fib + " is not a fib");
                    break;
                }
            }
            else
            {
                //Debug.Log("End of sequence origin[" + tiles.Origin + "] because quad is null");
                // it's not a fib, nevermind
                // break and return what we have
                break;
            }
        }

        return potentialScoringQuads;
    }
}

using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

// calculate scoring tiles
public class ScoreCalculator
{

    long minSize = 5;

    public HashSet<QuadController> FindScoringQuads(GameObject[,] quads)
    {
        quads = (GameObject[,])quads.Clone();

        HashSet<QuadController> scoringQuads = new HashSet<QuadController>();

        // find fib numbers
        SortedList<long, HashSet<Vector2Int>> mapFibNumsToLocations
        = findFibNumbersAndLocations(quads);

        // step 2 
        // check for fib sequences

        // only worth checking out if there are more than 4 distinct potential fibs
        // e.g. if the board is covered with 1's, then there's not going to be a sequence
        if (mapFibNumsToLocations.Keys.Count >= minSize - 1)
        {

            List<long> keys = new List<long>(mapFibNumsToLocations.Keys);
            keys.Sort(new LongDescendingComparer());

            // for each found fib number,
            // check for sequences
            foreach (long key in keys)
            {
                // for each location of this fib number
                // check for fib sequences
                foreach (Vector2Int loc in mapFibNumsToLocations[key])
                {
                    Debug.Log("Checking out location " + loc + " for key " + key);

                    // up
                    List<long> potentialSeq = new List<long>();
                    HashSet<QuadController> potentialScoringQuads = new HashSet<QuadController>();
                    for (int y = loc.y; y < GameController.gameHeight; y++)
                    {
                        GameObject quad = quads[loc.x, y];
                        if (quad != null)
                        {

                            long fib = quad.GetComponent<QuadController>().cellValue;

                            bool isFib = false;
                            if (potentialSeq.Count >= 2)
                            {
                                long prev = potentialSeq[potentialSeq.Count - 1];
                                long prevprev = potentialSeq[potentialSeq.Count - 2];
                                if (prevprev - prev == fib)
                                    isFib = true;
                            }
                            else if (potentialSeq.Count == 1)
                            {
                                long predictedPrev = Convert.ToInt64(Math.Round(potentialSeq[0] / FibTools.phi));
                                if (fib == predictedPrev)
                                    isFib = true;
                            }
                            else
                            {
                                // true by default, 
                                // it's already been vetted as a fib
                                isFib = true;
                            }
                            if (isFib)
                            {
                                potentialSeq.Add(fib);
                                potentialScoringQuads.Add(quad.GetComponent<QuadController>());
                            }
                            else
                            {
                                break;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    Debug.Log("Potential sequence for key " + key + ", loc " + loc + ": " + potentialSeq);
                    // we've got a potential sequence! Let's check it out
                    if (potentialSeq.Count >= minSize)
                    {
                        // add to scoring list
                        scoringQuads.UnionWith(potentialScoringQuads);
                    }
                }
            }
        }

        return scoringQuads;
    }

    SortedList<long, HashSet<Vector2Int>> findFibNumbersAndLocations(GameObject[,] quads) {
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

                        // store the location so we can investigate 
                        // to see if it's part of a sequence later

                        if (!mapFibNumsToLocations.ContainsKey(qc.cellValue))
                            mapFibNumsToLocations.Add(qc.CellValue, new HashSet<Vector2Int>());

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


    public class LongDescendingComparer : IComparer<long>
    {
        public int Compare(long x, long y)
        {
            return y.CompareTo(x);
        }
    }

}

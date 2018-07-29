
using System.Collections.Generic;
using UnityEngine;

// calculate scoring tiles
public class ScoreCalculator
{
    private long minimumSequenceLength = 5;

    private readonly ISequenceChecker sequenceChecker = new FibonacciSequenceChecker();

    /// <summary>
    /// Find tiles that are in a valid sequence
    /// </summary>
    /// <returns>The scoring tiles.</returns>
    /// <param name="tileGameObjects">Tile game objects.</param>
    public HashSet<TileController> FindScoringTiles(TileController[,] tileGameObjects)
    {
        // create a clone so we don't affect the real tiles
        tileGameObjects = (TileController[,])tileGameObjects.Clone();

        // we're going to check each tile and if they 'score', then store
        // them in this set and return them
        HashSet<TileController> scoringTiles = new HashSet<TileController>();

        // find fib numbers
        SortedList<long, HashSet<Vector2Int>> mapSequenceNumsToLocations = FindSequenceNumbersAndLocations(tileGameObjects);

        // step 2 
        // check for fib sequences

        // only worth checking out if there are more than 4 distinct potential fibs
        // e.g. if the board is covered with 1s, then there's not going to be a sequence
        if (mapSequenceNumsToLocations.Keys.Count >= minimumSequenceLength - 1)
        {
            List<long> sequenceNumbers = new List<long>(mapSequenceNumsToLocations.Keys);
            // sort it, so we check the highest fibs first
            sequenceNumbers.Sort(new LongDescendingComparer());

            // for each found fib number,
            // check for sequences
            foreach (long sequenceNumber in sequenceNumbers)
            {
                // for each location of this fib number
                // check for fib sequences
                foreach (Vector2Int locationOfSequenceNumber in mapSequenceNumsToLocations[sequenceNumber])
                {
                    HashSet<TileController> scoringQuadsForLocation = GetScoringQuads(tileGameObjects, locationOfSequenceNumber);

                    // add the scoring quads of this location to the total list
                    scoringTiles.UnionWith(scoringQuadsForLocation);

                    // we're done with this quad now
                    // remove it
                    // (maybe this improves performance?)
                    tileGameObjects[locationOfSequenceNumber.x, locationOfSequenceNumber.y] = null;
                }
            }
        }

        return scoringTiles;
    }

    /// <summary>
    /// Get the locations of all tiles that have a valid sequence number. 
    /// 
    /// Map each existing sequence number to a list of locations.
    /// </summary>
    /// <returns>The sequence numbers and locations.</returns>
    /// <param name="tileGameObjects">Tile game objects.</param>
    SortedList<long, HashSet<Vector2Int>> FindSequenceNumbersAndLocations(TileController[,] tileGameObjects)
    {
        // sort from high to low
        SortedList<long, HashSet<Vector2Int>> mapSequenceNumsToLocations
        = new SortedList<long, HashSet<Vector2Int>>(new LongDescendingComparer());

        // step 1
        // remove tiles which aren't fib numbers
        for (int x = 0; x < GameController.gameWidth; x++)
        {
            for (int y = 0; y < GameController.gameHeight; y++)
            {
                if (tileGameObjects[x, y] != null)
                {
                    TileController tileController = tileGameObjects[x, y];

                    if (sequenceChecker.IsNumInSequence(tileController.CellValue))
                    {
                        // we've found a Fib number!

                        // if this is the first Fib num we've found, create an array
                        if (!mapSequenceNumsToLocations.ContainsKey(tileController.CellValue))
                            mapSequenceNumsToLocations.Add(tileController.CellValue, new HashSet<Vector2Int>());
                        
                        // store the location so we can investigate 
                        // to see if it's part of a sequence later
                        mapSequenceNumsToLocations[tileController.CellValue].Add(new Vector2Int(x, y));
                    }
                    else
                    {
                        // it's not a Fib number, so nullify it
                        tileGameObjects[x, y] = null;
                    }
                }
            }
        }
        return mapSequenceNumsToLocations;
    }

    /// <summary>
    /// Given the location of a tile with a valid sequence number, check each direction for a valid sequence.
    /// 
    /// If there's a valid sequence longer or equal to $minimumSequenceLength then return it.
    /// 
    /// Otherwise, return an empty list.
    /// </summary>
    /// <returns>The scoring quads.</returns>
    /// <param name="tiles">Array of tiles with valid sequence numbers.</param>
    /// <param name="locationOfSequenceTile">Location of sequence tile.</param>
    private HashSet<TileController> GetScoringQuads(TileController[,] tiles, Vector2Int locationOfSequenceTile) {
        
        // create iterators for up, down, left, right
        TileControllerEnumerator tilesXDown = new TileControllerEnumerator(tiles, locationOfSequenceTile, -1, 0);
        TileControllerEnumerator tilesXUp = new TileControllerEnumerator(tiles, locationOfSequenceTile, 1, 0);
        TileControllerEnumerator tilesYDown = new TileControllerEnumerator(tiles, locationOfSequenceTile, 0, -1);
        TileControllerEnumerator tilesYUp = new TileControllerEnumerator(tiles, locationOfSequenceTile, 0, 1);
        TileControllerEnumerator[] tilesEnumerators = {
                        tilesXDown,
                        tilesXUp,
                        tilesYDown,
                        tilesYUp
                    };

        // for each direction, check to see if there's a Fib seq.
        foreach (TileControllerEnumerator tilesEnumerator in tilesEnumerators)
        {
            HashSet<TileController> potentialScoringQuads = GetValidSequenceQuads(tilesEnumerator);

            //if (potentialScoringQuads.Count > 0 && fibNum >= 5)
            //    Debug.Log("potential " + potentialScoringQuads.Count + ", fibNum: " + fibNum + ", loc " + loc);

            if (potentialScoringQuads.Count >= minimumSequenceLength)
            {
                // add to scoring list
                return potentialScoringQuads;
            }
        }
        return new HashSet<TileController>();
    }

    HashSet<TileController> GetValidSequenceQuads(TileControllerEnumerator tiles)
    {
        List<long> potentialSeq = new List<long>();
        HashSet<TileController> potentialScoringQuads = new HashSet<TileController>(new TileController.TileControllerComparer());

        // get adjacent quads, sequentially
        // (direction specified during Tiles creation)
        foreach (TileController tile in tiles)
        {
            if (tile != null)
            {
                long cellValue = tile.CellValue;

                // is this cell value the n-1th in the sequence?
                if (sequenceChecker.IsNumPreviousSequenceTerm(cellValue, potentialSeq))
                {
                    // We've got another fib for our sequence!
                    // great!
                    potentialSeq.Add(cellValue);
                    potentialScoringQuads.Add(tile);
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

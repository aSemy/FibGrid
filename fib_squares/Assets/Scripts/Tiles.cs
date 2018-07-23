using UnityEngine;
using System.Collections.Generic;
using System.Collections;

/// <summary>
/// Create an iterator that moves over a grid of tiles in a direction
/// starting at an origin
/// </summary>
public class Tiles : IEnumerable<GameObject>
{
    GameObject[,] tiles;
    private readonly Vector2Int origin;
    private readonly int xDiff;
    private readonly int yDiff;

    public Tiles(GameObject[,] tiles, Vector2Int origin, int xDiff, int yDiff)
    {
        this.tiles = tiles;
        this.origin = origin;
        this.xDiff = xDiff;
        this.yDiff = yDiff;
    }

    public IEnumerator<GameObject> GetEnumerator()
    {
        for (int x = origin.x, y = origin.y;
             // is X in bounds?
             x < tiles.GetLength(0) && x >= 0
             // and is Y in bounds?
             && y < tiles.GetLength(1) && y >= 0
             ; x += xDiff, y += yDiff)
        {
            yield return tiles[x, y];
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }

    public Vector2Int Origin { get { return this.origin; } }
}
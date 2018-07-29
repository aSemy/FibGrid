using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    // singleton
    public static GameController instance = null;
   
    public static long gameWidth = 50;
    public static long gameHeight = 50;

    private static TileController[,] tileControllers = new TileController[gameWidth, gameHeight];
    //private static SortedList<TileController, Vector2Int> mapTileGameObjectToLocation = new SortedList<TileController, Vector2Int>();

    // parent that holds all tiles
    public RectTransform canvasTransform;
    // a tile used to generate all other tiles
    public Image tileImage;

    // calculate scores
    private static ScoreCalculator scorer = new ScoreCalculator();

    private float tilePositionSpacingPercentage = 1.05f;

    static long score = 0;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance == this)
            Destroy(gameObject);
    }

    // Use this for initialization
    void Start()
    {
        CreateGrid();
    }

    private void CreateGrid()
    {
        tileControllers = new TileController[gameWidth, gameHeight];

        for (int x = 0; x < gameWidth; x++)
        {
            for (int y = 0; y < gameHeight; y++)
            {
                // create a new tile
                Image newTile = Instantiate(tileImage);
                newTile.name = "Tile [x:" + x + ",y:" + y + "]";

                // set parent to be canvas
                newTile.rectTransform.SetParent(canvasTransform, false);

                // generate position
                float tileWidth = newTile.rectTransform.rect.width;
                float tileHeight = newTile.rectTransform.rect.height;
                Vector2 pos = new Vector3(
                    (x - ((float)gameWidth / 2)) * tileWidth + tileWidth / 2, 
                    (y - ((float)gameHeight / 2)) * tileHeight + tileHeight / 2) 
                    * tilePositionSpacingPercentage;
                
                newTile.rectTransform.anchoredPosition = pos;

                // store this created tile
                TileController tc = newTile.gameObject.GetComponent<TileController>();
                tc.X = x;
                tc.Y = y;
                tileControllers[x, y] = tc;
                //mapTileGameObjectToLocation[tc] = new Vector2Int(x, y);
            }
        }

        // make sure the holder is big enough
        // get the first (bottom left) and last (top right) elements
        Image first = tileControllers[0, 0].GetComponent<Image>();
        Image last = tileControllers[tileControllers.GetLength(0) - 1, tileControllers.GetLength(1) - 1].GetComponent<Image>();
        // the difference is going to be the width and height of the holder
        // times a bit of spacing
        Vector2 diff = last.rectTransform.anchoredPosition - first.rectTransform.anchoredPosition
                           + 2 * first.rectTransform.rect.size * tilePositionSpacingPercentage;
        
        canvasTransform.sizeDelta = diff;
    }

    // Update is called once per frame
    void Update() { }

    public void GameUpdate(TileController clickedQuad)
    {
        UpdateScores(clickedQuad);

        // check for 5 consecutive fibs
        HashSet<TileController> scorers = scorer.FindScoringTiles(tileControllers);

        long turnScore = 0;
        foreach (TileController qc in scorers)
        {
            turnScore += qc.CellValue;
            qc.ResetScore();
        }
        if (turnScore > 0)
        {
            score += turnScore;
            Debug.Log("TurnScore: " + turnScore + ", " + "Score: " + score);
        }
    }

    public void GameReset() {
        foreach (TileController tile in tileControllers)
            tile.ResetScore();
    }

    void UpdateScores(TileController clickedTile)
    {
        //Vector2Int clickedTileLocation = mapTileGameObjectToLocation[clickedTile];

        // collect all the quads that need to be incremented
        HashSet<TileController> tilesToClick = new HashSet<TileController>();

        // x axis
        for (int x = 0; x < gameWidth; x++)
        {
            tilesToClick.Add(tileControllers[x, clickedTile.Y]);
        }
        // y axis
        for (int y = 0; y < gameHeight; y++)
        {
            tilesToClick.Add(tileControllers[clickedTile.X, y]);
        }

        // for each, increment the score
        foreach (TileController tile in tilesToClick)
        {
            tile.IncrementScore();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameController : MonoBehaviour {

    public static long gameWidth = 50;
    public static long gameHeight = 50;

    static GameObject[,] quads = new GameObject[gameWidth,gameHeight];
    static Dictionary<GameObject, Vector2Int> mapCoordToQuad = new Dictionary<GameObject, Vector2Int>();

    public Transform canvasTransform;
    public GameObject Quad;

    static ScoreCalculator scorer = new ScoreCalculator();

    private float spacing = 1.05f;

    static long score = 0;

	// Use this for initialization
	void Start () {
        CreateGrid();
	}

    private void CreateGrid() {
        for (int x = 0; x < gameWidth; x++)
        {
            for (int y = 0; y < gameHeight; y++)
            {
                Vector3 pos = new Vector3(x - ((float)gameWidth / 2), y - ((float)gameHeight / 2), 0) * spacing;
                GameObject newQuad = Instantiate(Quad, pos, Quaternion.identity);
                newQuad.name = "Quad [x:" + x + ",y:" + y + "]";

                quads[x, y] = newQuad;
                mapCoordToQuad[newQuad] = new Vector2Int(x, y);
            }
        }
    }

	
	// Update is called once per frame
	void Update () {
		
	}

    public void GameUpdate(QuadController clickedQuad) {

        UpdateScores(clickedQuad);

        // check for 5 consecutive fibs
        HashSet<QuadController> scorers = scorer.FindScoringQuads(quads);

        long turnScore = 0;
        foreach (QuadController qc in scorers) {
            turnScore += qc.cellValue;
            qc.ResetScore();
        }
        if (turnScore > 0) {
            score += turnScore;
            Debug.Log("TurnScore: " + turnScore + ", " + "Score: " + score);
        }

    }

    void UpdateScores(QuadController clickedQuad) {

        Vector2Int clickedQuadPos = mapCoordToQuad[clickedQuad.gameObject];

        // collect all the quads that need to be incremented
        HashSet<GameObject> quadsToIncrement = new HashSet<GameObject>();

        // x axis
        for (int x = 0; x < gameWidth; x++)
        {
            quadsToIncrement.Add(quads[x, clickedQuadPos.y]);
        }
        // y axis
        for (int y = 0; y < gameHeight; y++)
        {
            quadsToIncrement.Add(quads[clickedQuadPos.x, y]);
        }

        // for each, increment the score
        foreach (GameObject q in quadsToIncrement) {
            q.GetComponent<QuadController>().IncrementScore();
        }
    }
}

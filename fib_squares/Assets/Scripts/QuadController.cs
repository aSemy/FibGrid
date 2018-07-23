using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadController : MonoBehaviour {

    public TextMesh scoreMesh;
    public long cellValue = 0;

    public GameController gameController;

	// Use this for initialization
	void Start () {
        TextMesh[] textMeshes = GetComponentsInChildren<TextMesh>();

        scoreMesh = textMeshes[0];
        scoreMesh.text = cellValue.ToString();
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    void OnMouseDown()
    {
        gameController.GameUpdate(this);
    }

    public void HasBeenClickedAndIlikePonies(){
        gameController.GameUpdate(this);
    }

    public void IncrementScore() {

        cellValue += 1;

        // display updated score
        scoreMesh.text = cellValue.ToString();
    }


    public long CellValue
    {
        get { return cellValue; }
    }

    internal void ResetScore()
    {

        cellValue = 0;

        // display updated score
        scoreMesh.text = cellValue.ToString();
        
    }
}

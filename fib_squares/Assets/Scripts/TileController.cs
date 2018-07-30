using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileController : MonoBehaviour
{
    public Text scoreMesh;
    public long CellValue { get; private set; } = 0;
    public long X { get; set; }
    public long Y { get; set; }


    Color defaultColour;
    Color scoreMeshDefaultColour;

    //public GameController gameController;

    // Use this for initialization
    void Start()
    {
        Text[] textMeshes = GetComponentsInChildren<Text>();

        scoreMesh = textMeshes[0];
        scoreMesh.text = CellValue.ToString();
        defaultColour = GetComponent<Image>().color;
        scoreMeshDefaultColour = scoreMesh.color;
        //scoreMesh.enabled = false;
        scoreMesh.color = Color.clear;
    }

    // Update is called once per frame
    void Update()
    {

    }

    //void OnMouseDown()
    //{
    //    GameController.instance.GameUpdate(this);
    //}

    public void GameClick()
    {
        GameController.instance.GameUpdate(this);
    }

    public void IncrementScore()
    {
        this.CellValue += 1;

        StartCoroutine(ColourFlasher(GetComponent<Image>(), Color.green));

        // display updated score
        //scoreMesh.enabled = true;
        scoreMesh.color = scoreMeshDefaultColour;
        scoreMesh.text = CellValue.ToString();
    }

    internal void ResetScore()
    {
        StartCoroutine(ColourFlasher(GetComponent<Image>(), Color.yellow));

        CellValue = 0;

        //scoreMesh.enabled = false;
        scoreMesh.color = Color.clear;
        scoreMesh.text = CellValue.ToString();
    }

    IEnumerator ColourFlasher(Image image, Color colour)
    {
        image.color = colour;
        yield return new WaitForSeconds(0.15f);
        image.color = defaultColour;
    }

    public override bool Equals(object other)
    {
        var controller = other as TileController;
        return controller != null 
                    && CellValue == controller.CellValue 
                    && X == controller.X
                    && Y == controller.Y;
    }

    public override int GetHashCode()
    {
        var hashCode = -60141616;
        hashCode = hashCode * -1521134295 + CellValue.GetHashCode();
        hashCode = hashCode * -1521134295 + X.GetHashCode();
        hashCode = hashCode * -1521134295 + Y.GetHashCode();
        return hashCode;
    }

    public class TileControllerComparer : IEqualityComparer<TileController>
    {
        public bool Equals(TileController x, TileController y)
        {
            return 
                // not null
                x != null && y != null 
                // cell value same
                && x.CellValue == y.CellValue 
                // same location
                && x.X == y.X && x.Y == y.Y;
        }

        public int GetHashCode(TileController obj)
        {
            return obj.GetHashCode();
        }
    }
}

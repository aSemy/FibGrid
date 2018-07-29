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

    //public GameController gameController;

    // Use this for initialization
    void Start()
    {
        Text[] textMeshes = GetComponentsInChildren<Text>();

        scoreMesh = textMeshes[0];
        scoreMesh.text = CellValue.ToString();
        defaultColour = GetComponent<Image>().color;
        scoreMesh.enabled = false;
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
        scoreMesh.enabled = true;
        scoreMesh.text = CellValue.ToString();
    }

    internal void ResetScore()
    {
        GetComponent<Image>().color = Color.yellow;
        StartCoroutine(ColourFlasher(GetComponent<Image>(), Color.yellow));

        CellValue = 0;

        scoreMesh.enabled = false;
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
        return controller != null &&
               base.Equals(other) &&
               CellValue == controller.CellValue;
    }

    public override int GetHashCode()
    {
        var hashCode = -60141616;
        hashCode = hashCode * -1521134295 + base.GetHashCode();
        hashCode = hashCode * -1521134295 + CellValue.GetHashCode();
        return hashCode;
    }

    public class TileControllerComparer : IEqualityComparer<TileController>
    {
        public bool Equals(TileController x, TileController y)
        {
            return x != null && y != null && x.CellValue == y.CellValue && object.Equals(x, y);
        }

        public int GetHashCode(TileController obj)
        {
            return obj.GetHashCode();
        }
    }
}

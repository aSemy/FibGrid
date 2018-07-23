using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TileController : MonoBehaviour
{

    public Text scoreMesh;
    private long cellValue = 0;

    Color defaultColour;

    //public GameController gameController;

    // Use this for initialization
    void Start()
    {
        Text[] textMeshes = GetComponentsInChildren<Text>();

        scoreMesh = textMeshes[0];
        scoreMesh.text = cellValue.ToString();
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
        cellValue += 1;

        StartCoroutine(ColourFlasher(GetComponent<Image>(), Color.green));

        // display updated score
        scoreMesh.enabled = true;
        scoreMesh.text = cellValue.ToString();
    }


    public long CellValue
    {
        get { return cellValue; }
    }

    internal void ResetScore()
    {
        GetComponent<Image>().color = Color.yellow;
        StartCoroutine(ColourFlasher(GetComponent<Image>(), Color.yellow));

        cellValue = 0;

        scoreMesh.enabled = false;
        scoreMesh.text = cellValue.ToString();
    }

    IEnumerator ColourFlasher(Image image, Color colour)
    {
        image.color = colour;
        yield return new WaitForSeconds(0.25f);
        image.color = defaultColour;
    }
}

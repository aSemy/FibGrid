using UnityEngine;

public class CanvasController : MonoBehaviour {

    private Canvas canvas;

	// Use this for initialization
	void Start () {
        canvas = this.GetComponent<Canvas>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetAxis("Mouse ScrollWheel") != 0f) // forward
        {
            if (canvas)
            {
                canvas.scaleFactor += (Input.GetAxis("Mouse ScrollWheel") / 80);
                canvas.scaleFactor = Mathf.Clamp(canvas.scaleFactor, 0.1f, 1.5f);
            }
        }
	}
}

using UnityEngine;

public class ScrollRectController : MonoBehaviour {

    private RectTransform scrollRect;

	// Use this for initialization
	void Start () {
        scrollRect = this.GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetAxis("Mouse ScrollWheel") != 0f) // forward
        {
            if (scrollRect)
            {
                scrollRect.localScale += Vector3.one * (Input.GetAxis("Mouse ScrollWheel") / 80);
                scrollRect.localScale = new Vector3(
                    Mathf.Clamp(scrollRect.localScale.x, 0.1f, 1.5f),
                    Mathf.Clamp(scrollRect.localScale.y, 0.1f, 1.5f),
                    0);
                
            }
        }
	}
}

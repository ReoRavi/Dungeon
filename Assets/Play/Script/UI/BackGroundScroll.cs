using UnityEngine;
using System.Collections;

public class BackGroundScroll : MonoBehaviour {

    public float Offset;

    public void Scroll(float scrollSpeed)
    {
        Offset += Time.deltaTime * scrollSpeed;
        GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0, Offset);
    }
}

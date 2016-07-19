using UnityEngine;
using System.Collections;

public class ApplicationManager : MonoBehaviour {

    public GameObject exitUI;

	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            exitUI.SetActive(true);
        }
    }
}

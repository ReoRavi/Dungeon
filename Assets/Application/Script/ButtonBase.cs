using UnityEngine;
using System.Collections;

public class ButtonBase : MonoBehaviour {

    public void ChangeScene(string sceneName)
    {
        Application.LoadLevel(sceneName);
    }

    public void ExitApplication()
    {
        Application.Quit();
    }

    public void HideObject(GameObject hideObject)
    {
        hideObject.SetActive(false);
    }
}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadUserData : MonoBehaviour
{
    // 통신 객체
    private HTTPManager userData;

    // 첫 접속 시, 유저 코드를 받아올 객체
    public HTTPManager userCode;
    // 첫 접속 시, 유저 닉네임을 설정할 객체
    public NickNameUI nickNameUI;

    // Use this for initialization
    void Start()
    {
        userData = GetComponent<HTTPManager>();

        // 처음 접속이라면
        if (!PlayerPrefs.HasKey("firstAccess"))
        {
            string code = userCode.HTTP_REQUEST();
            if (code == "error")
            {
                Debug.Log("Get Code Error!");
            }
            else
            {
                // 유저 코드를 받아온다.
                PlayerPrefs.SetString("UserCode", code);
                Debug.Log("Code : " + PlayerPrefs.GetString("UserCode"));

                nickNameUI.ChangeActive(UIState.normal, UIState.empty);
            }
        }
        // 접속 기록이 있다면
        else
        {
            Application.LoadLevel("Main");
        }
    }
}

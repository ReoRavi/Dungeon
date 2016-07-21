using UnityEngine;
using System.Collections;
using System.Net.Json;
using UnityEngine.UI;

public enum UIState { normal, success, error, empty };

public class NickNameUI : MonoBehaviour
{
    // 통신 객체
    private HTTPManager userData;
    // 닉네임 설정 UI들
    public GameObject[] nickNameUI;
    // 닉네임 텍스트
    public Text nickNameText;
    // 설정 오류 UI들
    public GameObject[] errorUI;
    // 설정 성공 UI들
    public GameObject[] successUI;
    // 유저 DB를 생성할 통신 객체
    public HTTPManager createUserDBData;

    void Start()
    {
        userData = GetComponent<HTTPManager>();
    }

    public void ChangeActive(UIState appear, UIState hide)
    {
        if (!(appear == UIState.empty))
        {
            switch (appear)
            {
                case UIState.normal:
                    for (int index = 0; index < nickNameUI.Length; index++)
                    {
                        nickNameUI[index].SetActive(true);
                    }

                    break;

                case UIState.success:
                    for (int index = 0; index < successUI.Length; index++)
                    {
                        successUI[index].SetActive(true);

                    }
                    break;

                case UIState.error:
                    for (int index = 0; index < errorUI.Length; index++)
                    {
                        errorUI[index].SetActive(true);
                    }

                    break;
            }
        }

        if (!(hide == UIState.empty))
        {
            switch (hide)
            {
                case UIState.normal:
                    for (int index = 0; index < nickNameUI.Length; index++)
                    {
                        nickNameUI[index].SetActive(false);
                    }

                    break;

                case UIState.success:
                    for (int index = 0; index < successUI.Length; index++)
                    {
                        successUI[index].SetActive(false);

                    }
                    break;

                case UIState.error:
                    for (int index = 0; index < errorUI.Length; index++)
                    {
                        errorUI[index].SetActive(false);
                    }

                    break;
            }
        }
    }

    public void NickNameRequest()
    {
        if (nickNameText.text.Length < 2 || nickNameText.text.Length > 10)
        {
            return;
        }

        string result = userData.HTTP_REQUEST();
        bool flag = true;

        if (!(result == "error"))
        {
            string[] userNames = userData.GetUserDataFromJson(result, "NickName");

            foreach (string userName in userNames)
            {
                if (nickNameText.text == userName)
                {
                    flag = false;

                    ChangeActive(UIState.error, UIState.normal);

                    break;
                }
            }

            // 중복된 이름이 없다면
            if (flag)
            {
                createUserDBData.datas[0] = PlayerPrefs.GetString("UserCode").ToString();
                createUserDBData.datas[1] = "\\\"" + CreateFriendCode(PlayerPrefs.GetString("UserCode").ToString()) + "\\\"";
                createUserDBData.datas[2] = "\\\"" +  nickNameText.text + "\\\"";

                if (!(createUserDBData.HTTP_REQUEST() == "error"))
                {
                    ChangeActive(UIState.success, UIState.normal);
                }
                else
                {
                    Debug.Log("create DB Data Error!");
                }
            }
        }
    }

    public void NickNameErrorButton()
    {
        nickNameText.text = "";
        ChangeActive(UIState.normal, UIState.error);
    }

    public void NickNameSuccessButton()
    {
        Application.LoadLevel("Main");
    }

    private string CreateFriendCode(string userCode)
    {
        string friendCode = "FC";

        for (int index = 10; index >= 0; index--)
        {
            if (index - userCode.Length >= 0)
            {
                friendCode += '0';
            }
            else
            {
                friendCode += userCode[index];
            }
        }

        return friendCode;
    }
}

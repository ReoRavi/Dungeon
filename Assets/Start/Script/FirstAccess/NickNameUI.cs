using UnityEngine;
using System.Collections;
using System.Net.Json;
using UnityEngine.UI;

public enum UIState { normal, facebook, success, error, empty };

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
    // 페이스북 설정 UI들
    public GameObject[] facebookUI;
    // 페이스북 설정 통신 객체
    public FaceBookStart facebookData;
    // 페이스북 아이디를 통해 계정 정보를 가져올 객체
    public HTTPManager getUserDataFromFaceBook;
    // 유저 DB를 생성할 통신 객체
    public HTTPManager createUserDBData;

    public Text DebugString;

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

                case UIState.facebook:
                    for (int index = 0; index < facebookUI.Length; index++)
                    {
                        facebookUI[index].SetActive(true);
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

                case UIState.facebook:
                    for (int index = 0; index < facebookUI.Length; index++)
                    {
                        facebookUI[index].SetActive(false);
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
            // 빈 테이블 처리
            if (result.Length == 0)
            {
                ChangeActive(UIState.facebook, UIState.normal);

                return;
            }

            DebugString.text = result;

            string[] userNames = userNames = userData.GetUserDataFromJson(result, "NickName", DebugString); ;

            DebugString.text = "NO";

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
                ChangeActive(UIState.facebook, UIState.normal);
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
        // 상태 반전
        PlayerPrefs.SetInt("firstAccess", 1);

        Application.LoadLevel("Main");
    }

    public void FaceBookOK()
    {
        facebookData.FaceBookLogin();
    }

    public void FaceBookNO()
    {
        if (!CreateUserDB("0"))
        {
            Debug.Log("Create DB Error");
        }
        else
        {
            ChangeActive(UIState.success, UIState.facebook);
        }
    }

    public bool CheckDBFromFaceBook(string faceBookKey)
    {
        string result = getUserDataFromFaceBook.HTTP_REQUEST();

        if (!(result == "error"))
        {
            if (result.Length == 0)
            {
                return false;
            }

            string[] userFaceBookKey = getUserDataFromFaceBook.GetUserDataFromJson(result, "FaceBookToken");
            string[] userCodes = getUserDataFromFaceBook.GetUserDataFromJson(result, "UserCode");

            for (int index = 0; index < userFaceBookKey.Length; index++)
            {
                if (userFaceBookKey[index] == faceBookKey)
                {
                    PlayerPrefs.SetString("UserCode", userCodes[index]);

                    return true;
                }
            }
        }

        return false;
    }

    public bool CreateUserDB(string faceBookKey)
    {
        createUserDBData.datas[0] = PlayerPrefs.GetString("UserCode").ToString();
        createUserDBData.datas[1] = "\\\"" + CreateFriendCode(PlayerPrefs.GetString("UserCode").ToString()) + "\\\"";
        createUserDBData.datas[2] = "\\\"" + nickNameText.text + "\\\"";
        createUserDBData.datas[7] = faceBookKey;

        if (!(createUserDBData.HTTP_REQUEST() == "error"))
        {
            return true;
        }
        else
        {
            Debug.Log("create DB Data Error!");

            return false;
        }
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

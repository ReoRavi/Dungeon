using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MyInfoManager : MonoBehaviour {

    // 랭킹 정보들을 받아올 객체
    public HTTPManager getInfo;
    // 유저 이름
    private string[] nickName;
    // 점수
    private string[] highScore;
    // 점수
    private string[] firstAccessTime;
    // 점수
    private string[] facebookKey;
    // UI
    public Text[] infoUI;


    // Use this for initialization
    void Start () {
        getInfo.key = PlayerPrefs.GetString("UserCode");

        string result = getInfo.HTTP_REQUEST();

        if (!(result == "error"))
        {
            nickName = getInfo.GetUserDataFromJson(result, "NickName");
            highScore = getInfo.GetUserDataFromJson(result, "HighScore");
            firstAccessTime = getInfo.GetUserDataFromJson(result, "FirstAccessTime");
            facebookKey = getInfo.GetUserDataFromJson(result, "FaceBookToken");
        }

        infoUI[0].text = "닉네임 : " + nickName[0];
        infoUI[1].text = "최고 점수 : " + highScore[0];
        infoUI[2].text = "최초 생성 시간 : " + firstAccessTime[0];

        if (facebookKey[0] == "0")
        {
            infoUI[3].text = "FaceBook연동 : " + "X";
        }
        else
        {
            infoUI[3].text = "FaceBook연동 : " + "O";
        }
    }
}

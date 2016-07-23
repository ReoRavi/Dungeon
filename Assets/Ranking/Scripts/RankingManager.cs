using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RankingManager : MonoBehaviour {

    public Text[] rankingTexts;
    public Text pageText;
    private int pageNumber;

    // 랭킹 정보들을 받아올 객체
    private HTTPManager getRanking;
    // 유저 이름
    private string[] nickNames;
    // 점수
    private string[] scores;

    // Use this for initialization
    void Start () {
        pageNumber = 1;

        getRanking = GetComponent<HTTPManager>();

        string rankingResult = getRanking.HTTP_REQUEST();

        if (rankingResult != "error" && rankingResult.Length != 0)
        {
            nickNames = getRanking.GetUserDataFromJson(rankingResult, "NickName");
            scores = getRanking.GetUserDataFromJson(rankingResult, "Score");

            ChangeText();
        }
    }

    public void PageMove(int direction)
    {
        if (pageNumber <= 1 && direction == -1)
        {
            return;
        }

        if (pageNumber >= 100 && direction == 1)
        {
            return;
        }

        pageNumber += direction;

        ChangeText();
    }

    void ChangeText()
    {
        if (nickNames == null)
        {
            return;
        }

        for (int index = 0; index < rankingTexts.Length; index++)
        {
            if (index + (5 * (pageNumber - 1)) >= nickNames.Length)
            {
                for (int exitIndex = index; exitIndex < rankingTexts.Length; exitIndex++)
                {
                    rankingTexts[exitIndex].text = "X";
                }

                break;
            }

            rankingTexts[index].text = ((5 * (pageNumber - 1)) + index + 1).ToString() + "st";
            rankingTexts[index].text += " - " + nickNames[index + (5 * (pageNumber - 1))] + " : " + scores[index + (5 * (pageNumber - 1))];
        }

        pageText.text = pageNumber.ToString();
    }
}

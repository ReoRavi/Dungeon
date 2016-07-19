using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreRenewal : MonoBehaviour {

    public Sprite[] numberImages;
    public GameObject[] number;

    public void Renewal(int score)
    {
        // 100의 자리
        number[0].GetComponent<Image>().sprite = numberImages[(score / 100)];
        // 10의 자리
        number[1].GetComponent<Image>().sprite = numberImages[(score / 10) % 10];
        // 1의 자리
        number[2].GetComponent<Image>().sprite = numberImages[((score) % 100) % 10];
    }
}

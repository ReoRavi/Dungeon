using UnityEngine;
using System.Collections;

public class GetUserCode : MonoBehaviour {
    // 통신 객체
    private HTTPManager userData;

    // Use this for initialization
    void Start () {
        userData = GetComponent<HTTPManager>();
    }
}

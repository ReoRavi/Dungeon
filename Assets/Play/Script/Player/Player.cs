using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    public GameManager gameManager;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    // 적과 충돌할 시 제거
    void OnTriggerEnter(Collider col)
    {
        if (col.transform.tag == "Enemy")
        {
            Destroy(col.gameObject);
            gameManager.GameEnd(this.gameObject);
        }
    }
}

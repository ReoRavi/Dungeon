using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    EnemyMove enemyMove;
    public float moveSpeed;

    // Use this for initialization
    void Start () {
        enemyMove = GetComponent<EnemyMove>();
        //moveSpeed = 0.05F;
    }
	
	// Update is called once per frame
	void Update () {
        enemyMove.Move(moveSpeed);
    }
}

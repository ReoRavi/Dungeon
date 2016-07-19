using UnityEngine;
using System.Collections;

public class EnemyRespawn : MonoBehaviour {

    // 적 객체들
    public GameObject[] Enemys;
    // 리스폰 좌표
    public float respawnX, respawnY, respawnZ;
    // 리스폰 거리
    public int distance;

    public void Respawn(int enemyIndex, int respawnCount, float moveSpeed)
    {
        // 보정값
        float revisionPos = 0;

        // 데몬 위치 조정, 스프라이트 에셋에 오류가 있었음.
        if (enemyIndex == 0)
        {
            revisionPos = 0.4F;
        }

        GameObject monster = (GameObject)Instantiate(Enemys[enemyIndex], new Vector3(distance * respawnCount + revisionPos, respawnY, respawnZ), Enemys[enemyIndex].transform.rotation);
        monster.GetComponent<Enemy>().moveSpeed = moveSpeed;
    }
}

using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour {

    // 플레이어 이동 거리
    public int distance;
    // 플레이어 위치 카운트,
    private int moveCount;

    void Start()
    {
        moveCount = 1;
    }

    // 이동
    public void Move(int direction)
    {
        if ((moveCount <= 0 && direction == -1) || 
            (moveCount >= 2 && direction == 1))
        {
            return;
        }

        Vector3 position = transform.position;

        transform.position = new Vector3(position.x + distance * direction, position.y, position.z);

        moveCount += direction;
    }
}

using UnityEngine;
using System.Collections;

public class EnemyMove : MonoBehaviour {

    // 적은 지속적으로 밑으로 이동한다.
    public void Move(float moveSpeed)
    {
        Vector3 position = transform.position;

        transform.position = new Vector3(position.x, position.y - moveSpeed, position.z);
    }
}

using UnityEngine;
using System.Collections;

public class DeleteZone : MonoBehaviour {

    void OnTriggerEnter(Collider col)
    {
        if (col.transform.tag == "Enemy")
        {
            Destroy(col.gameObject);
        }
    }
}

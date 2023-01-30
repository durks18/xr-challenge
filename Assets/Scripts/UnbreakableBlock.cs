using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnbreakableBlock : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            Destroy(collision.gameObject);
        }
    }
}

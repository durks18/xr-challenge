using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void onCollision(Collision collision)
    {   
        Destroy(gameObject);
    }
}

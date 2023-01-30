using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.UIElements;

public class ShootingBullet : MonoBehaviour
{
    public float timeToDestroy = 1f;
    public Transform spawnPoint;
    public GameObject Bullet;
    public float speed = 5f;
    public float force = 1f;
    public float delayInSeconds;
    public EnemyAI enemyHealth;
    public BreakableBlocks blockHealth;


    void start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            ShootBullet();
            
        }
    }

    private void ShootBullet()
    {
        GameObject cB=Instantiate(Bullet, spawnPoint.position, Bullet.transform.rotation);
        Rigidbody rig = cB.GetComponent<Rigidbody>();

        rig.AddForce(spawnPoint.forward * speed, ForceMode.Impulse);

    }

}

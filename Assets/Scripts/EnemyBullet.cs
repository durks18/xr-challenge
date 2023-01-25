using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{

    public float timeToDestroy = 2f;
    public int damage = 1;
    public GameObject Bullet;
    public Transform spawnPoint;
    public float fireRate = 3f;
    private float lastFireTime = 1f;
    public float speed = 5f;

    public void onCollisionEnter (Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
           Player playerHealth = collision.gameObject.GetComponent<Player>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(1);
            }
        }
    }

    public void ShootBullet()
    {
        if (Time.time - lastFireTime > fireRate) 
        {
            GameObject cB = Instantiate(Bullet, spawnPoint.position, Bullet.transform.rotation);
            Rigidbody rb = cB.GetComponent<Rigidbody>();
            rb.velocity= Vector3.zero;
            rb.AddForce(spawnPoint.forward * speed, ForceMode.Impulse);

            lastFireTime = Time.time;
        }
    }
}

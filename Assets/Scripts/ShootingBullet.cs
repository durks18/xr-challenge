using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ShootingBullet : MonoBehaviour
{
    public float timeToDestroy = 2f;
    public Transform spawnPoint;
    public GameObject Bullet;
    public float speed = 5f;
    public float force = 10f;


    void start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            ShootBullet();
            RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.forward, out hit))
            {
                if (hit.transform.CompareTag("Block"))
                {
                    BreakableBlocks blockHealth = hit.transform.GetComponent<BreakableBlocks>();
                    if (blockHealth != null) blockHealth.TakeDamage(1);
                }
            }
        }
    }
    
    private void ShootBullet()
    {
        GameObject cB=Instantiate(Bullet, spawnPoint.position, Bullet.transform.rotation);
        Rigidbody rig = cB.GetComponent<Rigidbody>();

        rig.AddForce(spawnPoint.forward * speed, ForceMode.Impulse);

    }

}

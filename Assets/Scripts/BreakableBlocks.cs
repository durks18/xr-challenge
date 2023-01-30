using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableBlocks : MonoBehaviour
{
    public int maxHealth = 3;
    public int currentHealth;

    [SerializeField]
    private AudioClip soundClip;


    private void Start()
    {
        currentHealth = maxHealth;
    }
    

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            Destroy(collision.gameObject);

            currentHealth--;

            if (currentHealth <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}

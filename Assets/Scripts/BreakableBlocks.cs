using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableBlocks : MonoBehaviour
{
    public int maxHealth = 3;
    int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}

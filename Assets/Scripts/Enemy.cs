using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health, maxHealth = 10.0f;
    Rigidbody2D rb;
    public bool sleeping;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        health = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        Debug.Log("OWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWW");
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Code to handle enemy death
        Destroy(gameObject);
    }
}
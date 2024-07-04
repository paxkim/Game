using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float damage = 1.0f;
    Rigidbody2D rb;
    // Transform target;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
            if (rb == null) {
            rb = gameObject.AddComponent<Rigidbody2D>();
            rb.isKinematic = true;
        }
    }


    private void OnTriggerEnter2D(Collider2D other) {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null){
            enemy.TakeDamage(damage);
            Debug.Log("I have hit a hittable");
        }


    }

}

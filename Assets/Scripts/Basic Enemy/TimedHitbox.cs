using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedHitbox : MonoBehaviour
{
    public float duration = 1.5f; // Time until the attack hits
    private SpriteRenderer spriteRenderer;
    private Collider2D hitboxCollider;
    private Color initialColor;

    void Start()
    {
        // Initialize the sprite renderer, collider, and initial color
        spriteRenderer = GetComponent<SpriteRenderer>();
        hitboxCollider = GetComponent<Collider2D>();
        initialColor = spriteRenderer.color;

        // Disable the collider initially
        hitboxCollider.enabled = false;

        // Start the darkening effect coroutine
        StartCoroutine(DarkenAndHit());
    }

    private IEnumerator DarkenAndHit()
    {
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;
            spriteRenderer.color = Color.Lerp(initialColor, Color.black, t);
            yield return null;
        }

        // Enable the collider when the darkening effect completes
        hitboxCollider.enabled = true;

        Debug.Log("Hit!");
        // The hitbox can now trigger OnTriggerEnter2D
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the hitbox collides with the player
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player hit by attack!");
        }
    }
}

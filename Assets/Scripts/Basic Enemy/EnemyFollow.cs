using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public float followDistance = 5f;
    public float moveSpeed = 2f;
    public Transform player;
    private bool canMove = true;
    private bool facingRight = true; // To track the current facing direction

    void Awake()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogError("Player object not found! Make sure the player has the 'Player' tag.");
        }
    }

    void Update()
    {
        if (player != null && canMove)
        {
            float distance = Vector2.Distance(transform.position, player.position);

            if (distance > followDistance)
            {
                Vector2 direction = (player.position - transform.position).normalized;
                transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);

                // Check if the enemy needs to flip
                if (direction.x > 0 && facingRight)
                {
                    Flip();
                }
                else if (direction.x < 0 && !facingRight)
                {
                    Flip();
                }
            }
        }
    }

    // Method to flip the enemy sprite
    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1; // Flip the x scale
        transform.localScale = theScale;
    }

    public void StopMoving()
    {
        canMove = false;
    }

    public void ResumeMoving()
    {
        canMove = true;
    }
}
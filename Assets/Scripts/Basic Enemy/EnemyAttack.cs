using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float attackRange = 3f;
    public float attackRangeModifier = 2f;
    public float attackCooldown = 2f;
    private float attackTimer;

    public float attackDuration = 1.5f;  // How long the attack takes to 

    public GameObject hitboxPrefab;
    public EnemyFollow enemyFollow;
    private bool isAttacking = false;

    void Start()
    {
        enemyFollow = GetComponent<EnemyFollow>();
        attackRange = enemyFollow.followDistance + attackRangeModifier;

        if (enemyFollow == null || enemyFollow.player == null)
        {
            Debug.LogError("EnemyFollow component or player reference not found!");
        }
    }

    void Update()
    {
        if (enemyFollow != null && enemyFollow.player != null && !isAttacking)
        {
            float distance = Vector2.Distance(transform.position, enemyFollow.player.position);

            if (distance <= attackRange && attackTimer <= 0)
            {
                StartCoroutine(Attack());
                attackTimer = attackCooldown;
            }

            attackTimer -= Time.deltaTime;
        }
    }

    private IEnumerator Attack()
    {
        Debug.Log("Attack initiated!");
        isAttacking = true;
        enemyFollow.StopMoving();

        // Calculate the direction to the player
        Vector2 directionToPlayer = (enemyFollow.player.position - transform.position).normalized;

        // Calculate the rotation needed to face the player
        float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
        Quaternion rotationToPlayer = Quaternion.Euler(0f, 0f, angle);

        // Set the position for the hitbox with the desired z value (e.g., z = 0)
        Vector3 attackPosition = new Vector3(enemyFollow.player.position.x, enemyFollow.player.position.y, 0f);

        // Instantiate the hitbox with the calculated rotation
        GameObject hitbox = Instantiate(hitboxPrefab, attackPosition, rotationToPlayer);

        yield return new WaitForSeconds(attackDuration);
        

        Debug.Log("Attack Finished!");
        Destroy(hitbox);

        isAttacking = false;
        enemyFollow.ResumeMoving();
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Enemy : MonoBehaviour
{
    public Animator animator;
    public int maxHP = 100;
    private int currentHP;
    private Collider2D enemyCollider;
    private Rigidbody2D rb;
    public Boss_Run bossRunScript; // Reference to the Boss_Run script instance

    // Start is called before the first frame update
    void Start()
    {
        currentHP = maxHP;
        enemyCollider = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;

        // Trigger the "Hurt" animation
        if (animator != null)
        {
            animator.SetTrigger("Hurt");
        }

        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        
        if (animator != null)
        {
           
            GameManager.Instance.RespawnEnemy(this.gameObject);
            bossRunScript.enabled = false;
            animator.SetBool("isDead", true);
            
        }

        // Disable the collider so the enemy cannot be interacted with by the player
        if (enemyCollider != null)
        {
            enemyCollider.enabled = false;
        }

        // Disable the Rigidbody to prevent physics interactions
        if (rb != null)
        {
            rb.velocity = Vector2.zero; // Stop any existing movement
            rb.bodyType = RigidbodyType2D.Static; // Change body type to Static to prevent movement
        }

        // Disable this script to prevent further actions
        enabled = false;
    }

    // OnTriggerEnter2D is called when the Collider2D other enters the trigger (for trigger colliders)
    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collider belongs to the player
        if (other.CompareTag("Player"))
        {
            // Disable the enemy's collider to prevent interaction with the player
            if (enemyCollider != null)
            {
                enemyCollider.enabled = false;
            }
        }
    }
}

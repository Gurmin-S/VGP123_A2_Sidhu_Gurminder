using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerCombat : MonoBehaviour
{
    SpriteRenderer sr;

    public Animator animator;
    public Transform HitBox; // Reference to the hitbox
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public int AttackDamage = 20;
    public float attackRate = 4f;
    float nextAttackTime = 0f;
    private GameManager gameManager;

    void Start()
    {
        // Initialize the SpriteRenderer component
        sr = GetComponent<SpriteRenderer>();

        // Find and store the GameManager instance
        gameManager = FindObjectOfType<GameManager>();
       
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Time.time >= nextAttackTime)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Attack();
                nextAttackTime = Time.time + 2f / attackRate;

            }

        }

        // Change the x position of the HitBox based on the flipX state
        if (!sr.flipX)
        {
            // Set x position to -1 if the sprite is not flipped
            HitBox.localPosition = new Vector3(1, HitBox.localPosition.y, HitBox.localPosition.z);
        }
        else
        {
            // Set x position to 1 if the sprite is flipped
            HitBox.localPosition = new Vector3(-1, HitBox.localPosition.y, HitBox.localPosition.z);
        }
    }
    void Attack()
    {

        animator.SetTrigger("Attack");

        // Perform the attack using the active hitbox
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(HitBox.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(AttackDamage);
        }
    }

    public void TakeDamage(int damage)
    {
        int currentHP = gameManager.GetCurrentHealth();
        currentHP -= damage;

        // Update GameManager's health
        gameManager.SetCurrentHealth(currentHP);

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

    public void Die()
    {
        if (animator != null)
        {
            animator.SetBool("isDead", true);

            SceneManager.LoadScene("End");
            this.enabled = false;
        }
    }

    public int GetCurrentHP()
    {
        return gameManager.GetCurrentHealth();
    }

    public void SetCurrentHP(int hp)
    {
        gameManager.SetCurrentHealth(hp);
    }

    void OnDrawGizmosSelected()
    {
        if (HitBox != null)
        {
            Gizmos.DrawWireSphere(HitBox.position, attackRange);
        }
    }
}

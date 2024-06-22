using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Rigidbody2D), typeof(Animator))]
public class Boss_Run : MonoBehaviour
{
    public Transform HitBox;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;
    public Transform player;
    public float Range = 10f;
    public float attackRange = 3f;

    public float attackRate = 4f; // Slow attack rate
    private float nextAttackTime = 0f;
    public int AttackDamage = 5;
    public LayerMask PlayerLayer;
    [SerializeField] private int speed = 5;

    private PlayerController playerController;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        // Find player if not already assigned
        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                player = playerObject.transform;
            }
        }

        // Assuming the player has a PlayerController component
        playerController = player.GetComponent<PlayerController>();

        if (speed <= 0)
        {
            speed = 5;
        }
    }

    void Update()
    {
        float xInput = 0f;

        // Check if player is within range
        if (Vector2.Distance(transform.position, player.position) <= Range)
        {
            // Determine direction based on player's position relative to boss's position
            if (player.position.x < rb.position.x)
            {
                xInput = -1f; // Move left
            }
            else if (player.position.x > rb.position.x)
            {
                xInput = 1f; // Move right
            }
            // Set inRange trigger to true
            anim.SetBool("inRange", true);
        }
        else
        {
            // Set inRange trigger to false if player is out of range
            anim.SetBool("inRange", false);
        }

        // Check if player is within attack range
        if (Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            // Set ATKRange trigger to true
            anim.SetBool("ATKRange", true);
        }
        else
        {
            // Set ATKRange trigger to false if player is out of attack range
            anim.SetBool("ATKRange", false);
        }

        Vector2 moveDirection = new Vector2(xInput * speed, rb.velocity.y);
        rb.velocity = moveDirection;

        // Flip sprite based on movement direction
        if (xInput != 0)
        {
            sr.flipX = (xInput < 0);
        }

        // Change the x position of the HitBox based on the flipX state
        if (!sr.flipX)
        {
            // Set x position to 1 if the sprite is not flipped
            HitBox.localPosition = new Vector3(1, HitBox.localPosition.y, HitBox.localPosition.z);
        }
        else
        {
            // Set x position to -1 if the sprite is flipped
            HitBox.localPosition = new Vector3(-1, HitBox.localPosition.y, HitBox.localPosition.z);
        }
    }

    // Method to be called by the animation event
    public void OnAttackAnimationEvent()
    {
        if (Time.time >= nextAttackTime)
        {
            Attack();
            nextAttackTime = Time.time + 1f / attackRate;
        }
    }

    void Attack()
    {
        // Perform the attack using the active hitbox
        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(HitBox.position, attackRange, PlayerLayer);

        foreach (Collider2D playerCollider in hitPlayer)
        {
            PlayerCombat playerCombat = playerCollider.GetComponent<PlayerCombat>();
            if (playerCombat != null)
            {
                playerCombat.TakeDamage(AttackDamage);

                // Check if player's HP is <= 0
                if (playerCombat.GetCurrentHP() <= 0)
                {
                    anim.SetBool("inRange", false);
                    anim.SetBool("ATKRange", false);

                    if (playerController != null)
                    {
                        playerController.enabled = false;
                    }

                    playerCombat.enabled = false;
                    this.enabled = false;
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, Range);

        if (HitBox != null)
        {
            Gizmos.DrawWireSphere(HitBox.position, attackRange);
        }
    }
}

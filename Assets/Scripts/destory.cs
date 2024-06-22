using UnityEngine;

public class Destroy : MonoBehaviour
{

    public int healthBonus = 50;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the colliding object has a tag "Player"
        if (collision.gameObject.CompareTag("Player"))
        {

            GameManager gameManager = FindObjectOfType<GameManager>();
            if (gameManager != null)
            {
                int currentHealth = gameManager.GetCurrentHealth();
                gameManager.SetCurrentHealth(currentHealth + healthBonus);
            }
            // Destroy the coin when it collides with the player
            Destroy(gameObject);
        }
    }
}

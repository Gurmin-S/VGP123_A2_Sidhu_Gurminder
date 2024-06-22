using UnityEngine;

public class Coins : MonoBehaviour
{
    public int coin = 1;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the colliding object has a tag "Player"
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager gameManager = FindObjectOfType<GameManager>();
            if (gameManager != null)
            {
                gameManager.addCoins();
            }
            // Destroy the coin when it collides with the player
            Destroy(gameObject);
        }
    }
}

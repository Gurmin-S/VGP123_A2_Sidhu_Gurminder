using UnityEngine;
using UnityEngine.SceneManagement;

public class Dead : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collision is with the player (assuming player has a specific tag)
        if (collision.gameObject.CompareTag("Player"))
        {
            // Reset the scene to "Main"
            SceneManager.LoadScene("Main");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using static System.Net.Mime.MediaTypeNames;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu; // Reference to the pause menu UI object
    [SerializeField] GameObject UnPause; // Reference to the unpause button UI object
    [SerializeField] GameObject PauseB; // Reference to the pause button UI object

    public int maxHealth = 100; // Maximum health of the player
    private int currentHealth; // Current health of the player

    public HealthBar healthBar; // Reference to the HealthBar component

    public AudioSource audioSource; // AudioSource component to play sound effects

    public static GameManager Instance;

    public GameObject enemyPrefab; 
    public Transform respawnPoint;

    public int coins = 0;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public int kill = 2;
    public void RespawnEnemy(GameObject enemy)
    {
        kill -= 1;
        if (kill == 0)
        {
            SceneManager.LoadScene("Credits");
        }
        StartCoroutine(RespawnEnemyCoroutine());
    }

    private IEnumerator RespawnEnemyCoroutine()
    {
        float respawnTime = 5.0f; // Adjust the respawn time as needed

        yield return new WaitForSeconds(respawnTime);

        // Instantiate a new enemy at the respawn point
        GameObject newEnemy = Instantiate(enemyPrefab, respawnPoint.position, respawnPoint.rotation);
    }
    void Start()
    {
        currentHealth = maxHealth; // Initialize current health to max health
        healthBar.SetMaxHealth(maxHealth); // Set the maximum health for the health bar UI
    }

    // Get the current health of the player
    public int GetCurrentHealth()
    {
        return currentHealth;
    }
    public void addCoins()
    {
        coins += 1;
    }
    // Set the current health of the player and update the health bar UI
    public void SetCurrentHealth(int health)
    {
        currentHealth = health;
        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            // Handle death or game over logic here
        }
    }

    // Inflict damage to the player
    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            // Handle death or game over logic here
        }
    }

    // Pause the game
    public void Pause()
    {
        // Activate the pause menu
        pauseMenu.SetActive(true);
        // Freeze the game
        Time.timeScale = 0;

        // Show UnPause button and hide Pause button
        UnPause.SetActive(true);
        PauseB.SetActive(false);
    }

    // Unpause the game
    public void Unpause()
    {
        // Deactivate the pause menu
        pauseMenu.SetActive(false);
        // Resume the game
        Time.timeScale = 1;

        // Show Pause button and hide UnPause button
        PauseB.SetActive(true);
        UnPause.SetActive(false);
    }

    // Load the main menu scene
    public void Home()
    {
        SceneManager.LoadScene("Menu");
        Time.timeScale = 1; // Ensure time scale is reset to normal when loading a new scene
    }

    // Exit the application
    public void Exit()
    {
#if UNITY_EDITOR
        // Stop play mode in the editor
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // Quit the application if not in the editor
        Application.Quit();
#endif
    }
}

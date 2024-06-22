using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static System.Net.Mime.MediaTypeNames;

public class Menu : MonoBehaviour
{
    public void play()
    {
        SceneManager.LoadScene("Main");

    }
    public void menu()
    {
        SceneManager.LoadScene("Menu");

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

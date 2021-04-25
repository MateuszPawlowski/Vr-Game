using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    // Variable
    private int currentScene;

    private void Start()
    {
        // Get the scene right now
        currentScene = SceneManager.GetActiveScene().buildIndex;
    }

    public void MainMenu()
    {
        // Load first scene
        SceneManager.LoadScene(0);
    }


    public void LoadNextLevel()
    {
        // Check if the scene is not scene 5
        if(currentScene != 5)
        {
            // Load the next scene in the build index
            SceneManager.LoadScene(currentScene + 1);
        }
        else
        {
            // Call method
            MainMenu();
        }
    }

    public void LoadLevel2()
    {
        // Load 2 scene
        SceneManager.LoadScene("Level 2");
    }

    public void LoadLevel3()
    {
        // Load 3 scene
        SceneManager.LoadScene("Level 3");
    }

    public void LoadLevel4()
    {
        // Load 4 scene
        SceneManager.LoadScene("Level 4");
    }

    public void LoadLevel5()
    {
        // Load 5 scene
        SceneManager.LoadScene("Level 5");
    }

    public void QuitGame()
    {
        // Exit Game
        Application.Quit();
    }
}

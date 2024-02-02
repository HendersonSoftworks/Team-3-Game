using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuActions : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Actions when start game is selected
    public void StartGame()
    {
        Debug.Log("Start game pressed");
    }

    // Open options screen
    public void OpenOptions()
    {
        Debug.Log("Open options pressed");
    }

    // Open achievements screen
    public void OpenAchievements()
    {
        Debug.Log("Open achievements pressed");
    }

    // Open credits screen
    public void OpenCredits()
    {
        Debug.Log("Open credits pressed");
    }

    // Open options screen
    public void ExitGame()
    {
        Debug.Log("Exit game pressed");
        Application.Quit();
    }
}

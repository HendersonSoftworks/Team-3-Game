using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenuActions : MonoBehaviour
{
    // Menu screens
    public GameObject startMenu;
    public GameObject pauseMenu;
    public GameObject optionsMenu;

    // First selection on different screens
    public GameObject firstSelectionPause;
    public GameObject firstSelectionOptions;

    // Control flags
    public bool isGamePaused = false;


    // Start is called before the first frame update
    void Start()
    {
        startMenu.SetActive(true);
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
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
        startMenu.SetActive(false);
        optionsMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(firstSelectionOptions);
    }

    // Open credits screen
    public void OpenCredits()
    {
        Debug.Log("Open credits pressed");
    }

    // Open options screen
    public void ExitGame()
    {
        #if UNITY_EDITOR
        if (Application.isEditor)
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
        #endif

        Debug.Log("Exit game pressed");
        Application.Quit();
    }
}

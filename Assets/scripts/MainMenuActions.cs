using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenuActions : MonoBehaviour
{
    // Game screens
    public GameObject startScreen;
    public GameObject optionsScreen;
    public GameObject creditsScreen;

    // Modal screens
    public GameObject pauseModal;

    // First selection on different screens
    public GameObject firstSelectionPause;
    public GameObject firstSelectionOptions;
    public GameObject firstSelectionCredits;

    // Control flags
    public bool isGamePaused = false;


    // Start is called before the first frame update
    void Start()
    {
        startScreen.SetActive(true);
        pauseModal.SetActive(false);
        optionsScreen.SetActive(false);
        creditsScreen.SetActive(false);
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

    // Open options menu
    public void OpenOptions()
    {
        startScreen.SetActive(false);
        optionsScreen.SetActive(true);
        EventSystem.current.SetSelectedGameObject(firstSelectionOptions);
    }

    // Open credits screen
    public void OpenCredits()
    {
        startScreen.SetActive(false);
        creditsScreen.SetActive(true);
        EventSystem.current.SetSelectedGameObject(firstSelectionCredits);
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

        Application.Quit();
    }
}

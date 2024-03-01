using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StartScreen : MonoBehaviour
{
    public GameObject gameManager;

    // Levels
    public GameObject hauntedForest;
    public GameObject castleCourtyard;
    public GameObject insideCastle;

    // Game screens
    public GameObject startScreen;
    public GameObject optionsScreen;
    public GameObject creditsScreen;

    // Modal screens
    public GameObject pauseModal;

    // First selection on different screens
    public GameObject firstSelection;
    public GameObject firstSelectionPause;
    public GameObject firstSelectionOptions;
    public GameObject firstSelectionCredits;


    // Start is called before the first frame update
    void Start()
    {
        // Activate start screen
        startScreen.SetActive(true);

        // Deactivate other screens
        pauseModal.SetActive(false);
        optionsScreen.SetActive(false);
        creditsScreen.SetActive(false);

        // Deactivate levels
        hauntedForest.SetActive(false);
        castleCourtyard.SetActive(false);
        insideCastle.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
    }

    // Back to start menu
    public void BackToStart()
    {
        startScreen.SetActive(true);
        creditsScreen.SetActive(false);
        optionsScreen.SetActive(false);
        EventSystem.current.SetSelectedGameObject(firstSelection);
    }

    // Actions when start game is selected
    public void StartGame()
    {
        startScreen.SetActive(false);
        hauntedForest.SetActive(true);

        // Start game
        gameManager.GetComponent<GameManager>().StartGame();
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StartScreen : MonoBehaviour
{
    public GameObject gameManager;

    // Levels
    public GameObject gameLevels;
    public GameObject hauntedForest;
    public GameObject castleCourtyard;
    public GameObject insideCastle;

    // Game screens
    public GameObject startScreen;
    public GameObject pauseScreen;
    public GameObject optionsScreen;
    public GameObject creditsScreen;

    // First selection on different screens
    public GameObject firstSelection;
    public GameObject firstSelectionOptions;
    public GameObject firstSelectionCredits;


    // Start is called before the first frame update
    void Start()
    {
        DeactivateLevels();
        ActivateStartScreen();
    }

    // Deactivate levels
    private void DeactivateLevels()
    {
        gameLevels.SetActive(false);
        hauntedForest.SetActive(false);
        castleCourtyard.SetActive(false);
        insideCastle.SetActive(false);
    }

    // Activate start screen
    private void ActivateStartScreen()
    {
        startScreen.SetActive(true);

        // Deactivate other screens
        pauseScreen.SetActive(false);
        optionsScreen.SetActive(false);
        creditsScreen.SetActive(false);
    }

    // Back to start menu
    public void BackToStart()
    {
        ActivateStartScreen();
        EventSystem.current.SetSelectedGameObject(firstSelection);
    }

    // Actions when start game is selected
    public void StartGame()
    {
        startScreen.SetActive(false);

        gameLevels.SetActive(true);
        hauntedForest.SetActive(true);

        // Start game
        gameManager.GetComponent<GameManager>().StartGame();
    }

    // Actions when end game is selected
    public void EndGame()
    {
        gameManager.GetComponent<GameManager>().EndGame();

        Start();
    }

    // Open options menu
    public void OpenOptions()
    {
        startScreen.SetActive(false);
        pauseScreen.SetActive(false);
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

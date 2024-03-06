using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Player Settings")]
    public int roundCount;
    public GameObject player;
    public int hitPoints;
    public GameObject[] equippedSpells;
    public float[] spellsTimersReset; // timer depends on the spell

    [Header("Enemy Settings")]
    public GameObject[] enemies;
    public GameObject[] minionPrefabs;
    public GameObject[] bossPrefabs;

    [Header("UI Settings")]
    // Player
    public Text playerHealthUI;
    public Text[] spellSlotUI;

    // Level
    [Header("Game Settings")]
    public int currentLevel;
    public int currentWave;
    public Text levelTextUI;
    public Text waveTextUI;
    // UI
    [Header("HUD & Spell Shop")]
    public GameObject shopPanelUI;
    public GameObject hudPanel;
    public GameObject spellListPanel;

    public GameObject levels;
    public GameObject hauntedForest;
    public GameObject castleCourtyard;
    public GameObject insideCastle;

    // CTRL + M + O to collapse all code regions
    // CTRL + M + L to expand all code regions

    [Header("Audio Settings")]
    public AudioClip startScreenClip;
    public AudioClip hauntedForestClip;
    public AudioClip castleCourtyardClip;
    public AudioClip insideCastleClip;


    [Header("Control flags")]
    public bool isGamePaused = false;
    public bool isGameStarted = false;
    public bool shouldPauseWhenLostFocus = true;

    [Header("Game Screens")]
    StartScreen startScreen;
    public GameObject optionsScreen;
    public GameObject pauseScreen;
    public GameObject firstSelectionPause;
    public GameObject gameOverScreen;

    [Header("Pause screen stats")]
    public Text pauseLevelTextUI;
    public Text pauseHealthTextUI;

    public int gameDifficulty = 0;
    public String[] gameDifficultyTexts = new string[] { "Easy", "Normal", "Hard" };
    public Text pauseGameDifficultyTextUI;


    PlaySounds playSounds;

    public enum WavesTypes
    {
        start,
        wisps, spiders, raves, wolves,      // Level 1, Haunted forest
        serpents, bees, hounds, dragon,     // Level 2, Castle Courtyard
        gargoyles, guards, ancientWizard    // Level 3, Inside the Castle
    }

    private void Awake()
    {
        player = Instantiate(player, Vector3.zero, Quaternion.identity);
    }

    void Start()
    {
        player.SetActive(false);

    }

    void Update()
    {
        if (isGameStarted)
        {
            if (isGamePaused)
            {
                GameIsPaused(false);
            }
            else
            {
                GameIsPlaying();
            }
        }
        else
        {
            // If ESC is hit, goes back to start screen
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                startScreen = GetComponent<StartScreen>();
                startScreen.BackToStart();
            }
        }
    }

    void GameIsPlaying()
    {
        UpdateEnemyList();

        SetPlayerUI();

        ManageEnemyDestruction();

        // If ESC is hit, pauses game and open pause menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }

    public void GameIsPaused(bool resume)
    {
        // Pause game by setting timescale to 0
        Time.timeScale = 0;

        // If ESC is hit or resume button is clicked, goes back to game
        if ((Input.GetKeyDown(KeyCode.Escape) && pauseScreen.activeInHierarchy) || resume)
        {
            UnPauseGame();
        }
        else
        {
            // If ESC is hit on other screens, close it
            if (Input.GetKeyDown(KeyCode.Escape) && !pauseScreen.activeInHierarchy)
            {
                CloseScreen();
            }
        }
    }

    public void StartGame()
    {
        player.SetActive(true);

        InitialiseWave();

        UpdateEnemyList();

        SetSpellUI();

        SetLevelWaveUI(currentLevel, currentWave);


        playSounds = GetComponent<PlaySounds>();
        playSounds.PlayMusic(hauntedForestClip);

        isGameStarted = true;
        Time.timeScale = 1;
    }

    public void PauseGame()
    {
        isGamePaused = true;

        pauseScreen.SetActive(true);
        levels.SetActive(false);
        hauntedForest.SetActive(false);
        castleCourtyard.SetActive(false);
        insideCastle.SetActive(false);

        EventSystem.current.SetSelectedGameObject(firstSelectionPause);
    }

    public void UnPauseGame()
    {
        isGamePaused = false;

        playSounds = GetComponent<PlaySounds>();
        playSounds.PlaySelectSound();

        levels.SetActive(true);
        switch (currentLevel)
        {
            case 1:
                hauntedForest.SetActive(true);
                break;
            case 2:
                castleCourtyard.SetActive(true);
                break;
            case 3:
                insideCastle.SetActive(true);
                break;
        }
        pauseScreen.SetActive(false);

        Time.timeScale = 1;
    }

    public void EndGame()
    {
        currentLevel = 0;
        currentWave = 0;

        // Cleanup scene
        foreach (var enemy in enemies)
        {
            Destroy(enemy);
        }

        foreach (var spell in GameObject.FindGameObjectsWithTag("Spell"))
        {
            Destroy(spell);
        }

        roundCount = 0;
        hitPoints = 0;

        isGamePaused = false;
        isGameStarted = false;

        player.SetActive(false);
    }

    public void RestartGame()
    {
        isGamePaused = false;

    }

    public void CloseScreen()
    {
        if (isGamePaused)
        {
            optionsScreen.SetActive(false);
            pauseScreen.SetActive(true);
            EventSystem.current.SetSelectedGameObject(firstSelectionPause);
        }
        else
        {
            startScreen = GetComponent<StartScreen>();
            startScreen.BackToStart();
        }
    }

    private void InitialiseWave()
    {
        // Reset player pos
        player.transform.position = Vector2.zero;

        currentWave++;

        // Number of enemies increase depending on wave and game difficulty
        var min = 5 + currentWave + gameDifficulty;
        var max = 10 + currentWave + (gameDifficulty * 2);

        // Spawn enemies
        SpawnMinions(min, max, minionPrefabs[0]);

        // Set UI
        SetLevelWaveUI(currentLevel, currentWave);
    }

    private void SpawnMinions(int randAmountMin, int randAmountMax, GameObject minion)
    {
        // Get random num to spawn
        int randNum = UnityEngine.Random.Range(randAmountMin, randAmountMax);

        //print("Spawning " + randNum + " " + minion.name);

        // Calc random pos for each spawned enemy and spawn it
        for (int i = 0; i < randNum; i++)
        {
            int randX = UnityEngine.Random.Range(-10, 10);
            int randY = UnityEngine.Random.Range(-10, 10);

            Vector3 randPos = new Vector3(randX, randY, 0);
            if (Vector2.Distance(player.transform.position, randPos) <= 2 )
            {
                // Move enemies if they spawn too close to player
                randX *= 2;
                randY *= 2;
                randPos = new Vector3(randX, randY, 0);
            }

            Instantiate(minion, randPos, Quaternion.identity);
        }
    }

    // Versions of this method, trying things out
    private void SetLevelWaveUI(int level, int wave)
    {
        // Waves & Levels
        if (currentWave > 0)
        {
            currentLevel = 1;
        }
        if (currentWave > 4)
        {
            currentLevel = 2;
        }
        if (currentWave > 8)
        {
            currentLevel = 3;
        }

        levelTextUI.text = "Level: " + currentLevel.ToString();
        waveTextUI.text = "Wave: " + (wave - ((currentLevel - 1) * 4)).ToString();

        switch (currentLevel)
        {
            case 1:
                pauseLevelTextUI.text = "Haunted Forest";
                hauntedForest.SetActive(true);
                castleCourtyard.SetActive(false);
                insideCastle.SetActive(false);
                break;
            case 2:
                pauseLevelTextUI.text = "Castle Courtyard";
                hauntedForest.SetActive(false);
                castleCourtyard.SetActive(true);
                insideCastle.SetActive(false);
                break;
            case 3:
                pauseLevelTextUI.text = "Inside Castle";
                hauntedForest.SetActive(false);
                castleCourtyard.SetActive(false);
                insideCastle.SetActive(true);
                break;
        }

        pauseLevelTextUI.text = pauseLevelTextUI.text + " - Wave " + (wave - ((currentLevel - 1) * 4)).ToString();
    }

    private void SetSpellUI()
    {
        for (int i = 0; i < equippedSpells.Length; i++)
        {
            if (equippedSpells[i] != null)
            {
                spellSlotUI[i].text = equippedSpells[i].name;
            }
            else
            {
                spellSlotUI[i].gameObject.SetActive(false);
            }
        }
    }

    private void SetPlayerUI()
    {
        // Health
        playerHealthUI.text = "Health: " + hitPoints.ToString();
    }

    public void UpdateEnemyList()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy"); // very bad practice to have this running every frame - should be changed

        // Check if last enemy
        if (enemies.Length <= 0)
        {
            // Last enemy defeated, open shop and pause game
            OpenShop();
        }
    }

    public void OpenShop()
    {
        Time.timeScale = 0;

        hudPanel.SetActive(false);
        spellListPanel.SetActive(false);
        shopPanelUI.SetActive(true);
    }

    public void CloseShopAndInitNextWave()
    {
        shopPanelUI.SetActive(false);
        hudPanel.SetActive(true);
        spellListPanel.SetActive(true);

        Time.timeScale = 1;

        InitialiseWave();
    }

    public void UpdateSpellTimers()
    {

    }

    public void ManageEnemyDestruction()
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i].GetComponent<Enemy>().health <= 0)
            {
                Destroy(enemies[i]);

                UpdateEnemyList();
            }
        }
    }

    public void SetGameDifficulty(int difficulty)
    {
        gameDifficulty = difficulty;
        pauseGameDifficultyTextUI.text = gameDifficultyTexts[difficulty];
    }

    // Controls option to pause game when focus is lost
    public void TogglePauseWhenLostFocus(bool enabled)
    {
        shouldPauseWhenLostFocus = enabled;
    }

    // Gets notified when application loses or gains focus
    void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
        {
            if (isGameStarted && shouldPauseWhenLostFocus)
            {
                PauseGame();
            }
        }
    }
}

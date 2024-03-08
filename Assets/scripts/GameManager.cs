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
    public int Money; // player currency
    public GameObject defaultSpellMM;
    public GameObject[] equippedSpells;
    public float[] spellsTimersReset; // timer depends on the spell

    [Header("Enemy Settings")]
    public GameObject[] enemies;
    public GameObject[] minionPrefabs;
    public GameObject[] bossPrefabs;
    // coin prefab
    public GameObject currency;

    [Header("UI Settings")]
    // Player
    public Text playerHealthUI;
    public Text[] spellSlotUI;
    public Text levelTextUI;
    public Text waveTextUI;

    // Level
    [Header("Game Settings")]
    public int currentLevel;
    public int currentWave;
    public int enemiesKilled;
    public int bossesKilled;
    public int gameDifficulty = 0;
    public String[] gameDifficultyTexts = new string[] { "Easy", "Normal", "Hard" };

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
    public AudioClip hauntedForestClip;
    public AudioClip castleCourtyardClip;
    public AudioClip insideCastleClip;

    [Header("Control flags")]
    public bool isGamePaused = false;
    public bool isGameStarted = false;
    public bool isGameOver = false;
    public bool isSpellShop = false;
    public bool shouldPauseWhenLostFocus = true;

    [Header("Game Screens")]
    StartScreen startScreen;
    public GameObject optionsScreen;
    public GameObject pauseScreen;
    public GameObject firstSelectionPause;
    public GameObject gameOverScreen;
    public GameObject confirmationModal;
    public Button yesConfirmationButton;

    [Header("Pause screen stats")]
    public Text pauseLevelTextUI;
    public Text pauseHealthTextUI;
    public Text pauseDamageTextUI;
    public Text pausePowersTextUI;
    public Text pauseCurrencyTextUI;
    public Text pauseKillsTextUI;
    public Text pauseBossesTextUI;
    public Text pauseGameDifficultyTextUI;

    [Header("Game over screen stats")]
    public Text gameOverTitleTextUI;
    public Text gameOverLevelTextUI;
    public Text gameOverWavesTextUI;
    public Text gameOverCurrencyWonTextUI;
    public Text gameOverCurrencySpentTextUI;
    public Text gameOverKillsTextUI;
    public Text gameOverBossesTextUI;
    public Text gameOverDifficultyTextUI;

    PlaySounds playSounds;

    public GameObject spellShop; // set in spellshop start func

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

        ResetSpells();
        equippedSpells[0] = defaultSpellMM;
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
            if (isGameOver)
            {
                GameIsOver(currentWave == 12);
            }
            else
            {
                // If ESC is hit, goes back to start screen
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    startScreen = GetComponent<StartScreen>();
                    startScreen.BackToStart(false);
                }
            }
        }
    }

    private void GameIsOver(bool gameWon)
    {
        if (gameWon)
        {
            gameOverTitleTextUI.text = "Congratulations!";
        }
        else
        {
            gameOverTitleTextUI.text = "Game over";
        }

        gameOverCurrencyWonTextUI.text = Money.ToString();
        gameOverCurrencySpentTextUI.text = spellShop.GetComponent<SpellShop>().currencySpent.ToString();

        gameOverKillsTextUI.text = enemiesKilled.ToString();
        gameOverBossesTextUI.text = bossesKilled.ToString();

        gameOverScreen.SetActive(true);
        DeactivateGameScreens();
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
        levels.SetActive(true);
        hauntedForest.SetActive(true);

        player.SetActive(true);

        InitialiseWave();

        UpdateEnemyList();

        SetLevelWaveUI(currentLevel, currentWave);

        SetSpellUI();

        playSounds = GetComponent<PlaySounds>();
        playSounds.PlayMusic(hauntedForestClip);

        isGameStarted = true;
        Time.timeScale = 1;
    }

    public void PauseGame()
    {
        isGamePaused = true;

        int spellsTotal = 0;
        float damageTotal = 0;
        for (int i = 0; i < equippedSpells.Length; i++)
        {
            if (equippedSpells[i] != null)
            {
                spellsTotal++;
                switch (equippedSpells[i].name)
                {
                    case "CloudBall":
                        damageTotal += 20;
                        break;
                    case "DragonBreath":
                        damageTotal += 10;
                        break;
                    case "EldritchBlast":
                        damageTotal += 35;
                        break;
                    case "FireBall":
                        damageTotal += 100;
                        break;
                    case "LightningBolt":
                        damageTotal += 20;
                        break;
                    case "MagicMissile":
                        damageTotal += 25;
                        break;
                    case "PsychicBomb":
                        damageTotal += 50;
                        break;
                    case "RayOfFrost":
                        damageTotal += 15;
                        break;
                }
            }
        }

        pauseDamageTextUI.text = damageTotal.ToString();
        pausePowersTextUI.text = spellsTotal.ToString();
        pauseKillsTextUI.text = enemiesKilled.ToString();
        pauseBossesTextUI.text = bossesKilled.ToString();
        pauseCurrencyTextUI.text = "$ " + Money.ToString();

        pauseScreen.SetActive(true);
        DeactivateGameScreens();

        EventSystem.current.SetSelectedGameObject(firstSelectionPause);
    }

    public void UnPauseGame()
    {
        isGamePaused = false;

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

    public void DeactivateGameScreens()
    {
        levels.SetActive(false);
        hauntedForest.SetActive(false);
        castleCourtyard.SetActive(false);
        insideCastle.SetActive(false);
    }

    public void ConfirmEndGame()
    {
        yesConfirmationButton.onClick.AddListener(() => { EndGame(); });
        confirmationModal.SetActive(true);
    }

    public void EndGame()
    {
        currentLevel = 0;
        currentWave = 0;

        gameOverScreen.SetActive(false);

        //enemy projectiles cleanup
        foreach (var projectile in GameObject.FindGameObjectsWithTag("Projectile"))
        {
            Destroy(projectile);
        }

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
        isGameOver = false;

        player.SetActive(false);

        startScreen = GetComponent<StartScreen>();
        startScreen.BackToStart(true);
    }

    public void ConfirmRestartGame()
    {
        yesConfirmationButton.onClick.AddListener(() => { RestartGame(); });
        confirmationModal.SetActive(true);
    }

    public void RestartGame()
    {
        confirmationModal.SetActive(false);

        EndGame();

        startScreen = GetComponent<StartScreen>();
        startScreen.BackToStart(false);
        startScreen.StartGame();

        UnPauseGame();
    }

    public void CloseModal()
    {
        confirmationModal.SetActive(false);
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
            startScreen.BackToStart(false);
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
        switch (currentWave)
        {
            case 1:
                SpawnMinions(min, max, minionPrefabs[2]);   // Wisps
                break;
            case 2:
                SpawnMinions(min, max, minionPrefabs[0]);   // Spiders
                break;
            case 3:
                SpawnMinions(min, max, minionPrefabs[1]);   // Ravens
                break;
            case 4:
                SpawnMinions(min, max, minionPrefabs[1]);   // Ravens
                SpawnBoss(bossPrefabs[0]);                  // Werewolf
                break;
            case 5:
                SpawnMinions(min, max, minionPrefabs[3]);   // Serpents
                break;
            case 6:
                SpawnMinions(min, max, minionPrefabs[0]);   // Ravens
                SpawnMinions(min, max, minionPrefabs[3]);   // Serpents
                break;
            case 7:
                SpawnBoss(bossPrefabs[1]);                  // Dragon
                SpawnMinions(min, max, minionPrefabs[3]);   // Serpents
                break;
            case 8:
                SpawnMinions(min, max, minionPrefabs[4]);   // Skeletons
                break;
            case 9:
                SpawnMinions(min, max, minionPrefabs[4]);   // Skeletons
                SpawnMinions(min, max, minionPrefabs[2]);   // Wisps
                break;
            case 10:
                SpawnMinions(min, max, minionPrefabs[4]);   // Skeletons
                SpawnMinions(min, max, minionPrefabs[2]);   // Wisps
                SpawnMinions(min, max, minionPrefabs[0]);   // spiders
                break;
            case 11:
                SpawnMinions(min, max, minionPrefabs[4]);   // Skeletons
                SpawnBoss(bossPrefabs[2]);                  // Ancient Wizard
                break;
            default:
                break;
        }

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

    private void SpawnBoss(GameObject bossPrefab)
    {
        Vector3 bossPos = new Vector3(0, 2.5f, 0);
        Instantiate(bossPrefab, bossPos, Quaternion.identity);
    }

    // Versions of this method, trying things out
    private void SetLevelWaveUI(int level, int wave)
    {
        playSounds = GetComponent<PlaySounds>();

        // Waves & Levels
        if (currentWave > 0)
        {
            currentLevel = 1;
            if (currentWave == 1)
            {
                playSounds.PlayMusic(hauntedForestClip);
            }
        }
        if (currentWave > 4)
        {
            currentLevel = 2;
            if (currentWave == 5)
            {
                playSounds.PlayMusic(castleCourtyardClip);
            }
        }
        if (currentWave > 8)
        {
            currentLevel = 3;
            if (currentWave == 9)
            {
                playSounds.PlayMusic(insideCastleClip);
            }
        }

        levelTextUI.text = "Level: " + currentLevel.ToString();
        //waveTextUI.text = "Wave: " + (wave - ((currentLevel - 1) * 4)).ToString();
        waveTextUI.text = "Wave: " + wave;

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

        gameOverWavesTextUI.text = wave.ToString();
        gameOverLevelTextUI.text = pauseLevelTextUI.text;

        //pauseLevelTextUI.text = pauseLevelTextUI.text + " - Wave " + wave - ((currentLevel - 1) * 4)).ToString();
        pauseLevelTextUI.text = pauseLevelTextUI.text + " - Wave " + wave;
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
        pauseHealthTextUI.text = hitPoints.ToString();
    }

    public void UpdateEnemyList()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy"); // very bad practice to have this running every frame - should be changed

        // Check if last enemy
        if (enemies.Length <= 0)
        {
            if (currentWave == 12)
            {
                isGameOver = true;
                isGameStarted = false;
            }
            else
            {
                // Last enemy defeated, open shop and pause game
                OpenShop();
            }
        }
    }

    public void OpenShop()
    {
        Time.timeScale = 0;

        hudPanel.SetActive(false);
        spellListPanel.SetActive(false);
        shopPanelUI.SetActive(true);

        isSpellShop = true;
    }

    public void CloseShopAndInitNextWave()
    {
        isSpellShop = false;

        shopPanelUI.SetActive(false);
        hudPanel.SetActive(true);
        spellListPanel.SetActive(true);

        // Update equipped spells to match spellshop inventory
        print(spellShop.GetComponent<SpellShop>().playerInventory);
        print(equippedSpells);

        // Update spells from SpellShop inventory
        ResetSpells();

        Time.timeScale = 1;
        InitialiseWave();
        UpdateEnemyList();
        SetSpellUI();
    }

    private void ResetSpells()
    {
        if (currentLevel < 1)
        {
            return;
        }

        // Remove all spell prefabs
        for (int i = 0; i < equippedSpells.Length; i++)
        {
            equippedSpells[i] = null;
        }

        if (player.activeInHierarchy)
        {
            // Add spell prefabs back from inventory
            for (int i = 0; i < spellShop.GetComponent<SpellShop>().playerInventory.Count; i++)
            {
                equippedSpells[i] = spellShop.GetComponent<SpellShop>().playerInventory[i].spellPrefab;
            }
        }

        spellShop.GetComponent<SpellShop>().Shuffle(spellShop.GetComponent<SpellShop>().allSpells);
    }

    public void UpdateSpellTimers()
    {
        // Don't think we'll have time to add this
    }

    public void ManageEnemyDestruction()
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i].GetComponent<enemyHealth>().health <= 0)
            {
                enemiesKilled++;

                //create a coin before deleting the enemy
                Instantiate(currency, new Vector3(enemies[i].transform.position.x, enemies[i].transform.position.y,0), transform.rotation);
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
            if (isGameStarted && !isSpellShop && shouldPauseWhenLostFocus)
            {
                PauseGame();
            }
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public Text playerHealthUI;
    public Text[] spellSlotUI;

    [Header("Game Settings")]
    public int currentLevel;
    public int currentWave;
    public Text levelTextUI;
    public Text waveTextUI;

    [Header("Control flags")]
    public bool isGamePaused = false;
    public bool isGameStarted = false;

    PlaySounds playSounds;

    // Modal screens
    public GameObject pauseModal;

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

    }


    public void StartGame()
    {
        UpdateEnemyList();

        SetSpellUI();

        SetLevelWaveUI(currentLevel, currentWave);

        InitialiseWave();

        isGameStarted = true;
    }

    private void InitialiseWave()
    {
        // Reset player pos
        player.transform.position = Vector2.zero;

        currentWave++;
        if (currentWave > 0)
        {
            currentLevel = 1;
        }
        else if (currentWave > 4)
        {
            currentLevel = 2;
        }
        else if (currentWave > 8)
        {
            currentLevel = 3;
        }

        // Spawn enemies
        switch (currentWave)
        {
            case 1:
                SpawnMinions(5, 10, minionPrefabs[0]);
                break;
            case 2:
                SpawnMinions(5, 10, minionPrefabs[0]);
                break;
            case 3:
                SpawnMinions(5, 10, minionPrefabs[0]);
                break;
            case 4:
                SpawnMinions(5, 10, minionPrefabs[0]);
                break;
            case 5:
                SpawnMinions(5, 10, minionPrefabs[0]);
                break;
            case 6:
                SpawnMinions(5, 10, minionPrefabs[0]);
                break;
            case 7:
                SpawnMinions(5, 10, minionPrefabs[0]);
                break;
            case 8:
                SpawnMinions(5, 10, minionPrefabs[0]);
                break;
            case 9:
                SpawnMinions(5, 10, minionPrefabs[0]);
                break;
            case 10:
                SpawnMinions(5, 10, minionPrefabs[0]);
                break;
            case 11:
                SpawnMinions(5, 10, minionPrefabs[0]);
                break;

            default: 
                break;
        }

        // Set UI
        SetLevelWaveUI(currentWave, currentLevel);
    }

    private void SpawnMinions(int randAmountMin, int randAmountMax, GameObject minion)
    {
        // Get random num to spawn
        int randNum = UnityEngine.Random.Range(randAmountMin, randAmountMax);

        print("Spawning " + randNum + " " + minion.name);

        // Calc random pos for each spawned enemy and spawn it
        for (int i = 0; i < randNum; i++)
        {
            int randX = UnityEngine.Random.Range(-10, 10);
            int randY = UnityEngine.Random.Range(-10, 10);
            
            Vector3 randPos = new Vector3(randX, randY, 0);

            Instantiate(minion, randPos, Quaternion.identity);
        }
    }

    // Versions of this method, trying things out
    private void SetLevelWaveUI(int level, int wave)
    {
        levelTextUI.text = "Level: " + level.ToString();
        waveTextUI.text = "Wave: " + wave.ToString();
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

    void Update()
    {
        if (isGameStarted && !isGamePaused)
        {
            UpdateEnemyList();

            SetPlayerUI();

            // If ESC is hit, pauses game and open pause menu
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Debug.Log("Game was paused");

                isGamePaused = true;

                pauseModal.SetActive(true);
            }
        }
        else
        {
            if (isGamePaused)
            {
                // If ESC is hit, goes back to start screen
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    Debug.Log("Close pause menu");
                    isGamePaused = false;

                    playSounds = GetComponent<PlaySounds>();
                    playSounds.PlaySelectSound();
                    pauseModal.SetActive(false);
                }

            }
        }
    }

    private void SetPlayerUI()
    {
        playerHealthUI.text = "Health: " + hitPoints.ToString();
    }

    public void UpdateEnemyList()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy"); // very bad practice to have this running every frame - will need to be changed
    }

    public void UpdateSpellTimers()
    {

    }
}

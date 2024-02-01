using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Player Settings")]
    public int roundCount;
    public GameObject player;
    public int hitPoints;
    public Spell[] equippedSpells;

    [Header("Enemy Settings")]
    public GameObject[] enemies;

    private void Awake()
    {
        UpdateEnemyList();
    }

    void Start()
    {

    }

    void Update()
    {
        // Test spell casting
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            print("Input.GetKeyDown(KeyCode.Mouse0)");
            Instantiate(equippedSpells[0], player.transform.position, Quaternion.identity);
        }
    }

    public void UpdateEnemyList()
    {
        enemies = null;
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        print("Found " + enemies.Length + " enemies");
    }
}

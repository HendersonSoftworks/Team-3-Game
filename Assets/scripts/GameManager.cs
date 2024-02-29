using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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



    private void Awake()
    {
        UpdateEnemyList();
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

    }

    void Update()
    {
        UpdateEnemyList();

        //Test spell casting
        //if (Input.GetKeyDown(KeyCode.Mouse0))
        //{
        //    print("Input.GetKeyDown(KeyCode.Mouse0)");
        //    GameObject newSpell = Instantiate(equippedSpells[0], player.transform.position, Quaternion.identity);
        //    print(newSpell.GetComponent<MagicMissile>().Timer);
        //}
    }

    public void UpdateEnemyList()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy"); // very bad practice to have this running every frame - will need to be changed
    }

    public void UpdateSpellTimers()
    {

    }
}

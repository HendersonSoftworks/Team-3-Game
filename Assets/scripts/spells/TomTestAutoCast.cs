using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TomTestAutoCast : MonoBehaviour
{
    [SerializeField]
    private float timerReset; // these need to converted to arrays 
    [SerializeField]
    private float timer; // timer depends on the spell
    
    private GameManager gameManager;
    private GameObject player;

    void Start()
    {
        timer = timerReset;
        gameManager = FindObjectOfType<GameManager>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            CastSpell();
            timer = timerReset;
        }
    }

    public void CastSpell()
    {
        Instantiate(gameManager.equippedSpells[0], player.transform.position, Quaternion.identity);
    }
}

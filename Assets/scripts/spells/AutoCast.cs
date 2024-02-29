using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoCast : MonoBehaviour
{
    [SerializeField]
    private float timerReset; // these need to converted to arrays 
    [SerializeField]
    private float timer; // timer depends on the spell
    [SerializeField]
    private GameManager gameManager;
    [SerializeField]
    private Spell[] spells;

    void Start()
    {
        timer = timerReset;
        gameManager = FindObjectOfType<GameManager>();
        spells = gameManager.equippedSpells;
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
        Instantiate(gameManager.equippedSpells[0], transform.position, Quaternion.identity);
    }
}

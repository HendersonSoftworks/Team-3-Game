using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoCast : MonoBehaviour
{
    public bool isCasting = false;

    [SerializeField]
    private float timer; // these need to converted to arrays 
    [SerializeField]
    private float timerReset; // these need to converted to arrays 
    [SerializeField]
    private GameManager gameManager;
    [SerializeField]
    private GameObject[] spells;
    [SerializeField]
    private List<bool> spellsCanCast; // timer depends on the spell

    // Audio
    AudioSource audioSource;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        audioSource = gameManager.GetComponent<AudioSource>();

        spells = gameManager.equippedSpells;

        timer = timerReset;
    }

    void Update()
    {
        for (int i = 0; i < gameManager.equippedSpells.Length; i++)
        {
            var spellComp = gameManager.equippedSpells[i].GetComponent<Spell>();
            if (spellComp.targetDist < spellComp.Range)
            {
                isCasting = true;
            }
            else
            {
                isCasting = false;
            }
        }

        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = timerReset;
            CastSpells(spells);
        }
    }

    public void CastSpells(GameObject[] spells)
    {
        for (int i = 0; i < spells.Length; i++)
        {
            if (spells[i] != null && spellsCanCast[i])
            {
                Instantiate(spells[i], transform.position, Quaternion.identity);
                PlayCastSound(spells[i].GetComponent<Spell>().castClip); 
            }
        }

        isCasting = true;
    }

    private void PlayCastSound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}

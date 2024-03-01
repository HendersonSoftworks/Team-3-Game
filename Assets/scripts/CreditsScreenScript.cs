using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CreditsScreenScript : MonoBehaviour
{
    // Game screens
    public GameObject startScreen;
    public GameObject creditsScreen;

    public GameObject firstSelection;

    PlaySounds playSounds;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // If ESC is hit, goes back to start screen
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            playSounds = GetComponent<PlaySounds>();
            playSounds.PlaySelectSound();
            BackToStart();
        }
    }

    // Back to start menu
    public void BackToStart()
    {
        startScreen.SetActive(true);
        creditsScreen.SetActive(false);
        EventSystem.current.SetSelectedGameObject(firstSelection);
    }
}

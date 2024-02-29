using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OptionsMenuActions : MonoBehaviour
{
    // Menu screens
    public GameObject startMenu;
    public GameObject optionsMenu;

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
        Debug.Log("Back to start pressed");
        startMenu.SetActive(true);
        optionsMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(firstSelection);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySounds : MonoBehaviour
{
    public AudioClip moveSound;
    public AudioClip selectSound;

    AudioSource sound;

    // Start is called before the first frame update
    void Start()
    {
        sound = GetComponent<AudioSource>();
    }

    // Triggers sound when users move between menu options
    public void PlayMoveSound()
    {
        sound.PlayOneShot(moveSound);
    }

    // Triggers sound when users select a menu option
    public void PlaySelectSound()
    {
        sound.PlayOneShot(selectSound);
    }

}

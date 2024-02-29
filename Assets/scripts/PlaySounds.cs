using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySounds : MonoBehaviour
{
    public AudioClip moveSound;
    public AudioClip selectSound;

    AudioSource sound;

    // Control flags
    public bool shouldPlayEffects = true;

    // Start is called before the first frame update
    void Start()
    {
        sound = GetComponent<AudioSource>();
    }

    // Controls option to play/mute sound effects
    public void TogglePlayEffects(bool enabled)
    {
        shouldPlayEffects = enabled;
    }

    // Triggers sound when users move between menu options
    public void PlayMoveSound()
    {
        if (shouldPlayEffects)
        {
            sound.PlayOneShot(moveSound);
        }
    }

    // Triggers sound when users select a menu option
    public void PlaySelectSound()
    {
        if (shouldPlayEffects)
        {
            sound.PlayOneShot(selectSound);
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaySounds : MonoBehaviour
{
    public AudioSource soundMove;
    public AudioSource soundSelect;
    public AudioSource soundMusic;

    public Slider volumeSlider;

    // Control flags
    public bool shouldPlayEffects = true;
    public bool shouldPlayMusic = true;
    public bool shouldMuteWhenLostFocus = true;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Controls option to play/mute sound effects
    public void TogglePlayEffects(bool enabled)
    {
        shouldPlayEffects = enabled;
    }

    // Controls option to play/mute the music
    public void TogglePlayMusic(bool enabled)
    {
        if (enabled)
        {
            shouldPlayMusic = true;
            soundMusic.UnPause();
            volumeSlider.enabled = true;
        }
        else
        {
            shouldPlayMusic = false;
            soundMusic.Pause();
            volumeSlider.enabled = false;
        }
    }

    // Controls option to play/mute the music
    public void ToggleMuteWhenLostFocus(bool enabled)
    {
        shouldMuteWhenLostFocus = enabled;
    }

    // Triggers sound when users move between menu options
    public void PlayMoveSound()
    {
        if (shouldPlayEffects)
        {
            soundMove.Play();
        }
    }

    // Triggers sound when users select a menu option
    public void PlaySelectSound()
    {
        if (shouldPlayEffects)
        {
            soundSelect.Play();
        }
    }

    // Starts playing music 
    public void PlayMusic()
    {
        if (shouldPlayMusic)
        {
            soundMusic.Play();
        }
    }

    // Starts playing music 
    public void PlayMusic(AudioClip music)
    {
        if (shouldPlayMusic)
        {
            soundMusic.clip = music;
            soundMusic.Play();
        }
    }
}

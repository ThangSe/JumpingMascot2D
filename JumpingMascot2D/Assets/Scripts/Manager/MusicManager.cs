using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    private float volume = .3f;
    private bool isMute;
    private AudioSource audioSource;

    private void Awake()
    {
        Instance = this;
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = volume;
    }
    public void MuteVolume()
    {
        audioSource.mute = !audioSource.mute;
    }

    public void ToggleVolume()
    {
        if (!isMute)
        {
            audioSource.mute = !audioSource.mute;
            isMute = true;
        }
        if (isMute)
        {
            audioSource.mute = !audioSource.mute;
            isMute = false;
        }
    }
}

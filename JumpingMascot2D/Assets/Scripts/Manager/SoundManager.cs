using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    [SerializeField] private AudioClipRefsSO audioClipRefsSO;
    private float volume = 1f;
    private bool isMute;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public void PlayDeadSound()
    {
        PlaySound(audioClipRefsSO.die, transform.position);
    }
    public void PlayWinningSound()
    {
        PlaySound(audioClipRefsSO.win, transform.position);
    }
    public void PlayJumpSound()
    {
        PlaySound(audioClipRefsSO.jump, transform.position);
    }
    public void PlayLandingSound()
    {
        PlaySound(audioClipRefsSO.landing, transform.position);
    }

    private void PlaySound(AudioClip[] audioClipArray, Vector3 position, int num = 0, float volume = 1f)
    {

        PlaySound(audioClipArray[num], position, volume);
    }

    private void PlaySound(AudioClip audioClip, Vector3 position, float volumeMultiplier = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volumeMultiplier * volume);
    }

    public void ToggleVolume()
    {
        if(!isMute)
        {
            volume = 0f;
            isMute = true;
        }
        if(isMute)
        {
            volume = 1f;
            isMute = false;
        }
    }
}

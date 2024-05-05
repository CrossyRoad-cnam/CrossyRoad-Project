using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{

    public AudioClip sound;

    [Range(0, 1f)]
    public float volume = 0.5f;

    [Range(0.1f, 2.5f)]
    public float pitch = 1f;

    protected AudioSource audioSource;

    private const string volumeKey = "Volume";

    public bool playOnAwake = false;

    public bool IsLoop = false;  

    public virtual void Awake()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        if (sound != null)
        {
            audioSource.clip = sound;
        }

        audioSource.volume = volume;
        audioSource.pitch = pitch;
        audioSource.loop = IsLoop;
        audioSource.playOnAwake = playOnAwake;

        LoadVolume();
        if (audioSource.playOnAwake)
        {
            Play();
        }
    }

    public void Play()
    {
        if (audioSource.clip == null)
        {
            return;
        }
        if (audioSource.isPlaying)
        {
            return;
        }
        audioSource.PlayOneShot(sound);
    }

    public void SetVolume(float newVolume)
    {
        volume = newVolume;
        audioSource.volume = volume;
        SaveVolume();
    }

    protected void SaveVolume()
    {
        PlayerPrefs.SetFloat(volumeKey, volume);
        PlayerPrefs.Save(); 
    }

    protected void LoadVolume()
    {
        if (PlayerPrefs.HasKey(volumeKey))
        {
            volume = PlayerPrefs.GetFloat(volumeKey);
            audioSource.volume = volume;
        }
    }
}

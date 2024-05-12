using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{

    public AudioClip sound;

    [Range(0, 1f)]
    public float volume = 0.5f;

    public bool isAmbient = false;

    [Range(0.1f, 2.5f)]
    public float pitch = 1f;

    protected AudioSource audioSource;

    public bool isPlayWhenGameOver;

    private const string volumeKey = "Volume";

    public bool playOnAwake = false;

    public bool IsLoop = false;

    protected GameOverManager gameOverManager;

    public virtual void Awake()
    {
        gameOverManager = FindAnyObjectByType<GameOverManager>();
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

    public virtual void Update()
    {
        if (gameOverManager != null) { 
            if (gameOverManager.GameOverScreen.active == true && !isPlayWhenGameOver)
            {
                audioSource.Stop();
            }
        }
    }

    public void PlayAll()
    {
        if (audioSource.clip == null)
        {
            return;
        }
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
            audioSource.PlayOneShot(sound);
        }
        else
            audioSource.PlayOneShot(sound);

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
        if (isAmbient)
        {
            audioSource.volume = volume / 2;
        }
        else { 
            audioSource.volume = volume;
        }
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
            if (isAmbient)
            {
                audioSource.volume = volume / 3;
            } else
            {
                audioSource.volume = volume;
            }
        }
    }
}

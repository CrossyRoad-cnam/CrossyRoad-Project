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

    private AudioSource audioSource;

    private const string volumeKey = "Volume";

    public void Awake()
    {
        gameObject.AddComponent<AudioSource>();
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = sound;
        audioSource.volume = volume;
        audioSource.pitch = pitch;
        audioSource.loop = true;
        LoadVolume();
        audioSource.Play();
    }
    public void SetVolume(float newVolume)
    {
        volume = newVolume;
        audioSource.volume = volume;
        SaveVolume();
    }

    // Sauvegarde le volume actuel dans les PlayerPrefs
    private void SaveVolume()
    {
        PlayerPrefs.SetFloat(volumeKey, volume);
        PlayerPrefs.Save(); // N'oubliez pas de sauvegarder
    }

    // Charge le volume sauvegardé depuis les PlayerPrefs
    private void LoadVolume()
    {
        if (PlayerPrefs.HasKey(volumeKey))
        {
            volume = PlayerPrefs.GetFloat(volumeKey);
            audioSource.volume = volume;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

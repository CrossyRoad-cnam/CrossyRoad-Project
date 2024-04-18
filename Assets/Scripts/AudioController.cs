using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{

    public AudioClip sound;

    [Range(0, 1f)]
    public float volume = 1f;

    [Range(0.1f, 2.5f)]
    public float pitch = 1f;

    private AudioSource audioSource;

    public void Awake()
    {
        gameObject.AddComponent<AudioSource>();
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = sound;
        audioSource.volume = volume;
        audioSource.pitch = pitch;
        audioSource.loop = true;
        audioSource.Play();
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

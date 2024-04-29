using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemiesAudioController : AudioController
{
    private GameObject player; 
    public float maxDistance = 2f;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void CheckDistanceToPlayer()
    {
        if (player != null)
        {
            float distanceZ = Mathf.Abs(transform.position.x - player.transform.position.x);
            float distanceX = Mathf.Abs(transform.position.z - player.transform.position.z);
            if (distanceZ < maxDistance)
            {
                if (!audioSource.isPlaying)
                {
                    audioSource.Play();
                }
            }
            else
            {
                if (audioSource.isPlaying)
                {
                    audioSource.Stop();
                }
            }
        }
    }

    void Update()
    {
        if (Time.timeScale == 0f)
        {
            audioSource.Stop();
        }
        else if (player != null) 
        {
            CheckDistanceToPlayer();
        }
        else
        {
            audioSource.Stop();
        }
    }

    private void OnDestroy()
    {
        audioSource.Stop();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemiesAudioController : AudioController
{
    private GameObject player; 
    public float maxDistance = 2f;
    private bool soundPlayed = false;
    private float maxdistanceX = 1f;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void CheckDistanceToPlayer()
    {
        if (player != null)
        {
            float distanceZ = Mathf.Abs( player.transform.position.z - transform.position.z );
            float distanceX = Mathf.Abs(transform.position.x - player.transform.position.x);
            int distanceXI = Mathf.FloorToInt(Mathf.Abs( distanceX) );
            int distanceZI = Mathf.FloorToInt(Mathf.Abs( distanceZ) );

            int distance = distanceXI + distanceZI;
            
            if (distanceZI <= maxDistance && distanceXI >= 0 && distanceXI <= maxdistanceX)
            {
                if (!audioSource.isPlaying && !soundPlayed)
                {
                    audioSource.PlayOneShot(sound);

                    soundPlayed = true;
                }
            }
            else
            {
                if (audioSource.isPlaying)
                {
                    audioSource.Stop();
                    soundPlayed = false;
                }
            }
        }
    }

    override public void Update()
    {
        if (Time.timeScale == 0f)
        {
            audioSource.Stop();
            soundPlayed = false;
        }

        else if (player != null) 
        {
            if (gameOverManager != null && !gameOverManager.isGameOver)
            { 
                CheckDistanceToPlayer();
            }
        }
        else
        {
            audioSource.Stop();
            soundPlayed = false;
        }
    }

    private void OnDestroy()
    {
        audioSource.Stop();
    }
}

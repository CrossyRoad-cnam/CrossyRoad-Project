using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemiesAudioController : AudioController
{
    private GameObject player; // Référence au joueur
    public float maxDistance = 3f; // Distance maximale pour entendre l'audio

    // Fonction pour vérifier la distance entre le joueur et cet objet
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    private void CheckDistanceToPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance > maxDistance)
            {
                // baisse le volume et puis ce stop
                // 
                audioSource.Stop();
            }
            else
            {
                if (!audioSource.isPlaying)
                {
                    audioSource.Play();
                }
            }
        }
    }

    // Surcharge de la méthode Update pour inclure la vérification de la distance
    void Update()
    {
        if (player != null)
        {
            CheckDistanceToPlayer();
        }
        else
        {
            // J'aimerias que le son s'arrete a la fin de la piste audio et pas brutalement ou que ca baisse le volume

            audioSource.Stop(); 
        }
    }
}

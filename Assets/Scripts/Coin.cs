using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private CoinManager coinManager;

    void Start()
    {
        coinManager = FindObjectOfType<CoinManager>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        CollectCoin(collision.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        CollectCoin(other.gameObject);
    }

    private void CollectCoin(GameObject collidedObject)
    {
        if (collidedObject == Player.Instance.gameObject)
        {
            PlaySound();
            Destroy(gameObject);
            coinManager.CollectCoin();
        }
    }

    private void PlaySound()
    {
        AudioController audio = gameObject.GetComponent<AudioController>();
        audio.Play();
    }
}

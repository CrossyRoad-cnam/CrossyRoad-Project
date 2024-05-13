using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinHelper : MonoBehaviour
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
            Destroy(gameObject);
            coinManager.CollectCoin();
        }
    }
}

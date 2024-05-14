using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : ObjectSpawner
{
    [SerializeField] private GameObject coins;
    void Start()
    {
        elements.Clear();
        elements.Add(coins);
        Launch();
    }
    internal override void SpawnObjects()
    {
        if (Random.Range(1, 5) == 1)
        {
            while (spawnPos.z < startPositionOnZ + zoneLength)
            {
                if (SpawnedElementCount < 1)
                {
                    int elementIndex = Random.Range(0, elements.Count);
                    spawnPos.z += Random.Range(minSpaceBetween, maxSpaceBetween);
                    GameObject coin = elements[elementIndex];
                    Instantiate(coin, spawnPos, coin.transform.rotation,transform);
                    SpawnedElementCount++;
                }
                else
                {
                    return;
                }
            }

        }
    }
}

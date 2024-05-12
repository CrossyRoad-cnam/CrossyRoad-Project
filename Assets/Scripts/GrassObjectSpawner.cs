using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassObjectSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> elements;
    [SerializeField] private GameObject coins;
    [SerializeField] private int minSpaceBetween = 2;
    [SerializeField] private int maxSpaceBetween = 7;
    [SerializeField] private int zoneLength = 12;
    [SerializeField] private int startPositionOnZ = -6;
    private const float SPAWN_Y = 0.75f;

    private Vector3 spawnPos;

    void Start()
    {
        DefineXAxis();
        SpawnObjects();
    }

    private void DefineXAxis()
    {
        spawnPos = new Vector3(gameObject.transform.position.x, SPAWN_Y, startPositionOnZ);
    }

    private void SpawnObjects()
    {
        bool CoinSpawned = false;
        while (spawnPos.z < startPositionOnZ + zoneLength)
        {
            int randomIndex = Random.Range(0, 5);
            if (randomIndex == 0 && !CoinSpawned)
            {
                Instantiate(coins, spawnPos, coins.transform.rotation, transform);
                CoinSpawned = true;
            }
            else
            {
                int elementIndex = Random.Range(0, elements.Count);
                Instantiate(elements[elementIndex], spawnPos, Quaternion.identity, transform);
            }

            spawnPos.z += Random.Range(minSpaceBetween, maxSpaceBetween);
        }
    }
}

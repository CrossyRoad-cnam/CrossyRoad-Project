using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassObjectSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> elements;
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
        while (spawnPos.z < startPositionOnZ + zoneLength)
        {
            int randomIndex = Random.Range(0, elements.Count);
            GameObject newObject = Instantiate(elements[randomIndex], spawnPos, Quaternion.identity);
            newObject.transform.SetParent(transform);
            spawnPos.z += Random.Range(minSpaceBetween, maxSpaceBetween);
        }
    }
}

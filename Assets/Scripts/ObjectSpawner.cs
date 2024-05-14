using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectSpawner : MonoBehaviour
{
    [SerializeField] internal List<GameObject> elements;
    [SerializeField] internal int minSpaceBetween = 2;
    [SerializeField] internal int maxSpaceBetween = 7;
    [SerializeField] internal int zoneLength = 12;
    [SerializeField] internal int startPositionOnZ = -6;
    internal const float SPAWN_Y = 0.75f;
    internal Vector3 spawnPos;
    internal int SpawnedElementCount = 0;

    void Start()
    {
        Launch();
    }

    internal void Launch()
    {
        DefineXAxis();
        SpawnObjects();
    }

    private void DefineXAxis()
    {
        spawnPos = new Vector3(gameObject.transform.position.x, SPAWN_Y, startPositionOnZ);
    }

    virtual internal void SpawnObjects()
    {
        while (spawnPos.z < startPositionOnZ + zoneLength)
        {
                int elementIndex = Random.Range(0, elements.Count);
                Instantiate(elements[elementIndex], spawnPos, Quaternion.identity, transform);
                spawnPos.z += Random.Range(minSpaceBetween, maxSpaceBetween);
                SpawnedElementCount++;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassObjectSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> elements;
    [SerializeField] private int minSpaceBetween = 2;
    [SerializeField] private int maxSpaceBetween = 5;
    [SerializeField] private int terrainLength = 30;
    [SerializeField] private int startPositionOnZ = -15;

    private Vector3 spawnPos;

    void Start()
    {
        DefineXAxis();
        SpawnObjects();
    }

    private void DefineXAxis()
    {
        spawnPos = new Vector3(gameObject.transform.position.x, 0.5f, startPositionOnZ);
    }

    private void SpawnObjects()
    {
        while (spawnPos.z < startPositionOnZ + terrainLength)
        {
            int randomIndex = Random.Range(0, elements.Count);
            GameObject newObject = Instantiate(elements[randomIndex], spawnPos, Quaternion.identity);
            newObject.transform.SetParent(transform);
            spawnPos.z += Random.Range(minSpaceBetween, maxSpaceBetween);
        }
    }
}

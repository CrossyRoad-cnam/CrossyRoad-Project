using System;
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

    private Vector3 SpawnPos;
    private System.Random myRandom = new System.Random();

    // Start is called before the first frame update
    void Start()
    {
        DefineXAxis();
        SpawnObjects();
    }

    private void DefineXAxis()
    {
        SpawnPos = new Vector3(gameObject.transform.position.x, 0.4f, startPositionOnZ);
    }

    private void SpawnObjects()
    {
        while (SpawnPos.z < startPositionOnZ + terrainLength)
        {
            int random = myRandom.Next(elements.Count - 1);
            Instantiate(elements[random], SpawnPos, Quaternion.identity);
            SpawnPos.z = SpawnPos.z + myRandom.Next(minSpaceBetween, maxSpaceBetween);
            Console.WriteLine(SpawnPos.z);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

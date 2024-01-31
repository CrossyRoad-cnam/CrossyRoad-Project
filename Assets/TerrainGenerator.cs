using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    private Vector3 currentPosition = new Vector3(0, 0, 0);

    [SerializeField] private List<GameObject> terrains = new List<GameObject>();

    private void Start()
    {
        SpawnTerrain();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            SpawnTerrain();
        }
    }

    private void SpawnTerrain()
    {
        Instantiate(terrains[Random.Range(0, terrains.Count)], currentPosition, Quaternion.identity);
        currentPosition.x++;
    }
}

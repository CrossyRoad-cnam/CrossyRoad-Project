using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    [SerializeField] private List<TerrainData> terrainData= new List<TerrainData>();
    [SerializeField] private int maxTerrainCount;

    private List<GameObject> currentTerrains = new List<GameObject>();
    private Vector3 currentPosition = Vector3.zero;

    private void Start()
    {
        for (int i = 0; i < maxTerrainCount; i++)
        {
            SpawnTerrain();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            SpawnTerrain();
        }
    }

    private void SpawnTerrain()
    {
        int whichTerrain = Random.Range(0, terrainData.Count);
        int terrainInSuccession = Random.Range(1, terrainData[whichTerrain].maxInSuccession);

        for (int i = 0; i < terrainInSuccession; i++)
        {
            GameObject terrain = Instantiate(terrainData[whichTerrain].terrain, currentPosition, Quaternion.identity);
            currentTerrains.Add(terrain);
            if (currentTerrains.Count > maxTerrainCount)
            {
                Destroy(currentTerrains[0]);
                currentTerrains.RemoveAt(0);
            }
            currentPosition.x++;
        }
    }
}

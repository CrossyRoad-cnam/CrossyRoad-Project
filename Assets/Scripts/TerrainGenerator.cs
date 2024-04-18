using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    [SerializeField] private int minDistanceFromPlayer;
    [SerializeField] private List<TerrainData> terrainDatas;
    [SerializeField] private int maxTerrainCount;
    [SerializeField] private Transform terrainHolder;

    [HideInInspector] public Vector3 currentPosition = new Vector3(0, 0, 0);

    private List<GameObject> currentTerrains = new List<GameObject>();
    private GameObject lastSpawnedPrefab = null;

    private void Start()
    {
        for (int i = 0; i < maxTerrainCount; i++)
        {
            SpawnTerrain(true, new Vector3(0, 0, 0));
        }
    }

    public void SpawnTerrain(bool isStart, Vector3 playerPosition)
    {
        if (currentPosition.x - playerPosition.x < minDistanceFromPlayer || isStart)
        {
            int whichTerrain = Random.Range(0, terrainDatas.Count);
            int maxSuccession = terrainDatas[whichTerrain].maxInSuccession;
            int terrainInSuccession = Random.Range(1, maxSuccession + 1);

            for (int i = 0; i < terrainInSuccession; i++)
            {
                GameObject terrainPrefab = ChoosePrefab(terrainDatas[whichTerrain]);
                GameObject terrain = Instantiate(terrainPrefab, currentPosition, Quaternion.identity, terrainHolder);
                currentTerrains.Add(terrain);
                lastSpawnedPrefab = terrainPrefab;

                if (!isStart && currentTerrains.Count > maxTerrainCount)
                {
                    GameObject toRemove = currentTerrains[0];
                    currentTerrains.RemoveAt(0);
                    Destroy(toRemove);
                }

                currentPosition.x++;
            }
        }
    }

    private GameObject ChoosePrefab(TerrainData terrainData)
    {
        List<GameObject> possiblePrefabs = new List<GameObject>(terrainData.possibleTerrain);
        if (possiblePrefabs.Count > 1 && lastSpawnedPrefab != null)
        {
            possiblePrefabs.Remove(lastSpawnedPrefab);
        }

        return possiblePrefabs[Random.Range(0, possiblePrefabs.Count)];
    }
}

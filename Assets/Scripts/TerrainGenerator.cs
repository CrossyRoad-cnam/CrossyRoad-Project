using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR;
using Random = UnityEngine.Random;

public class TerrainGenerator : MonoBehaviour
{
    [SerializeField] private int minDistanceFromPlayer;
    [SerializeField] private List<TerrainData> terrainDatas;
    [SerializeField] private TerrainData StartTerrains;
    [SerializeField] private int maxTerrainCount;
    [SerializeField] private Transform terrainHolder;
   [SerializeField] private int playerStartPos;

    [HideInInspector] public Vector3 currentPosition = new Vector3(0, 0, 0);

    private List<GameObject> currentTerrains = new List<GameObject>();
    private GameObject lastSpawnedPrefab = null;
    private GameObject lastSpawnedPrefabStart = null;


    private void Awake()
    {
        DefineStartPosition();
        SpawnStartingTerrains();

    }
    private void Start()
    {
        for (int i = 0; i < maxTerrainCount; i++)
        {
            SpawnTerrain(true, new Vector3(0, 0, 0));
        }
    }
    private void DefineStartPosition()
    {
        if (playerStartPos <= StartTerrains.maxInSuccession && playerStartPos >= 0)
        {
            currentPosition.x = currentPosition.x - playerStartPos;
        }
        else
        {
            throw new ArgumentOutOfRangeException("playerStartPos", playerStartPos, "La position de départ du joueur est actuellement située en dehors sa zone d'apparition (voir StartTerrains) !");
        }
    }

    private void SpawnStartingTerrains()
    {
        for (int i = 0; i < StartTerrains.maxInSuccession; i++)
        {
            List<GameObject> possiblePrefabs = new List<GameObject>(StartTerrains.possibleTerrain);
            if (possiblePrefabs.Count > 1 && lastSpawnedPrefabStart != null)
            {
                possiblePrefabs.Remove(lastSpawnedPrefabStart);
            }
            if (possiblePrefabs.Count > 0)
            {
                GameObject terrainPrefab = possiblePrefabs[i];

            GameObject terrain = Instantiate(terrainPrefab, currentPosition, Quaternion.identity, terrainHolder);
            currentTerrains.Add(terrain);
            lastSpawnedPrefabStart = terrainPrefab;
            currentPosition.x++;
            }
            else
            {
                Debug.LogWarning("Aucun préfabriqué de terrain disponible.");
            }
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

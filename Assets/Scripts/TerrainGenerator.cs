using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR;
using Random = UnityEngine.Random;

public class TerrainGenerator : MonoBehaviour
{
    [SerializeField] private int minDistanceFromPlayer;
    [SerializeField] private List<TerrainData> terrainDatas = new List<TerrainData>();
    [SerializeField] private int maxTerrainCount;
    [SerializeField] private Transform terrainHolder;
    /// <summary>
    /// Indicates which terrain the player will spawn on
    /// </summary>
    [SerializeField] private int playerStartPos;
    [SerializeField] private List<GameObject> StartTerrains;

    [HideInInspector] public Vector3 currentPosition = new Vector3(0, 0, 0);
    private List<GameObject> currentTerrains = new List<GameObject>();

    private void Start()
    {
        DefineStartPosition();
        SpawnStartingTerrains();
        for (int i = 0; i < maxTerrainCount; i++)
        {
            SpawnTerrain(true, new Vector3(0, 0, 0));
        }
    }

    private void DefineStartPosition()
    {
        if (playerStartPos <= StartTerrains.Count - 1 && playerStartPos >= 0)
        {
            currentPosition.x = currentPosition.x - playerStartPos;
        }
        else
        {
            throw new ArgumentOutOfRangeException("playerStartPos", playerStartPos, "La position de d�part du joueur est actuellement situ�e en dehors sa zone d'apparition (voir StartTerrains) !");
        }
    }

    public void SpawnStartingTerrains()
    {
        int i = 0;
        foreach (var terrain in StartTerrains)
        {
            SpawnTerrain(terrain, i++ - playerStartPos);
        }
    }
    /// <summary>
    /// Spawns a terrain to a dedicated x location
    /// </summary>
    /// <param name="terrain"></param>
    /// <param name="position_X"></param>
    public void SpawnTerrain(GameObject terrain, int position_X)
    {
        currentTerrains.Add(terrain);
        Instantiate(terrain, currentPosition, Quaternion.identity, terrainHolder);
        currentPosition.x++;
    }

    public void SpawnTerrain(bool isStart, Vector3 playerPosition)
    {
        if (currentPosition.x - playerPosition.x < minDistanceFromPlayer || (isStart))
        {
            int whichTerrain = Random.Range(0, terrainDatas.Count);
            int maxSuccession = terrainDatas[whichTerrain].maxInSuccession;
            int terrainInSuccession = Random.Range(1, maxSuccession);

            if (terrainInSuccession > maxSuccession)
            {
                terrainInSuccession = maxSuccession;
            }

            for (int i = 0; i < terrainInSuccession; i++)
            {
                GameObject terrain = Instantiate(terrainDatas[whichTerrain].possibleTerrain[Random.Range(0, terrainDatas[whichTerrain].possibleTerrain.Count)], currentPosition, Quaternion.identity, terrainHolder);
                currentTerrains.Add(terrain);

                if (!isStart)
                {
                    if (currentTerrains.Count > maxTerrainCount)
                    {
                        Destroy(currentTerrains[0].gameObject);
                        Destroy(currentTerrains[0]);
                        currentTerrains.RemoveAt(0);
                    }
                }

                currentPosition.x++;
            }
        }
    }

}
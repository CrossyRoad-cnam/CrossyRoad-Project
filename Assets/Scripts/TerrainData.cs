using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Terrain Data", menuName = "Terrain Data")]


/// <summary>
/// class TerrainData
/// </summary>
public class TerrainData : ScriptableObject
{
    public List<GameObject> possibleTerrain;
    public int maxInSuccession;
}

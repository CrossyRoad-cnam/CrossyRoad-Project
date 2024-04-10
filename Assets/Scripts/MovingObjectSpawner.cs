using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObjectSpawner : MonoBehaviour
{
    [SerializeField] private GameObject movingObject;
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private float minSeparationTime;
    [SerializeField] private float maxSeparationTime;
    [SerializeField] private bool isRightSide;

    private void Start()
    {
        StartCoroutine(SpawnObject());
    }

    private IEnumerator SpawnObject()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minSeparationTime, maxSeparationTime));
            GameObject newObject = Instantiate(movingObject, spawnPosition.position, Quaternion.identity);

            if (!isRightSide)
            {
                newObject.transform.Rotate(0, 180, 0);
            }

            StartCoroutine(DestroyOutOfBounds(newObject));
        }
    }

    private IEnumerator DestroyOutOfBounds(GameObject obj)
    {
        float terrainSize = 60f;

        float minTerrainZ = spawnPosition.position.z - terrainSize;
        float maxTerrainZ = spawnPosition.position.z + terrainSize;

        while (true)
        {
            yield return null;

            float objZ = obj.transform.position.z;
            if (objZ < minTerrainZ || objZ > maxTerrainZ)
            {
                Destroy(obj);
                yield break;
            }
        }
    }
}

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
    [SerializeField] float EventThrowAdvancePercentage = 0.3f;
    public delegate void ObjectIncomingEventHandler();
    public event ObjectIncomingEventHandler ObjectIncoming;


    private void Start()
    {
        StartCoroutine(SpawnObject());
    }

    private IEnumerator SpawnObject()
    {
        float seconds = 1f;
        while (true)
        {
            yield return new WaitForSeconds(seconds * (1 - EventThrowAdvancePercentage));
            ObjectIncoming?.Invoke();
            yield return new WaitForSeconds(seconds * EventThrowAdvancePercentage);
            GameObject newObject = Instantiate(movingObject, spawnPosition.position, movingObject.transform.rotation);
            newObject.transform.SetParent(transform, true); // Hotfix pour que les objets soient détruites avec le terrain

            if (!isRightSide)
            {
                newObject.transform.Rotate(0, 180, 0);
            }
            seconds = Random.Range(minSeparationTime, maxSeparationTime);

            StartCoroutine(DestroyOutOfBounds(newObject));
        }
    }

    private IEnumerator DestroyOutOfBounds(GameObject obj)
    {
        float terrainSize = transform.localScale.z;
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

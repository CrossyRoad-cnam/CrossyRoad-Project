using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObjectSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> movingObjects;
    [SerializeField] private Transform spawnPositionR;
    [SerializeField] private Transform spawnPositionL;
    [SerializeField] private float minSeparationTime;
    [SerializeField] private float maxSeparationTime;
    [SerializeField] private bool isFixed;
    [SerializeField] private bool isRightSide;
    [SerializeField] float EventThrowAdvancePercentage = 0.3f;
    public delegate void ObjectIncomingEventHandler();
    public event ObjectIncomingEventHandler ObjectIncoming;
    private GameObject movingObject;
    private Transform spawnPosition;

    private void Awake()
    {
        if (!isFixed) { 
            float randomValue = Random.Range(0f, 1f);
            isRightSide = (randomValue < 0.5f);
        }
        movingObject = movingObjects[Random.Range(0, movingObjects.Count)];
        spawnPosition = isRightSide ? spawnPositionR : spawnPositionL;
    }

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
        float terrainSize = Mathf.Abs(spawnPosition.position.z) * 3;
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

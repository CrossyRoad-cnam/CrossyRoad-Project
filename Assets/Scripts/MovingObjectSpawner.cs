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
        StartCoroutine(SpawnVehicle());
    }

    private IEnumerator SpawnVehicle()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minSeparationTime, maxSeparationTime));
            GameObject newObject = Instantiate(movingObject, spawnPosition.position, Quaternion.identity);

            if (!isRightSide)
            {
                newObject.transform.Rotate(0, 180, 0);
            }
        }
    }
  
}

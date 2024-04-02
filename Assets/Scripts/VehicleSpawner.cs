using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject Vehicle;
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
            GameObject newVehicle = Instantiate(Vehicle, spawnPosition.position, Quaternion.identity);

            if (!isRightSide)
            {
                newVehicle.transform.Rotate(0, 180, 0);
            }
        }
    }
  
}

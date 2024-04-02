using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject Vehicle;
    [SerializeField] private Transform spawnPos;

    private void Start()
    {
        StartCoroutine(SpawnVehicle());
    }

    private IEnumerator SpawnVehicle()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(1, 3));
            Instantiate(Vehicle, spawnPos.position, Quaternion.identity);
        }
    }
    
}

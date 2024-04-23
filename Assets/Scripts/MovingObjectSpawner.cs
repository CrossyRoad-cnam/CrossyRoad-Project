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
    /// <summary>
    /// Détermine de cb de temps d'avance l'évènement 'ObjectIncoming' est déclenché
    /// </summary>
    [SerializeField] float EventThrowAdvancePercentage = 0.3f;
    public delegate void ObjectIncomingEventHandler();
    public event ObjectIncomingEventHandler ObjectIncoming;


    private void Start()
    {
        StartCoroutine(SpawnObject());
    }

    private IEnumerator SpawnObject()
    {
        while (true)
        {
            /// MODIFICATION JEREMIE
            /// J'ai découpé ici le temps d'attente afin que l'évènement 'ObjectIncoming' (objet en mouvement en approche !) puisse être envoyé en avance
            /// Ainsi pour les rails, je sais à l'avance qu'un train arrive et je peux donc faire clignoter les feux du terrain
            /// MODIFICATION JEREMIE
            float seconds = Random.Range(minSeparationTime, maxSeparationTime);
            yield return new WaitForSeconds(seconds * (1 - EventThrowAdvancePercentage));
            ObjectIncoming?.Invoke();
            yield return new WaitForSeconds(seconds * EventThrowAdvancePercentage);
            GameObject newObject = Instantiate(movingObject, spawnPosition.position, movingObject.transform.rotation);

            // Allow the 'movingObject' to rotate, according to the 'spawnPos' values (usage: train orientation, yAngle = 180°)
            newObject.transform.Rotate(spawnPosition.rotation.x, spawnPosition.rotation.y, spawnPosition.rotation.z);

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

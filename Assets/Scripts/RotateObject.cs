using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
///  ROTATION D'OBJETS pour les rotation de nenuphare
/// </summary>
public class RotateObject : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private int rotationDirection1 = 1;
    [SerializeField] private int rotationDirection2 = -1;
    private float currentRotationSpeed;

    void Start()
    {
        int chosenDirection = Random.Range(0, 2) == 0 ? rotationDirection1 : rotationDirection2;
        currentRotationSpeed = rotationSpeed * chosenDirection;
    }

    void Update()
    {
        RotateOverTime();
    }

    private void RotateOverTime()
    {
        Quaternion deltaRotation = Quaternion.Euler(0f, currentRotationSpeed * Time.deltaTime, 0f);
        transform.rotation *= deltaRotation;
    }
}




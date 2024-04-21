using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
   [SerializeField] private float rotationSpeed = 15f;
    void Update()
    {
        RotateOverTime();
    }

    private void RotateOverTime()
    {
        Quaternion deltaRotation = Quaternion.Euler(0f, rotationSpeed * Time.deltaTime, 0f);
        transform.rotation = transform.rotation * deltaRotation;
    }

}



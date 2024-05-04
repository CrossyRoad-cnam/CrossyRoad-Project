using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private Vector3 offset;
    [SerializeField] private float smoothSpeed;
    private void Update()
    {
        if (Player.Instance != null)
            transform.position = Vector3.Lerp(transform.position, Player.Instance.transform.position + offset, smoothSpeed);
    }   
}

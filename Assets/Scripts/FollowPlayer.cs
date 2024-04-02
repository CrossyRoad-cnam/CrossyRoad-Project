using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float smoothSpeed;
    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, player.transform.position + offset, smoothSpeed);
    }   
}

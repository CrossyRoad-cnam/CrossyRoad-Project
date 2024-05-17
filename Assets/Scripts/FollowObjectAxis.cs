using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObjectAxis : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private bool followXAxis;
    [SerializeField] private bool followYAxis;
    [SerializeField] private bool followZAxis;
    [SerializeField] private float smoothSpeed;

    private float minZ = -2f; 
    private float maxZ = 2f;

    void Update()
    {
        if (player == null)
            return;

        FollowPlayerOnAxis();
    }

    /// <summary>
    /// Smoothly follow the player on the checked axis using Lerp
    /// </summary>
    private void FollowPlayerOnAxis()
    {

        Vector3 targetPosition = new Vector3(
            followXAxis ? player.transform.position.x : transform.position.x,
            followYAxis ? player.transform.position.y : transform.position.y,
            followZAxis ? Mathf.Clamp(player.transform.position.z, minZ, maxZ) : transform.position.z);

        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
    }
}
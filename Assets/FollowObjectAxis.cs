using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObjectAxis : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private bool followXAxis;
    [SerializeField] private bool followYAxis;
    [SerializeField] private bool followZAxis;

    void Update()
    {
        // si le joueur n'est pas défini, on ne fait rien
        if (player == null)
            return;

        FollowPlayerOnAxis();
    }

    /// <summary>
    /// Follow the player on the checked axis
    /// </summary>
    private void FollowPlayerOnAxis()
    {
        transform.position = new Vector3(followXAxis ? player.transform.position.x : transform.position.x, followYAxis ? player.transform.position.y : transform.position.y, followZAxis ? player.transform.position.z : transform.position.z);
    }
}

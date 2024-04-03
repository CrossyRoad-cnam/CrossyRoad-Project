using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlayer : MonoBehaviour
{
    // Handle collision when the player collides with the obstacle
    private void OnCollisionEnter(Collision collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player != null)
        {
            Debug.Log("Player collided with obstacle. GAME OVER");
            Destroy(player.gameObject);
        }
    }

    // Handle collision when the obstacle collides with the player
    private void OnTriggerEnter(Collider other)
    {
        Player player = other.gameObject.GetComponent<Player>();
        if (player != null)
        {
            Debug.Log("Obstacle collided with player. GAME OVER");
            Destroy(player.gameObject);
        }
    }
}

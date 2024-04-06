using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlayer : MonoBehaviour
{
    private GameOverManager gameOverManager;

    
    private void OnCollisionEnter(Collision collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player != null)
        {
            Debug.Log("Player killed himself. GAME OVER");
            gameOverManager.GameOver();
            Destroy(player.gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Player player = other.gameObject.GetComponent<Player>();
        if (player != null)
        {
            Debug.Log("Player was killed. GAME OVER");
            gameOverManager.GameOver();
            Destroy(player.gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlayer : MonoBehaviour
{
    private GameOverManager gameOverManager;
    private ScoreManager scoreManager;

    void Start()
    {
        gameOverManager = FindObjectOfType<GameOverManager>();
        scoreManager = FindObjectOfType<ScoreManager>();
    } 
    private void ProcessGameOver(Player player)
    {
        gameOverManager.GameOver();
        scoreManager.AddScore(new Score("player", player.scoreValue));
        scoreManager.SaveScore();
        Destroy(player.gameObject);
    }
    private void OnCollisionEnter(Collision collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player != null)
        {
            Debug.Log("Player killed himself. GAME OVER");
            ProcessGameOver(player);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Player player = other.gameObject.GetComponent<Player>();
        if (player != null)
        {
            Debug.Log("Player was killed. GAME OVER");
            ProcessGameOver(player);
        }
    }
}

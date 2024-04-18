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
        if (gameOverManager == null)
        {
            Debug.LogError("GameOverManager not found in the scene.");
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player != null)
        {
            Debug.Log("Player killed himself. GAME OVER");
            gameOverManager.GameOver();
            scoreManager.AddScore(new Score("player", player.scoreValue));
            scoreManager.SaveScore();
            Destroy(player.gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Player player = other.gameObject.GetComponent<Player>();
        if (player != null)
        {
            Debug.Log("Player was killed. GAME OVER");
            scoreManager.AddScore(new Score("player", player.scoreValue));
            scoreManager.SaveScore();
            gameOverManager.GameOver();
            Destroy(player.gameObject);
        }
    }
}

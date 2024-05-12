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
    private void Update()
    {
        CheckWaterCollision();
    }
    private void ProcessGameOver()
    {
        gameOverManager.GameOver();
        scoreManager.AddScore(new Score("player", Player.Instance.scoreValue));
        scoreManager.SaveScore();


        Player.Instance.DeathAnimation();
        Player.Instance.SetDead(true);
    }
    private void OnCollisionEnter(Collision collision)
    {
        CheckPlayerCollision(collision.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        CheckPlayerCollision(other.gameObject);
    }

    private void CheckPlayerCollision(GameObject collidedObject)
    {
        if (collidedObject == Player.Instance.gameObject)
        {
            Debug.Log("Player was killed. GAME OVER");
            ProcessGameOver();
        }
    }

    private void CheckWaterCollision()
    {
        // TODO : add if the player is on the water
        return;
    }
}

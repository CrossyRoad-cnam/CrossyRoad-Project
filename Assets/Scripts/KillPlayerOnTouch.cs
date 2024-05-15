using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class KillPlayer : MonoBehaviour
{
    private GameOverManager gameOverManager;
    private ScoreManager scoreManager;
    private string name;

    void Start()
    {
        gameOverManager = FindObjectOfType<GameOverManager>();
        scoreManager = FindObjectOfType<ScoreManager>();
    }

    void Update()
    {
        if (!Player.Instance.isDead && !Player.Instance.isRobot)
            CheckWaterBelow();
    }
    private void ProcessGameOver()
    {
        gameOverManager.GameOver();
        name = Player.Instance.isRobot ? "bot" : "player";
        int difficulty = PlayerPrefs.GetInt("Difficulty", 0); 
        scoreManager.AddScore(new Score(name, Player.Instance.scoreValue, difficulty));
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
    private void CheckWaterBelow()
    {
        RaycastHit hit;
        if (Physics.Raycast(Player.Instance.transform.position, Vector3.down, out hit,1f) && hit.collider.CompareTag("Water"))
            ProcessGameOver();
    }
}

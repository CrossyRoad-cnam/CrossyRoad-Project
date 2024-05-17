using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class KillPlayer : MonoBehaviour
{
    private GameOverManager gameOverManager;


    void Start()
    {
        gameOverManager = FindObjectOfType<GameOverManager>();
    }

    void Update()
    {
        if (!Player.Instance.isDead)
            CheckWaterBelow();
    }
    private void ProcessGameOver()
    {
        gameOverManager.GameOver();
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
        int waterCount = 0;

        float raycastDistance = 2f;

        Vector3 leftPosition = Player.Instance.transform.position - Vector3.forward * 0.5f;
        Vector3 rightPosition = Player.Instance.transform.position + Vector3.forward * 0.5f;
        Vector3 downDirection = Vector3.down;

        RaycastHit leftHit, rightHit;
        if (Physics.Raycast(leftPosition, downDirection, out leftHit, raycastDistance))
        {
            Debug.DrawRay(leftPosition, downDirection * raycastDistance, Color.green);
            if (leftHit.collider.CompareTag("Water"))
            {
                waterCount++;
            }
        }
        else
        {
            Debug.DrawRay(leftPosition, downDirection * raycastDistance, Color.red);
        }

        if (Physics.Raycast(rightPosition, downDirection, out rightHit, raycastDistance))
        {
            Debug.DrawRay(rightPosition, downDirection * raycastDistance, Color.green);
            if (rightHit.collider.CompareTag("Water"))
            {
                waterCount++;
            }
        }
        else
        {
            Debug.DrawRay(rightPosition, downDirection * raycastDistance, Color.red);
        }

        // Vérifier si les deux raycasts ont détecté de l'eau
        if (waterCount == 2)
        {
            ProcessGameOver();
        }
    }
}

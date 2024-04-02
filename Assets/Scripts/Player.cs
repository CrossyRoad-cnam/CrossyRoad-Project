using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private TerrainGenerator terrainGenerator;
    [SerializeField] private Text scoreText;

    private float initialPosition;
    private Animator animator;
    private bool isHopping;
    private int scoreValue = 0;

    private void Start()
    {
        animator = GetComponent<Animator>();
        initialPosition = transform.position.x;
        scoreValue = 0;
        UpdateScoreText();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && !isHopping)
        {
            MoveCharacter(Vector3.right);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) && !isHopping)
        {
            MoveCharacter(Vector3.left);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && !isHopping)
        {
            MoveCharacter(Vector3.forward);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && !isHopping)
        {
            MoveCharacter(Vector3.back);
        }
    }

    private void MoveCharacter(Vector3 direction)
    {
        if (CanMoveInDirection(direction))
        {
            PerformMove(direction);
            UpdateScore();
        }
    }

    private bool CanMoveInDirection(Vector3 direction)
    {
        RaycastHit hit;
        float range = 1.0f;

        if (Physics.Raycast(transform.position, direction, out hit, range))
        {
            if (hit.collider.CompareTag("Obstacle"))
            {
                Debug.Log("Obstacle detected, can't move!");
                return false;
            }
        }
        return true;
    }

    private void PerformMove(Vector3 direction)
    {
        animator.SetTrigger("hop");
        isHopping = true;
        transform.position += direction;
        terrainGenerator.SpawnTerrain(false, transform.position);
    }

    private void UpdateScore()
    {
        int currentScore = CalculateScore();
        if (currentScore > scoreValue)
        {
            scoreValue = currentScore;
            UpdateScoreText();
        }
    }

    private int CalculateScore()
    {
        float distanceMoved = transform.position.x - initialPosition;
        return Mathf.RoundToInt(distanceMoved);
    }

    private void UpdateScoreText()
    {
        scoreText.text = scoreValue.ToString();
    }

    public void FinishHop()
    {
        isHopping = false;
    }
    // Pour tester si la collision fonctionne
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log(collision.gameObject.name);
        }
    }
}

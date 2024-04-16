using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private TerrainGenerator terrainGenerator;
    [SerializeField] private Text scoreText;
    [SerializeField] private ScoreManager scoreManager;


    private float initialPosition;
    private Quaternion initialRotation;
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
        if (!isHopping)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                MoveCharacter(new Vector3(1, 0, 0));
                Debug.Log(scoreManager);
                scoreManager.AddScore(new Score("Player", 454));
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                MoveCharacter(new Vector3(-1, 0, 0));
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                MoveCharacter(new Vector3(0, 0, 1));
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                MoveCharacter(new Vector3(0, 0, -1));
            }
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
        float playerSize = Mathf.Max(transform.localScale.x, transform.localScale.z);
        float range = playerSize * 1.2f;

        if (Physics.Raycast(transform.position, direction, out hit, range))
        {
            if (hit.collider.CompareTag("Obstacle"))
            {
                return false;
            }
        }
        return true;
    }
    private void RotateCharacter(Vector3 direction)
    {
        if (direction.x > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (direction.x < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (direction.z > 0)
        {
            transform.rotation = Quaternion.Euler(0, -90, 0);
        }
        else if (direction.z < 0)
        {
            transform.rotation = Quaternion.Euler(0, 90, 0);
        }
    }
    private void PerformMove(Vector3 direction)
    {
        animator.SetTrigger("hop");
        isHopping = true;
        transform.position += direction;
        RotateCharacter(direction);
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

    private void FinishHop()
    {
        isHopping = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.GetComponent<MovingObject>() != null)
        {
            if (collision.collider.GetComponent<MovingObject>().isLog)
            {
                transform.parent = collision.collider.transform;
            }
        }
        else
        {
            transform.parent = null;
        }
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying) // Only draw in editor, not during play
        {
            float playerSize = Mathf.Max(transform.localScale.x, transform.localScale.z);
            float range = playerSize * 1f;
            Gizmos.color = Color.yellow; // Adjust color as needed
            Gizmos.DrawRay(transform.position, transform.forward * range);
        }
    }

}

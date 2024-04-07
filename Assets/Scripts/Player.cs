using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private TerrainGenerator terrainGenerator;
    [SerializeField] private Text scoreText;

    private float initialPosition;
    private Quaternion initialRotation;
    private Animator animator;
    private bool isHopping;
    private int scoreValue = 0;

    private void Start()
    {
        animator = GetComponent<Animator>();
        initialPosition = transform.position.x;
        initialRotation = transform.rotation;
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
        float range = 1.0f;

        if (Physics.Raycast(transform.position, direction, out hit, range))
        {
            if (hit.collider.CompareTag("Obstacle"))
            {
                Debug.Log("Obstacle detected, can't move! Name : " + hit.collider.name);
                return false;
            }
        }
        return true;
    }
    private void PerformMove(Vector3 direction)
    {
        animator.SetTrigger("hop");
        isHopping = true;
        Invoke("FinishHop", 0.5f);

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

}

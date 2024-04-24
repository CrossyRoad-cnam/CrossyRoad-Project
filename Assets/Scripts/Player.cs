using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private TerrainGenerator terrainGenerator;
    [SerializeField] private Text scoreText;
    [SerializeField] private GameObject ennemyPrefab;
    private float initialPosition;
    private Quaternion initialRotation;
    private Animator animator;
    private bool isHopping;
    public float scoreValue = 0;
    private float lastScore = 0;
    private GameObject ennemyInstance;
    private float idleTime = 0;
    private bool isEagleActive = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        initialPosition = transform.position.x;
        UpdateScoreText();
        ennemyInstance = Instantiate(ennemyPrefab);
        ennemyInstance.SetActive(false);
    }

    private void Update()
    {
        if (!isHopping && !isEagleActive)
        {
            HandleMovement();
            CheckIdleTime();
        }
    }

    private void HandleMovement()
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
        float range = 1f;

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
        Quaternion newRotation = Quaternion.identity;
        if (direction.x > 0)
        {
            newRotation = Quaternion.Euler(0, 0, 0);
        }
        else if (direction.x < 0)
        {
            newRotation = Quaternion.Euler(0, 180, 0);
        }
        else if (direction.z > 0)
        {
            newRotation = Quaternion.Euler(0, -90, 0);
        }
        else if (direction.z < 0)
        {
            newRotation = Quaternion.Euler(0, 90, 0);
        }
        StartCoroutine(RotateOverTime(newRotation));
    }

    private IEnumerator RotateOverTime(Quaternion newRotation)
    {
        float duration = 0.1f;
        float time = 0;
        Quaternion startRotation = transform.rotation;

        while (time < duration)
        {
            transform.rotation = Quaternion.Slerp(startRotation, newRotation, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        transform.rotation = newRotation;
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
    private void FixPlayerPosition()
    {
        Vector3 position = transform.position;
        position.x = Mathf.Round(position.x);
        position.z = Mathf.Round(position.z);
        transform.position = position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.GetComponent<MovingObject>() != null)
        {
            if (collision.collider.GetComponent<MovingObject>().isJumpable)
            {
                transform.parent = collision.collider.transform;
            }
        }
        else
        {
            transform.parent = null;
            FixPlayerPosition();
        }
    }

    private void CheckIdleTime()
    {
        if (scoreValue == lastScore)
        {
            idleTime += Time.deltaTime;
            if (idleTime >= 5.0f && !isEagleActive)
            {
                TriggerEagle();
            }
        }
        else
        {
            lastScore = scoreValue;
            idleTime = 0;
        }
    }

    private void TriggerEagle()
    {
        isEagleActive = true;
        ennemyInstance.SetActive(true);
        ennemyInstance.transform.position = transform.position + new Vector3(10, 2, 0); 
    }

    private void LateUpdate()
    {
        if (isEagleActive)
        {
            Vector3 targetPosition = transform.position;
            ennemyInstance.transform.position = Vector3.Lerp(ennemyInstance.transform.position, targetPosition, Time.deltaTime * 7);
        }
    }
}

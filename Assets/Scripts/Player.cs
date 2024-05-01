using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    /// <summary>
    /// Indicates the amount of time, the player is allow to pass inactive, in seconds
    /// </summary>
    [SerializeField] private float EagleDelay = 5.0f;
    [SerializeField] private TerrainGenerator terrainGenerator;
    [SerializeField] private Text scoreText;
    [SerializeField] private GameObject ennemyPrefab;
    [SerializeField] private Text timeText;
    [SerializeField] private Text highScoreText;
    [SerializeField] private ScoreManager scoreManager;
    private float initialPosition;
    private Quaternion initialRotation;
    private Animator animator;
    private bool isHopping;
    public float scoreValue = 0;
    private float lastScore = 0;
    private GameObject ennemyInstance;
    private float idleTime = 0;
    /// <summary>
    /// Indicates if the eagle is flying toward the player. Avoid sending multiple eagles
    /// </summary>
    private bool isEagleActive = false;
    private int backStepsCounter;

    private void Start()
    {
        animator = GetComponent<Animator>();
        initialPosition = transform.position.x;
        ennemyInstance = Instantiate(ennemyPrefab);
        ennemyInstance.SetActive(false);
    }

    private void Update()
    {
        DisplayHighScore();
        UpdateTimeText();
        if (!isHopping && !isEagleActive)
        {
            HandleMovement();
            CheckIdleTime();
        }
    }
    private void HandleMovement()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
            MoveCharacter(new Vector3(1, 0, 0), false);
        else if (Input.GetKeyDown(KeyCode.DownArrow))
            MoveCharacter(new Vector3(-1, 0, 0), true);
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
            MoveCharacter(new Vector3(0, 0, 1), false);
        else if (Input.GetKeyDown(KeyCode.RightArrow))
            MoveCharacter(new Vector3(0, 0, -1), false);
    }
    private void MoveCharacter(Vector3 direction, bool isBack)
    {
        if (CanMoveInDirection(direction))
        {
            PerformMove(direction);
            UpdateScore();
            ManageBackwardSteps(isBack);
        }
    }
    private void ManageBackwardSteps(bool isBack)
    {
        if (isBack)
        {
            backStepsCounter++;
            if (backStepsCounter >= 3)
            {
                TriggerEagle();
                backStepsCounter = 0;
            }
        }
        else
        {
            backStepsCounter = 0;
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
            newRotation = Quaternion.Euler(0, 0, 0);
        else if (direction.x < 0)
            newRotation = Quaternion.Euler(0, 180, 0);
        else if (direction.z > 0)
            newRotation = Quaternion.Euler(0, -90, 0);
        else if (direction.z < 0)
            newRotation = Quaternion.Euler(0, 90, 0);

        StartCoroutine(RotateOverTime(newRotation));
    }
    private IEnumerator RotateOverTime(Quaternion newRotation)
    {
        float duration = 0.2f;
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
            scoreValue = currentScore;
            UpdateScoreText();
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
            if (idleTime >= EagleDelay && !isEagleActive)
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
    private void CheckBackSteps()
    {
        if (backStepsCounter >= 3)
        {
            TriggerEagle();
            backStepsCounter = 0;
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
    private void UpdateTimeText()
    {
        int totalSeconds = Mathf.RoundToInt(Time.timeSinceLevelLoad);
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;

        timeText.text = "Time\n" + string.Format("{0:D2} : {1:D2}", minutes, seconds);
    }
    private void DisplayHighScore()
    {
        int highScore = scoreManager.GetHighestScore();
        highScoreText.text = "Top " + highScore;
        Debug.Log(highScore);
    }

    // TO DO : Séparer les logiques d'affichage et de gestion de score de cette classe
}

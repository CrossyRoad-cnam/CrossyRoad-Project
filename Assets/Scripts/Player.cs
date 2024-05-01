using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    [SerializeField] private TerrainGenerator terrainGenerator;
    private float initialPosition;
    private Quaternion initialRotation;
    private Animator animator;
    private bool isHopping;
    public float scoreValue = 0;
    private float lastScore = 0;
    private float idleTime = 0;
    private bool isEnnemyActive = false;
    private int backStepsCounter;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        animator = GetComponent<Animator>();
        initialPosition = transform.position.x;
    }
    private void Update()
    {
        if (!isHopping && !isEnnemyActive && Time.timeScale != 0)
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
            scoreValue = currentScore;
    }
    private int CalculateScore()
    {
        float distanceMoved = transform.position.x - initialPosition;
        return Mathf.RoundToInt(distanceMoved);
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
            if (idleTime >= 5.0f && !isEnnemyActive)
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
        isEnnemyActive = true;

    }
    public bool CheckEnnemyTrigger()
    {
        return isEnnemyActive;
    }
    // TO DO : Séparer les logiques d'affichage et de gestion de score de cette classe
}

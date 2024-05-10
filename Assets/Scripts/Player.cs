using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    [SerializeField] private TerrainGenerator terrainGenerator;
    public bool eagleEnable = false;
    public GameObject currentSkin;
    public Transform playerContainer;
    public bool isRobot = false;
    private SkinController skinController;
    private float initialPosition;
    private bool isHopping;
    public float scoreValue = 0;
    private float lastScore = 0;
    private float idleTime = 0;
    private const float IDLE_TIME_LIMIT = 7.0f;
    private const int MAX_BACKSTEPS = 3;
    private const float ANIMATION_TIME = 0.15f;
    private bool isEnnemyActive = false;
    private int backStepsCounter;
    private bool hasFirstMoved = false;
    private Vector3 forward = new Vector3(1, 0, 0);
    private Vector3 backward = new Vector3(-1, 0, 0);
    private Vector3 left = new Vector3(0, 0, 1);
    private Vector3 right = new Vector3(0, 0, -1);


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            DestroyImmediate(gameObject, true);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        skinController = SkinController.Instance;
        initialPosition = transform.position.x;
        currentSkin = playerContainer.GetChild(0).gameObject;
        GetSkin();
    }

    private void Update()
    {
        if (!isHopping && !isEnnemyActive && Time.timeScale != 0)
        {
            if (isRobot)
                HandleRobotMovement();
            else
                HandleMovement();

            if (hasFirstMoved)
                CheckIdleTime();
        }
    }
    private void HandleMovement()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
            MoveCharacter(forward, false);
        else if (Input.GetKeyDown(KeyCode.DownArrow))
            MoveCharacter(backward, true);
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
            MoveCharacter(left, false);
        else if (Input.GetKeyDown(KeyCode.RightArrow))
            MoveCharacter(right, false);
    }
    private void MoveCharacter(Vector3 direction, bool isBack)
    {
        if (CanMoveInDirection(direction))
        {
            if (!hasFirstMoved)
                hasFirstMoved = true;
            PerformMove(direction);
            ManageBackwardSteps(isBack);
        }
    }
    private void ManageBackwardSteps(bool isBack)
    {
        if (isBack)
        {
            backStepsCounter++;
            if (backStepsCounter >= MAX_BACKSTEPS && eagleEnable)
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
                return false;
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
        float time = 0;
        Quaternion startRotation = playerContainer.rotation;

        while (time < ANIMATION_TIME)
        {
            playerContainer.rotation = Quaternion.Slerp(startRotation, newRotation, time / ANIMATION_TIME);
            time += Time.deltaTime;
            yield return null;
        }

        playerContainer.rotation = newRotation;
    }
    private void PerformMove(Vector3 direction)
    {
        isHopping = true;
        StartCoroutine(SmoothMove(transform.position, transform.position + direction));
        RotateCharacter(direction);
        terrainGenerator.SpawnTerrain(false, transform.position);
    }

    private IEnumerator SmoothMove(Vector3 startPosition, Vector3 endPosition, float duration = ANIMATION_TIME)
    {
        float timeElapsed = 0;
        float height = 0.5f;
        float playerContainerInitialY = playerContainer.position.y;

        while (timeElapsed < duration)
        {
            float animationTime = timeElapsed / duration;
            float jumpArc = height * Mathf.Sin(Mathf.PI * animationTime);
            Vector3 horizontalPosition = Vector3.Lerp(startPosition, endPosition, animationTime);
            transform.position = new Vector3(horizontalPosition.x, startPosition.y, horizontalPosition.z);
            playerContainer.position = new Vector3(horizontalPosition.x, playerContainerInitialY + jumpArc, horizontalPosition.z);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = endPosition;
        playerContainer.position = new Vector3(endPosition.x, playerContainerInitialY, endPosition.z);
        FinishHop();
        UpdateScore();
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
            if (idleTime >= IDLE_TIME_LIMIT && !isEnnemyActive && eagleEnable)
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
    public void ApplySkin(GameObject skin)
    {
        if (currentSkin != null)
        {
            Destroy(currentSkin);
        }
        currentSkin = Instantiate(skin, playerContainer);
    }

    private void GetSkin()

    {
        int selectedSkin = PlayerPrefs.GetInt("SelectedSkin", 0);
            ApplySkin(skinController.skins[selectedSkin]);
    }
    public bool HasMoved()
    {
        return hasFirstMoved;
    }

    private void HandleRobotMovement()
    {
        if (CanRobotMoveInDirection(forward))
        {
            MoveCharacter(forward, false);
        }
        else
        {
            WaitForSeconds wait = new WaitForSeconds(0.5f);
            if (CanRobotMoveInDirection(left) && CanRobotMoveInDirection(right)) // TODO : rajouter ici une optimisation s'il peut se déplacer sur les deux côtés, prioriser le movement qui se rapproche du centre. A optimiser
            // Si possible, tester cette fonctionnalité
            {
                float distanceToLeft = Vector3.Distance(transform.position + left, Vector3.zero);
                float distanceToRight = Vector3.Distance(transform.position + right, Vector3.zero);
                if (distanceToLeft < distanceToRight)
                    MoveCharacter(left, false);
                else
                    MoveCharacter(right, false);
            }
            else if (CanRobotMoveInDirection(left))
                MoveCharacter(left, false);
            else if (CanRobotMoveInDirection(right))
                MoveCharacter(right, false);
            else if (CanRobotMoveInDirection(backward))
                MoveCharacter(backward, true);
        }
    }

    private bool CanRobotMoveInDirection(Vector3 direction)
    {
        RaycastHit hit;
        float range = 1f;
        if(!CanMoveInDirection(direction)) // la règle qu'on utilise de base pour le Player humain
            return false;

        if (Physics.Raycast(transform.position + direction, Vector3.down, out hit, range))
        {
            if (hit.collider.CompareTag("Ennemy"))
                return false;
        }

        if (Physics.Raycast(transform.position, direction, out hit, range))
        {
            if (hit.collider.GetComponent<MovingObject>() != null && !hit.collider.GetComponent<MovingObject>().isJumpable)
                return false;
        }
        return true;
    }
}

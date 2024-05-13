using System.Collections;
using System.Collections.Generic;
using System.Threading;
using GLTF.Schema;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    [SerializeField] private TerrainGenerator terrainGenerator;
    public GameObject currentSkin;
    public Transform playerContainer;
    public bool isRobot = false;
    private SkinController skinController;
    private Animator animator;
    public bool eagleEnable = false;
    private bool isEnnemyActive = false;
    private float initialPosition;
    private bool isHopping;
    public float scoreValue = 0;
    private float lastScore = 0;
    private float idleTime = 0;
    private const float IDLE_TIME_LIMIT = 7.0f;
    private const int MAX_BACKSTEPS = 3;
    private const float ANIMATION_TIME = 0.15f;
    private int backStepsCounter;
    private bool hasFirstMoved = false;
    private static readonly Vector3 forward = new Vector3(1, 0, 0);
    private static readonly Vector3 backward = new Vector3(-1, 0, 0);
    private static readonly Vector3 left = new Vector3(0, 0, 1);
    private static readonly Vector3 right = new Vector3(0, 0, -1);
    public bool isDead {get; private set;} = false;
    private Vector3 raycastDirection = forward;
    


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            DestroyImmediate(gameObject, true);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (PlayerPrefs.HasKey("Eagle"))
            eagleEnable = PlayerPrefs.GetInt("Eagle") == 1;
        if (PlayerPrefs.HasKey("Robot"))
            isRobot = PlayerPrefs.GetInt("Robot") == 1;
    }
    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        skinController = SkinController.Instance;
        initialPosition = transform.position.x;
        currentSkin = playerContainer.GetChild(0).gameObject;
        GetSkin();
    }

    private void Update()
    {
        if (!isHopping && !isEnnemyActive && Time.timeScale != 0 && !isDead)
        {
            if (isRobot)
                HandleRobotMovement();
            else
                HandleMovement();

            if (hasFirstMoved)
                CheckIdleTime();
        }
        
        RayDraw();
    }
    private void HandleMovement()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            MoveCharacter(forward);
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            MoveCharacter(backward, true);
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            MoveCharacter(left);
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            MoveCharacter(right);
    }
    private void MoveCharacter(Vector3 direction, bool isBack = false)
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
        AudioController audio = gameObject.GetComponent<AudioController>();
        audio.PlayAll();
        StartCoroutine(SmoothMove(transform.position, transform.position + direction));
        RotateCharacter(direction);
        terrainGenerator.SpawnTerrain(false, transform.position);
    }

    private IEnumerator SmoothMove(Vector3 startPosition, Vector3 endPosition, float duration = ANIMATION_TIME)
    {
        float timeElapsed = 0;
        float height = 0.5f;

        while (timeElapsed < duration)
        {
            float animationTime = timeElapsed / duration;
            float jumpArc = height * Mathf.Sin(Mathf.PI * animationTime);
            transform.position = new Vector3(Vector3.Lerp(startPosition, endPosition, animationTime).x, startPosition.y + jumpArc, Vector3.Lerp(startPosition, endPosition, animationTime).z);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = endPosition;
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

    private void ApplySkin(GameObject skin)
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
    public void DeathAnimation()
    {
        animator.Play("Death");
    }
    public void SetDead(bool isDead)
    {
        this.isDead = isDead;
    }
    // ROBOT
    public void SetRobot(bool isRobot)
    {
        this.isRobot = isRobot;
    }
    private void HandleRobotMovement()
    {
        if (CanRobotMoveInDirection(forward))
        {
            MoveCharacter(forward);
            return;
        }
        else
        {
            if (CanRobotMoveInDirection(left) && CanRobotMoveInDirection(right))
            {
                float distanceToLeft = Vector3.Distance(transform.position + left, Vector3.zero);
                float distanceToRight = Vector3.Distance(transform.position + right, Vector3.zero);
                MoveCharacter(distanceToLeft < distanceToRight ? left : right);
            }
            else if (CanRobotMoveInDirection(left))
                MoveCharacter(left);
            else if (CanRobotMoveInDirection(right))
                MoveCharacter(right);
            else if (CanRobotMoveInDirection(backward))
                MoveCharacter(backward, true);
        }
    }

    private bool CanRobotMoveInDirection(Vector3 direction)
    {
        RaycastHit hit;
        float range = 1f;
        
        if(!CanMoveInDirection(direction))
            return false;

        if (Physics.Raycast(transform.position + direction, Vector3.down, out hit, range))
        {
            if (hit.collider.CompareTag("Water"))
                return false;
        }
        if (Physics.Raycast(transform.position, direction, out hit, range))
        {
            if (hit.collider.CompareTag("Ennemy"))
                return false;
        }
        return true;
    }

    private void RayDraw() // TEST de Raycast rotation
    {
        // CETTE fonction c'est que pour tester le raycast, le fonctionnement est à implémenter sur le robotMovement
        float rotationSpeed = 0f;
        float angle = rotationSpeed * Time.deltaTime;
        Quaternion rotation = Quaternion.Euler(0, angle, 0);
        raycastDirection = rotation * raycastDirection;
        RaycastHit hit;
        float range = 20f;
        Vector3 halfScale = transform.localScale / 2;
        Debug.DrawRay(transform.position + forward, right * range, Color.black);
        Debug.DrawRay(transform.position + forward, left * range, Color.black);
        Debug.DrawRay(transform.position, right * range, Color.red);
        Debug.DrawRay(transform.position, left * range, Color.red);
        Debug.DrawRay((transform.position - new Vector3(0, 0, halfScale.z)), forward, Color.red);
        Debug.DrawRay((transform.position - new Vector3(0, 0, -halfScale.z)), forward, Color.red);
        bool detectRight = Physics.Raycast(transform.position + forward, right, out hit, range);
        bool detectLeft = Physics.Raycast(transform.position + forward, left, out hit, range);
        
        if (detectLeft || detectRight)
            if (hit.collider.CompareTag("Ennemy"))
            {
                Vector3 ennemyPosition = hit.collider.transform.position;
                Vector3 destinationPosition = transform.position;
                float ennemySpeed = hit.collider.GetComponent<MovingObject>().speed;
                float distanceToPosition = Vector3.Distance(ennemyPosition, destinationPosition);
                float timeToDestination = distanceToPosition / ennemySpeed;
                Debug.Log(timeToDestination);
                //Debug.Log(hit.collider.name + " - POSITION: " + ennemyPosition + " - VITESSE: " + ennemySpeed);

                if (timeToDestination > 0.75f)
                {
                    Debug.Log("SAFE");
                }
                else
                {
                    Thread.Sleep(1000);
                    Debug.Log("DANGER");
                }
            }
                
        // INFO
        // SI position augmente => les véhicules vont à droite SINON les véhicules vont à gauche
        // On peut augmenter la range du raycast pour détecter plus tôt
        // De préférence, on fera un raycast par vérification (obstacle, eau, objets mouvants)
    }
}

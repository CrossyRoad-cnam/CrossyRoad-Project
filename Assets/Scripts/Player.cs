using System.Collections;
using System.Collections.Generic;
using System.Threading;
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
    private Coroutine smoothMoveCoroutine; // Ajoutez une variable pour stocker la coroutine en cours


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
        if (!isHopping) {
            if (smoothMoveCoroutine != null)
            {
                return;
            }
            AudioController audio = gameObject.GetComponent<AudioController>();
            audio.PlayAll();
            smoothMoveCoroutine = StartCoroutine(SmoothMove(transform.position, transform.position + direction));
            RotateCharacter(direction);
            terrainGenerator.SpawnTerrain(false, transform.position);
        }
    }

    private IEnumerator SmoothMove(Vector3 startPosition, Vector3 endPosition, float duration = ANIMATION_TIME)
    {
        isHopping = true;
        Transform parent = transform.parent;
        Vector3 initialParentPosition = parent != null ? parent.position : Vector3.zero;

        float timeElapsed = 0;
        float height = 0.5f;

        while (timeElapsed < duration)
        {
            if (parent != null)
            {
                Vector3 parentDelta = parent.position - initialParentPosition;
                startPosition += parentDelta;
                endPosition += parentDelta;
                initialParentPosition = parent.position;
            }

            float animationTime = timeElapsed / duration;
            float jumpArc = height * Mathf.Sin(Mathf.PI * animationTime);
            Vector3 newPos = Vector3.Lerp(startPosition, endPosition, animationTime);
            transform.position = new Vector3(newPos.x, startPosition.y + jumpArc, newPos.z);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        if (parent != null)
        {
            Vector3 parentDelta = parent.position - initialParentPosition;
            startPosition += parentDelta;
            endPosition += parentDelta;
        }

        smoothMoveCoroutine = null;
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
            FixPlayerPosition();
            transform.parent = null;
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
            ApplySkin(skinController.skins[selectedSkin].GetSkin());
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

        if (CanRobotMoveInDirection(left) && CanRobotMoveInDirection(right))
        {
            float distanceToLeft = Vector3.Distance(transform.position + left, Vector3.zero);
            float distanceToRight = Vector3.Distance(transform.position + right, Vector3.zero);
            MoveCharacter(distanceToLeft < distanceToRight ? left : right);
        }
        // detection des objets a gauche et a droite, la plus grande distance avec l'objet gagne si rien gauche
        else if (CanRobotMoveInDirection(left))
        {
            MoveCharacter(left);
        }
        else if (CanRobotMoveInDirection(right))
        {
            MoveCharacter(right);
        }
        else if (CanRobotMoveInDirection(backward))
        {
            MoveCharacter(backward, true);
        }
    }

    private bool CanRobotMoveInDirection(Vector3 direction)
    {
        return !IsObstacleAhead(direction) && !IsWaterAhead(direction) && !IsMovingObjectAhead(direction) && !IsTrainAhead(direction);
    }

    private bool IsObstacleAhead(Vector3 direction)
    {
        RaycastHit hit;
        float range = 1f;
        Vector3 halfScale = transform.localScale / 2;
        Ray[] rays = {
            new Ray(transform.position, direction),
            new Ray(transform.position + new Vector3(0, 0, halfScale.z), direction),
            new Ray(transform.position + new Vector3(0, 0, -halfScale.z), direction)
        };
        foreach (var ray in rays)
        {
            if (Physics.Raycast(ray, out hit, range) && hit.collider.CompareTag("Obstacle"))
            {
                return true;
            }
        }
        return false;
    }
    private bool IsTrainAhead(Vector3 direction)
    {
        RaycastHit hit;
        float range = 1f;
        return Physics.Raycast(transform.position + direction, Vector3.down, out hit, range) && hit.collider.CompareTag("Rail") && IsRailSignalOn(direction);
    }

    private bool IsWaterAhead(Vector3 direction)
    {

        int waterCount = 0;

        float raycastDistance = 2f;
        bool rectifieFirst = false;
        bool rectifie = false;

        Vector3 rightPosition = (Player.Instance.transform.position+direction) - Vector3.forward * 0.5f;
        Vector3 middlePosition = (Player.Instance.transform.position + direction);
        Vector3 leftPosition = (Player.Instance.transform.position+direction) + Vector3.forward * 0.5f;
        Vector2 actualPosition = Player.Instance.transform.position;
        Vector3 downDirection = Vector3.down;
        // recuperer le gameObject devant le joueur. 


        RaycastHit leftHit, rightHit, hit, middlehit, actualHit;

        if (direction == Vector3.forward) { 
            if (Physics.Raycast(actualPosition, downDirection, out actualHit, raycastDistance) && actualHit.collider.CompareTag("Water"))
            {
                    bool isRightSide;
                    MovingObjectSpawner movingObjectSpawn = actualHit.collider.GetComponent<MovingObjectSpawner>();
                    if (movingObjectSpawn != null)
                    {
                        isRightSide = movingObjectSpawn.isRightSide;
                        if (isRightSide && !rectifieFirst)
                        {
                            Debug.Log("de droite a gauche");
                        middlePosition -= (Vector3.right * 0.3f);
                        rightPosition -= (Vector3.right * 0.3f);
                        leftPosition -= (Vector3.right * 0.3f);
                        rectifieFirst = true;

                        }
                        else if (!isRightSide && !rectifieFirst)
                        {
                            Debug.Log("de gauche a droite");
                        middlePosition += (Vector3.right * 0.3f);
                        rightPosition += (Vector3.right * 0.3f);
                        leftPosition += (Vector3.right * 0.3f);
                        rectifieFirst = true;
                        }
                    
                }
            }
            if (Physics.Raycast(middlePosition, downDirection, out hit, raycastDistance) && hit.collider.CompareTag("Water"))
            {
                    bool isRightSide;
                    MovingObjectSpawner movingObjectSpawn = hit.collider.GetComponent<MovingObjectSpawner>();
                    if (movingObjectSpawn != null)
                    {
                        isRightSide = movingObjectSpawn.isRightSide;
                        if (isRightSide && !rectifie)
                        {
                            Debug.Log("de droite a gauche");
                            middlePosition += (Vector3.right * 0.3f);
                            rightPosition += (Vector3.right * 0.3f);
                            leftPosition += (Vector3.right * 0.3f);
                            rectifie = true;
                        }
                        else if(!isRightSide && !rectifie)
                        {
                            Debug.Log("de gauche a droite");
                            middlePosition -= (Vector3.right * 0.3f);
                            rightPosition -= (Vector3.right * 0.3f);
                            leftPosition -= (Vector3.right * 0.3f);
                            rectifie = true;
                        }

                    }
                }

        }

        if (Physics.Raycast(middlePosition, downDirection, out middlehit, raycastDistance))
        {
            if (direction == Vector3.forward) { 
            Debug.DrawRay(middlePosition, downDirection * raycastDistance, Color.blue);
        }

            if (middlehit.collider.CompareTag("Water"))
            {
                waterCount++;
            }
        }


        if (Physics.Raycast(leftPosition, downDirection, out leftHit, raycastDistance))
        {
            if (direction == Vector3.forward)
            {
                Debug.DrawRay(leftPosition, downDirection * raycastDistance, Color.magenta);
            }
            if (leftHit.collider.CompareTag("Water"))
            {
                waterCount++;
            }
        }
        if (Physics.Raycast(rightPosition, downDirection, out rightHit, raycastDistance))
        {
            if (direction == Vector3.forward) { 
            Debug.DrawRay(rightPosition, downDirection * raycastDistance, Color.yellow);
            }
            if (rightHit.collider.CompareTag("Water"))
            {
                waterCount++;
            }
        }

        return waterCount >= 1;
    }

    private bool IsMovingObjectAhead(Vector3 direction)
    {
        Vector3 halfScale = transform.localScale / 2;
        float frontBackRange = 1f;
        float sideRange = 6f;

        Ray[] frontBackRays = {
            new Ray(transform.position + new Vector3(0, 0, halfScale.z), direction),
            new Ray(transform.position + new Vector3(0, 0, -halfScale.z), direction),
        };

        Ray[] sideRays = {
            new Ray(transform.position + new Vector3(0, 0, halfScale.z), left),
            new Ray(transform.position + new Vector3(0, 0, -halfScale.z), left),
            new Ray(transform.position + new Vector3(1, 0, halfScale.z), left),
            new Ray(transform.position + new Vector3(1, 0, -halfScale.z), left),
            new Ray(transform.position + new Vector3(-1, 0, halfScale.z), left),
            new Ray(transform.position + new Vector3(-1, 0, -halfScale.z), left),
            new Ray(transform.position + new Vector3(0, 0, halfScale.z), right),
            new Ray(transform.position + new Vector3(0, 0, -halfScale.z), right),
            new Ray(transform.position + new Vector3(1, 0, halfScale.z), right),
            new Ray(transform.position + new Vector3(1, 0, -halfScale.z), right),
            new Ray(transform.position + new Vector3(-1, 0, halfScale.z), right),
            new Ray(transform.position + new Vector3(-1, 0, -halfScale.z), right)
        };


        foreach (var ray in frontBackRays)
        {
            DrawRays(ray, frontBackRange);
            if (Physics.Raycast(ray, out RaycastHit hit, frontBackRange) && hit.collider.CompareTag("Ennemy") && IsEnemyApproaching(hit, direction))
            {
                    return true;
            }
        }

        foreach (var ray in sideRays)
        {
            DrawRays(ray, sideRange);
            if (Physics.Raycast(ray, out RaycastHit hit, sideRange) && hit.collider.CompareTag("Ennemy") && IsEnemyApproaching(hit, direction))
            {
                    return true;
            }
        }

        return false;
    }

    private bool IsEnemyApproaching(RaycastHit hit, Vector3 direction)
    {
        if (!hit.collider)
        {
            return false;
        }

        MovingObject movingObject = hit.collider.GetComponent<MovingObject>();
        if (!movingObject)
        {
            return false;
        }

        Vector3 enemyPosition = movingObject.transform.position;
        float enemySpeed = movingObject.speed;
        float distanceToPosition = Vector3.Distance(enemyPosition, transform.position + direction);
        float timeToDestination = distanceToPosition / enemySpeed;

        return timeToDestination <= 0.5f;
    }

    private void DrawRays(Ray ray, float range)
    {
        Debug.DrawRay(ray.origin, ray.direction * range, Color.red);
    }

    private bool IsRailSignalOn(Vector3 direction)
    {
        RaycastHit hit;
        float range = 1f;
        if (Physics.Raycast(transform.position + direction, Vector3.down, out hit, range))
        {
            if (hit.collider.CompareTag("Rail"))
            {
                RailwayLightingSystem railwayLightingSystem = hit.collider.GetComponent<RailwayLightingSystem>();
                if (railwayLightingSystem)
                {
                    return railwayLightingSystem.IsLightOn;
                }
            }
        }
        return false;
    }

}

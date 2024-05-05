using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailwayLightingSystem : MonoBehaviour
{
    private MovingObjectSpawner movingObjectSpawner;
    [SerializeField] private Light railwayLight1;
    [SerializeField] private Light railwayLight2;
    [SerializeField] private float blinkDuration = 2f;
    [SerializeField] private float blinkingTime = 0.4f;
    private AudioController audioController;
    private GameObject player;
    private const int maxDistance = 2;
    private bool isLightOn = true;
    private Coroutine blinkCoroutine;

    void Awake()
    {
        audioController = GetComponent<AudioController>();
        player = GameObject.FindGameObjectWithTag("Player");

    }
    void Start()
    {
        try
        {
            movingObjectSpawner = GetComponent<MovingObjectSpawner>();
            movingObjectSpawner.ObjectIncoming += StartBlinking; // Subscribe to the event to start blinking
        }
        catch (System.Exception)
        {
            throw;
        }
    }

    private void CheckDistanceToPlayer()
    {
        if (player != null)
        {
            float distanceX = Mathf.Abs(transform.position.x - player.transform.position.x  );
            // arrondi 
            int distance = Mathf.RoundToInt(distanceX);
            if (distance <= maxDistance && distanceX >= 0 )
            {
                if (audioController != null)
                {
                    audioController.Play();
                }
            }
        }
        else
        {
            Debug.LogWarning("Player object is null.");
        }
    }


    void OnDisable()
    {
        if (movingObjectSpawner != null)
            movingObjectSpawner.ObjectIncoming -= StartBlinking; // Unsubscribe safely
        StopBlinking(); // Stop blinking when disabled
    }

    private void StartBlinking()
    {
        if (blinkCoroutine != null) StopCoroutine(blinkCoroutine);
        blinkCoroutine = StartCoroutine(BlinkLight()); // Start the blinking coroutine


    }

    private void StopBlinking()
    {
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
            railwayLight1.enabled = false;
            railwayLight2.enabled = false;
        }
    }

    private IEnumerator BlinkLight()
    {
        float timeElapsed = 0f;
        CheckDistanceToPlayer(); // Play the audio

        while (timeElapsed < blinkDuration)
        {
            railwayLight1.enabled = isLightOn;
            railwayLight2.enabled = isLightOn;
            isLightOn = !isLightOn; // Toggle light state
            yield return new WaitForSeconds(blinkingTime);
            timeElapsed += blinkingTime;
        }

        StopBlinking(); 
    }
}
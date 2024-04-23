using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailwayLightingSystem : MonoBehaviour
{
    MovingObjectSpawner movingObjectSpawner;
    [SerializeField] private GameObject RailwayLight;
    /// <summary>
    /// Total duration of blinking (1 second visible, 1 second hidden)
    /// </summary>
    [SerializeField] private float blinkDuration = 2f;
    [SerializeField] float blinkingTime = 0.4f;

    private bool isLightOn = true;
    private Coroutine blinkCoroutine;

    void Start()
    {
        try
        {
            movingObjectSpawner = GetComponent<MovingObjectSpawner>();
            movingObjectSpawner.ObjectIncoming += StartBlinking; // Subscribe to start blinking
        }
        catch (System.Exception)
        {
            throw;
        }
    }

    void OnDisable()
    {
        movingObjectSpawner.ObjectIncoming -= StartBlinking; // Unsubscribe from start blinking
        StopBlinking(); // Stop blinking when disabled
    }

    private void StartBlinking()
    {
        RailwayLight.SetActive(true); // Ensure light is visible when new object comes in
        blinkCoroutine = StartCoroutine(BlinkLight());
    }

    private void StopBlinking()
    {
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
            RailwayLight.SetActive(false); // Ensure light is hidden when blinking stops
        }
    }

    private IEnumerator BlinkLight()
    {
        float timeElapsed = 0f;

        while (timeElapsed < blinkDuration)
        {
            RailwayLight.SetActive(isLightOn);
            isLightOn = !isLightOn;
            yield return new WaitForSeconds(blinkingTime);
            timeElapsed += blinkingTime;
        }

        // Stop blinking after the duration
        StopBlinking();
    }
}

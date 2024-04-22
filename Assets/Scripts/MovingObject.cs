using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    [SerializeField] public bool isJumpable;
    [SerializeField] private float speed;
    private int difficulty;


    private void Update()
    {
        difficulty = PlayerPrefs.GetInt("Difficulty", 1);
        float speedMultiplier = GetSpeedMultiplier(difficulty);
        float currentSpeed = speed * speedMultiplier;
        transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);
    }

    private float GetSpeedMultiplier(int difficultyLevel)
    {
        switch (difficultyLevel)
        {
            case 0:
                return 0.5f;
            case 1:
                return 1f;
            case 2:
                return 20f; 
            default:
                return 1f; 
        }
    }
    // TO DO : Variation of speed
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// CHange la vitesse des objets selon la difficulté
/// et lui donne une trajectoire et une vitesse
/// </summary> 

public class MovingObject : MonoBehaviour
{
    [SerializeField] public bool isJumpable;
    [SerializeField] public float speed;
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
                return 0.75f;
            case 1:
                return 1f;
            case 2:
                return 2f; 
            default:
                return 1f; 
        }
    }
    // TO DO : Variation of speed
}

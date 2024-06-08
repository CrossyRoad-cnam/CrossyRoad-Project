using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Classe score 
/// </summary>
[Serializable]
public class Score
{
    public string name;
    public float score;
    public int difficulty;

    public Score(string name, float score, int difficulty)
    {
        this.name = name;
        this.score = score;
        this.difficulty = difficulty;
    }
}

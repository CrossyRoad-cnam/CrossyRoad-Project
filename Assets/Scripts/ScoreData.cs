using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;


/// <summary>
/// Classe de données de score
/// </summary>
[Serializable]
public class ScoreData 
{
    public List<Score> scores;

    public ScoreData()
    {
        scores = new List<Score>();
    }
}

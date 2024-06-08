using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Trigger de l'eagle
/// </summary>
public class GameManager : MonoBehaviour
{
    [SerializeField] private Enemy enemyController;

    public bool isGameOver = false;

    private void Update()
    {
        CheckAndTriggerEagle();
    }

    private void CheckAndTriggerEagle()
    {
        if (Player.Instance != null && Player.Instance.CheckEnnemyTrigger())
        {
            enemyController.ActivateEnemy(Player.Instance.transform.position);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Enemy enemyController;

    private void Update()
    {
        CheckAndTriggerEagle();
    }

    public void CheckAndTriggerEagle()
    {
        if (Player.Instance != null && Player.Instance.CheckEnnemyTrigger())
        {
            enemyController.ActivateEnemy(Player.Instance.transform.position);
        }
    }
}

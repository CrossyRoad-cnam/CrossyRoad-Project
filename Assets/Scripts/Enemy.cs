using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private bool isActive = false;
    [SerializeField] private float speed = 40.0f;
    private Vector3 lastPlayerPosition;

    private void Start()
    {
        DeactivateEnemy();
    }

    private void Update()
    {
        if (isActive)
        {
            Vector3 targetPosition;
            if (Player.Instance != null)
            {
                lastPlayerPosition = Player.Instance.transform.position + new Vector3(-10, -2, 0);
                targetPosition = lastPlayerPosition;
            }
            else
            {
                targetPosition = lastPlayerPosition;
            }

            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.unscaledDeltaTime);
        }
    }

    public void ActivateEnemy(Vector3 playerPosition)
    {
        if (!isActive)
        {
            transform.position = playerPosition + new Vector3(10, 2, 0);
            gameObject.SetActive(true);
            isActive = true;
            lastPlayerPosition = transform.position;
        }
    }

    public void DeactivateEnemy()
    {
        gameObject.SetActive(false);
        isActive = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private bool isActive = false;
    [SerializeField] private float speed = 40.0f;
    private void Start()
    {
        DeactivateEnemy();
    }
    private void Update()
    {
        if (isActive && Player.Instance != null)
        {
            Vector3 targetPosition = Player.Instance.transform.position;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        }
    }
    public void ActivateEnemy(Vector3 playerPosition)
    {
        if(!isActive)
        {
            transform.position = playerPosition + new Vector3(10, 2, 0);
            gameObject.SetActive(true);
            isActive = true;
        }
    }
    public void DeactivateEnemy()
    {
        gameObject.SetActive(false);
        isActive = false;
    }
}

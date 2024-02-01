using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private TerrainGenerator terrainGenerator; 
    private Animator animator;
    private bool isHopping;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && !isHopping)
        {
            float zDifference = 0;
            if (transform.position.z % 1 != 0)
            {
                zDifference = Mathf.Round(transform.position.z) - transform.position.z;
            }
            MoveCharacter(new Vector3(1, 0, zDifference));
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) && !isHopping)
        {
            MoveCharacter(new Vector3(-1, 0, 0));
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && !isHopping)
        {
            MoveCharacter(new Vector3(0, 0, -1));
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && !isHopping)
        {
            MoveCharacter(new Vector3(0, 0, 1));
        }   
    }

    private void MoveCharacter(Vector3 direction)
    {
        animator.SetTrigger("hop");
        isHopping = true;
        transform.position = (transform.position + direction);
        terrainGenerator.SpawnTerrain(false);
    }
    public void FinishHop()
    {
        isHopping = false;
    }
}

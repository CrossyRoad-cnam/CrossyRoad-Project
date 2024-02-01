using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
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
            animator.SetTrigger("hop");
            isHopping = true;
            float zDifference = 0;
            if (transform.position.z % 1 != 0)
            {
                zDifference = Mathf.Round(transform.position.z) - transform.position.z;
            }
            transform.position = (transform.position + new Vector3(1, 0, zDifference));
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) && !isHopping)
        {
            animator.SetTrigger("hop");
            isHopping = true;
            float zDifference = 0;
            if (transform.position.z % 1 != 0)
            {
                zDifference = Mathf.Round(transform.position.z) - transform.position.z;
            }
            transform.position = (transform.position + new Vector3(-1, 0, zDifference));
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && !isHopping)
        {
            animator.SetTrigger("hop");
            isHopping = true;
            float xDifference = 0;
            if (transform.position.x % 1 != 0)
            {
                xDifference = Mathf.Round(transform.position.x) - transform.position.x;
            }
            transform.position = (transform.position + new Vector3(xDifference, 0, 1));
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && !isHopping)
        {
            animator.SetTrigger("hop");
            isHopping = true;
            float xDifference = 0;
            if (transform.position.x % 1 != 0)
            {
                xDifference = Mathf.Round(transform.position.x) - transform.position.x;
            }
            transform.position = (transform.position + new Vector3(xDifference, 0, -1));
        }   
    }

    public void FinishHop()
    {
        isHopping = false;
    }
}

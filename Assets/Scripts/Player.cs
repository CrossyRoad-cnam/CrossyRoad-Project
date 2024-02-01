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
    }

    public void FinishHop()
    {
        isHopping = false;
    }
}

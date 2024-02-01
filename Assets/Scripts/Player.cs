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
            Debug.Log(transform.position);
            if(transform.position.z % 1 == 0)
            {
                Debug.Log("On Grid Space");
                transform.Translate(new Vector3(1, 0, 0));
            }
        }

    }

    public void FinishHop()
    {
        isHopping = false;
    }
}

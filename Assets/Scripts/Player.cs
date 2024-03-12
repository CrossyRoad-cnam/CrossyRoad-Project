using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private TerrainGenerator terrainGenerator;
    [SerializeField] private Text scoreText;
    
    private int score;
    private Animator animator;
    private bool isHopping;
    private Rigidbody rb;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.UpArrow) && !isHopping)
        {
            MoveCharacter(new Vector3(1, 0, 0));
            score++;
            scoreText.text = "Score: " + score;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) && !isHopping)
        {
            MoveCharacter(new Vector3(-1, 0, 0));
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && !isHopping)
        {
            MoveCharacter(new Vector3(0, 0, 1));
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && !isHopping)
        {
            MoveCharacter(new Vector3(0, 0, -1));
        }
    }

    private void MoveCharacter(Vector3 direction)
    {
        animator.SetTrigger("hop");
        isHopping = true;
        transform.position += direction;
        terrainGenerator.SpawnTerrain(false, transform.position);
    }
    public void FinishHop()
    {
        isHopping = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log(collision.gameObject.name);
        }
    }
}

using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{

    public static Player Instance {  get; private set; }
    private Rigidbody2D rb;
    PlayerStats player;

    public Vector2 move_dir;
    


    private void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() {
        HandleMovement();
    }

    private void HandleMovement()
    {
        Vector2 movement_vector = new Vector2(0, 0);



        if (Input.GetKey(KeyCode.W))
        {
            movement_vector.y = 1f;

        }

        if (Input.GetKey(KeyCode.S))
        {
            movement_vector.y = -1f;


        }

        if (Input.GetKey(KeyCode.D))
        {
            movement_vector.x = 1f;

        }

        if (Input.GetKey(KeyCode.A))
        {
            movement_vector.x = -1f;

        }
        movement_vector = movement_vector.normalized;
        rb.MovePosition(rb.position + player.currentMoveSpeed * (movement_vector * Time.fixedDeltaTime));


        move_dir = movement_vector;
        
    }

    private void Start()
    {
        player = GetComponent<PlayerStats>();
    }


    public bool IsRunningLeft() { return move_dir.x < 0f; }
    public bool IsRunningRight() { return move_dir.x > 0f; }
}

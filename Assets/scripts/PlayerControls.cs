using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed of movement

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Horizontal movement
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector2 movement = new Vector2(horizontalInput, verticalInput).normalized * moveSpeed;

        // If no input is detected, set the velocity to zero
        if (movement.magnitude == 0)
        {
            rb.velocity = Vector2.zero;
        }
        else
        {
            // Normalize the movement vector and apply move speed
            movement = movement.normalized * moveSpeed;
            rb.velocity = movement;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public bool isDamaged;
    public float moveSpeed = 5f; // Speed of movement

    private Rigidbody2D rb;
    private Animator animator;
    private AutoCast autoCast;

    float horizontalInput;
    float verticalInput;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        autoCast = GetComponent<AutoCast>();
    }

    // Update is called once per frame
    void Update()
    {
        // Horizontal movement
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
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

        ManageAnimations();
    }

    private void ManageAnimations()
    {
        if (isDamaged)
        {
            animator.SetBool("isDamaged", true);
            isDamaged = false;
            return;
        }
        else
        {
            animator.SetBool("isDamaged", false);
        }

        if (autoCast.isCasting)
        {
            animator.SetBool("isAttacking", true);
            autoCast.isCasting = false;
            return;
        }
        else
        {
            animator.SetBool("isAttacking", false);
        }

        if (horizontalInput > 0 || verticalInput > 0)
        {
            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
    }
}

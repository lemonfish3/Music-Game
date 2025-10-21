using NUnit.Framework.Interfaces;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class Player : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    [Header("References")]
    private Rigidbody2D rb;
    private Animator animator;

    private Vector2 movement;
    private Vector2 facingDirection = Vector2.down; // Default facing down

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        HandleInput();
        //UpdateAnimation();
    }

    void FixedUpdate()
    {
        Move();
    }

    void HandleInput()
    {
        // Get raw input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Normalize diagonal movement
        movement = movement.normalized;

        // Update facing direction when moving
        if (movement != Vector2.zero)
            facingDirection = movement;
    }

    void Move()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    //void UpdateAnimation()
    //{
    //    animator.SetFloat("MoveX", movement.x);
    //    animator.SetFloat("MoveY", movement.y);
    //    animator.SetFloat("FacingX", facingDirection.x);
    //    animator.SetFloat("FacingY", facingDirection.y);
    //    animator.SetBool("IsMoving", movement.magnitude > 0);
    //}
}

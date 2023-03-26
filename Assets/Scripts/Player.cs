using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class Player : MonoBehaviour
{
    public Animator animator;
    public float speed;

    private CharacterController2D charCon;
    private float moveDir;
    private bool jump;
    public int maxJumps;
    public int jumpsRemaining;

    public float airSpeed;
    public float origSpeed;

    private Vector2 spawnPoint;
    public bool alive = true;

    private void Start()
    {
        charCon = GetComponent<CharacterController2D>();
        jump = false;
        maxJumps = 1;
        origSpeed = speed;
        airSpeed = speed * 2 / 3;
    }

    private void Update()
    {   
        if (charCon.IsPlayerOnGround())// && charCon.m_Rigidbody2D.velocity.y <=0) 
        {
            jumpsRemaining = maxJumps;
            speed = origSpeed;
        }
    }

    public void Left(InputAction.CallbackContext context)
    {
        moveDir = -context.ReadValue<float>();
    }

    public void Right(InputAction.CallbackContext context)
    {
        moveDir = context.ReadValue<float>();
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (jumpsRemaining > 0 && context.started)
        {
            jumpsRemaining--;
            jump = true;
            animator.SetTrigger("Jump");
            charCon.m_Grounded = false;
            speed = airSpeed;
        }
    }

    private void FixedUpdate()
    {
        if (alive)
        {
            if (jump)
            {
                charCon.Move(moveDir * speed * Time.fixedDeltaTime, false, jump);
            }
            else
            {
                charCon.Move(moveDir * speed * Time.fixedDeltaTime, false, jump);
            }

            if (charCon.IsPlayerOnGround())
            {
                animator.SetTrigger("Grounded");
            }
            jump = false;
            animator.SetFloat("Idle Run", Mathf.Abs(moveDir));
        }
    }

    public void respawn()
    {
        transform.position = spawnPoint;
        animator.SetTrigger("Respawn");
    }

    public void checkpoint(Vector2 pos)
    {
        spawnPoint = pos;
        print("Checkpoint set at: " + pos);
    }
}

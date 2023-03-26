using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class Player : MonoBehaviour
{
    public Animator animator;

    public LevelReset levelReset;

    private CharacterController2D charCon;
    
    private bool jump;
    private bool crouch; 
    public int maxJumps;
    public int jumpsRemaining;

    private float moveDir;
    public float speed;
    public float airSpeed;
    public float origSpeed;
    public float crouchSpeed;

    private Vector2 spawnPoint;
    public bool alive = true;

    private void Start()
    {
        charCon = GetComponent<CharacterController2D>();
        jump = false;
        maxJumps = 1;
        origSpeed = speed;
        airSpeed = speed * 4 / 5;
        crouchSpeed = speed /5;
        spawnPoint = transform.position;
    }

    private void Update()
    {   
        
        if (charCon.IsPlayerOnGround())// && charCon.m_Rigidbody2D.velocity.y <=0) COLIN: we could re-add this and make it to where you must stand still to jump? Call it a feature
        {
            jumpsRemaining = maxJumps;
            speed = origSpeed;
        }
        
        if (crouch)
        {
            speed = crouchSpeed;
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

    public void Crouch(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            crouch = true;
            speed = crouchSpeed;
            print("Speed = " + speed);
 
        }
        else
        {
            crouch = false;
            speed = origSpeed;
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

                if (crouch)
                {
                    charCon.Move(moveDir * speed * Time.fixedDeltaTime, true, false);
                }
                else
                {
                    charCon.Move(moveDir * speed * Time.fixedDeltaTime, false, false);
                }
            }
            jump = false;
            animator.SetFloat("Idle Run", Mathf.Abs(moveDir));
        }
    }

    public void death()
    {
        print("died");
        alive = false;
        animator.SetTrigger("Death");
        StartCoroutine(WaitThenRespawn());
    }

    private IEnumerator WaitThenRespawn()
    {
        print("waiting");
        yield return new WaitForSecondsRealtime(5);
        print("respawn");
        respawn();
    }

    public void respawn()
    {
        transform.position = spawnPoint;
        animator.SetTrigger("Respawn");
        alive = true;
        levelReset.resetLevel();
    }

    public void checkpoint(Vector2 pos)
    {
        spawnPoint = pos;
        print("Checkpoint set at: " + pos);
    }
}

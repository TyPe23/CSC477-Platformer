using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class Player : MonoBehaviour
{
    #region Fields and Properties
    private float moveDir;
    private CharacterController2D charCon;
    private Vector2 spawnPoint;

    public Animator animator;

    public Enemy[] enemyList;
    public movingObj[] objList;
    public Lever[] leverList;
    public bool jump;
    public bool crouch; 
    public int jumpsRemaining;
    public int maxJumps;
    public float speed;
    public float airSpeed;
    public float origSpeed;
    public float crouchSpeed;
    public bool alive = true;
    #endregion

    #region Life Cycle
    private void Start()
    {
        charCon = GetComponent<CharacterController2D>();
        jump = false;
        maxJumps = 1;
        origSpeed = speed;
        airSpeed = speed * 6 / 6;
        crouchSpeed = speed / 5;
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
    #endregion

    #region Movement
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
    #endregion

    #region Helper Funcs
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
        yield return new WaitForSecondsRealtime(2);
        print("respawn");
        respawn();
    }

    public void respawn()
    {
        transform.position = spawnPoint;
        animator.SetTrigger("Respawn");
        alive = true;
        resetLevel();
    }

    public void checkpoint(Vector2 pos)
    {
        spawnPoint = pos;
        print("Checkpoint set at: " + pos);
    }

    public void resetLevel()
    {
        foreach (Enemy enemy in enemyList)
        {
            enemy.respawn();
        }
        foreach (movingObj obj in objList)
        {
            obj.resetPos();
        }
        foreach (Lever lever in leverList)
        {
            lever.flipped = false;
        }
    }
    #endregion
}

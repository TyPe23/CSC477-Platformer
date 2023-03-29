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
    private Vector3 origPoint;

    public Animator animator;
    public Vector2 aim;
    public Enemy[] enemyList;
    public movingObj[] objList;
    public Lever[] leverList;
    public bool jump;
    public bool crouch; 
    public int jumpsRemaining;
    public int maxJumps;
    public float speed;
    public bool alive = true;
    public bool stickyHand = false;
    #endregion

    #region Life Cycle
    private void Start()
    {
        charCon = GetComponent<CharacterController2D>();
        jump = false;
        spawnPoint = transform.position;
        origPoint = transform.position;
    }

    private void Update()
    {   
        
        if (charCon.IsPlayerOnGround())// && charCon.m_Rigidbody2D.velocity.y <=0) COLIN: we could re-add this and make it to where you must stand still to jump? Call it a feature
        {
            jumpsRemaining = maxJumps;
        }
    }
    private void FixedUpdate()
    {
        if (alive)
        {
            if (charCon.IsPlayerOnGround())
            {
                animator.SetTrigger("Grounded");
            }

            charCon.Move(moveDir * speed * Time.fixedDeltaTime, crouch, jump);
            jump = false;
            animator.SetFloat("Idle Run", Mathf.Abs(moveDir));
        }
    }
    #endregion

    #region Input
    public void Left(InputAction.CallbackContext context)
    {
        moveDir = -context.ReadValue<float>();
        aim = Vector2.left;
    }

    public void Right(InputAction.CallbackContext context)
    {
        moveDir = context.ReadValue<float>();
        aim = Vector2.right;
    }

    public void Up(InputAction.CallbackContext context)
    {
        aim = Vector2.up;
    }

    public void Down(InputAction.CallbackContext context)
    {
        aim = Vector2.down;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (jumpsRemaining > 0 && context.started)
        {
            jumpsRemaining--;
            jump = true;
            animator.SetTrigger("Jump");
            charCon.m_Grounded = false;
        }
    }

    public void Crouch(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            crouch = true;
        }
        else
        {
            crouch = false;
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

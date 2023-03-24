using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        // Player Movement 
        moveDir = Input.GetAxis("Horizontal");

        // Player Jumping 
        if (Input.GetKeyDown(KeyCode.Space) && jumpsRemaining > 0)
        {
            jumpsRemaining--;
            jump = true;
            animator.SetTrigger("Jump");
            charCon.m_Grounded = false;
            speed = airSpeed;
        }
        // print("before check: " + charCon.IsPlayerOnGround());
        // This checks if the player has finished jumping. resets jumps and changes speed 
        if (charCon.IsPlayerOnGround() && charCon.m_Rigidbody2D.velocity.y <=0) 
        {
            jumpsRemaining = maxJumps;
            speed = origSpeed;
        }

        // Player kys 
        if (Input.GetKeyDown(KeyCode.K))
        {
            animator.SetTrigger("Death");
        }
        
    }

    private void FixedUpdate()
    {
        //print(jump);


        if (jump)
        {
            //print("before jump: " + charCon.IsPlayerOnGround());
            charCon.Move(moveDir * speed * Time.fixedDeltaTime, false, jump);
            //print("after jump: " + charCon.IsPlayerOnGround());
        }
        else
        {
            charCon.Move(moveDir * speed * Time.fixedDeltaTime, false, jump);
        }

        //print("before check: " + charCon.IsPlayerOnGround());
        if (charCon.IsPlayerOnGround())
        {
            animator.SetTrigger("Grounded");
        }
        jump = false;
        animator.SetFloat("Idle Run", Mathf.Abs(moveDir));
    }
}

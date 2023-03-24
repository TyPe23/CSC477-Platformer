using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Player1 : MonoBehaviour {
    public Animator animator;
    public float speed;

    private CharacterController2D charCon;
    private float moveDir;
    private bool jump;
    public int maxJumps;
    public int jumpsRemaining;

    public float airSpeed;
    public float origSpeed;

    private void Start() {
        charCon = GetComponent<CharacterController2D>();
        jump = false;
        maxJumps = 1;
        origSpeed = speed;
        airSpeed = speed * 2 / 3;
    }

    private void Update()
    {
        moveDir = Input.GetAxis("Horizontal");
        if (Input.GetKeyDown(KeyCode.Space) && jumpsRemaining > 0)
        {
            jumpsRemaining--;
            jump = true;
            animator.SetTrigger("Jump");
            charCon.m_Grounded = false;
            speed = airSpeed;
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            animator.SetTrigger("Death");
        }
        print("before check: " + charCon.IsPlayerOnGround());
        if (charCon.IsPlayerOnGround() && jump == false)
        {
            jumpsRemaining = maxJumps;
            speed = origSpeed;
        }
    }

    private void FixedUpdate() {
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

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using state = enemyStates;

public enum enemyStates
{
    RESET,
    PATROL_RIGHT,
    PATROL_LEFT,
    CHASE,
    DEAD,
    IDLE
}

public class Enemy : MonoBehaviour
{
    #region FieldsAndProperties
    private Dictionary<enemyStates, Action> statesStayMeths;
    private Dictionary<enemyStates, Action> statesEnterMeths;
    private Dictionary<enemyStates, Action> statesExitMeths;
    private CharacterController2D charCon;
    public state state { get; private set; }
    public float speed;
    private Animator animator;
    public float xTarget;
    private Vector2 origPos;

    //Set in inspector
    public Transform tfPlayer;
    public float distFromPlayer;
    public float speedInc;
    private bool jump = false;
    public float patrolDist;
    private float resetTarget;
    public Player player;
    #endregion

    #region LifeCycle
    // Start is called before the first frame update
    void Start()
    {
        statesStayMeths = new Dictionary<enemyStates, Action>()
        {
            {state.RESET, StateStayReset},
            {state.PATROL_LEFT, StateStayPatLeft},
            {state.PATROL_RIGHT, StateStayPatRight},
            {state.CHASE, StateStayChase},
            {state.DEAD, StateStayDead},
            {state.IDLE, StateStayIdle},
        };

        statesEnterMeths = new Dictionary<enemyStates, Action>()
        {
            {state.RESET, StateEnterReset},
            {state.PATROL_LEFT, StateEnterPatLeft},
            {state.PATROL_RIGHT, StateEnterPatRight},
            {state.CHASE, StateEnterChase},
            {state.DEAD, StateEnterDead},
            {state.IDLE, StateEnterIdle},
        };

        statesExitMeths = new Dictionary<enemyStates, Action>()
        {
            {state.RESET, StateExitReset},
            {state.PATROL_LEFT, StateExitPatLeft},
            {state.PATROL_RIGHT, StateExitPatRight},
            {state.CHASE, StateExitChase},
            {state.DEAD, StateExitDead},
            {state.IDLE, StateExitIdle},
        };

        state = state.PATROL_RIGHT;
        StateEnterPatRight();
        charCon = GetComponent<CharacterController2D>();
        animator = GetComponent<Animator>();
        origPos = transform.position;
    }

    

    // Update is called once per frame
    void FixedUpdate()
    {
        statesStayMeths[state].Invoke();
    }

    private void OnTriggerEnter2D(Collider2D c)
    {
        if (c.CompareTag("Player"))
        {
            if ( state == state.PATROL_LEFT || state == state.PATROL_RIGHT || state == state.RESET)
            {
                ChangeState(state.CHASE);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D c)
    {
        if (c.CompareTag("Player"))
        {
            if (state == state.CHASE)
            {
                ChangeState(state.RESET);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D c)
    {
        if (c.gameObject.CompareTag("Player"))
        {
            player.death();
            ChangeState(state.IDLE);
        }
        else if (c.gameObject.CompareTag("attack"))
        {
            ChangeState(state.DEAD);
        }
    }

    public void ChangeState(state newState)
    {
        if (state != newState)
        {
            statesExitMeths[state].Invoke();
            state = newState;
            statesEnterMeths[state].Invoke();
        }

    }
    #endregion

    #region Exit
    private void StateExitChase()
    {
        speed -= speedInc;
    }

    private void StateExitPatRight()
    {
        
    }

    private void StateExitPatLeft()
    {
        
    }

    private void StateExitReset()
    {
        
    }

    private void StateExitDead()
    {

    }

    private void StateExitIdle()
    {

    }
    #endregion

    #region Enter
    private void StateEnterChase()
    {
        speed += speedInc;
        StartCoroutine(WaitThenJump());
    }

    private void StateEnterPatRight()
    {
        xTarget = transform.position.x + patrolDist;
    }

    private void StateEnterPatLeft()
    {
        xTarget = transform.position.x - patrolDist;
    }

    private void StateEnterReset()
    {
        
    }

    private void StateEnterDead()
    {
        animator.SetTrigger("Death");
    }

    private void StateEnterIdle()
    {

        animator.SetFloat("Idle Run", 0f);
    }
    #endregion

    #region Stay
    private void StateStayChase()
    {
        float xPlayerPos = tfPlayer.position.x;
        float xPos = transform.position.x;
        float dir = (xPlayerPos - xPos) < 0 ? -1 : 1;

        jump = false;
        charCon.Move(dir * speed * Time.fixedDeltaTime, false, jump);
        animator.SetFloat("Idle Run", 1f);
    }

    private void StateStayPatRight()
    {
        if (transform.position.x >= xTarget)
        {
            ChangeState(state.PATROL_LEFT);
        }
        else
        {
            jump = false;
            charCon.Move(speed * Time.fixedDeltaTime, false, jump);
            animator.SetFloat("Idle Run", 1f);
        }
    }

    private void StateStayPatLeft()
    {
        if (transform.position.x <= xTarget)
        {
            ChangeState(state.PATROL_RIGHT);
        }
        else
        {
            jump = false;
            charCon.Move(-speed * Time.fixedDeltaTime, false, jump);
            animator.SetFloat("Idle Run", 1f);
        }
    }

    private void StateStayReset()
    {
        if (Mathf.Abs(transform.position.x - origPos.x) < 0.1f)
        {
            ChangeState(state.PATROL_RIGHT);
        }
        else
        {
            float dir = (transform.position.x - origPos.x) < 0 ? 1 : -1;
            jump = false;
            charCon.Move((dir * speed) * Time.fixedDeltaTime, false, jump);
            animator.SetFloat("Idle Run", 1f);
        }
    }

    private void StateStayDead()
    {

    }

    private void StateStayIdle()
    {

    }
    #endregion

    #region Helper Meths
    private IEnumerator WaitThenJump()
    {
        yield return new WaitForSeconds(2);
        jump = true;
    }

    public void respawn()
    {
        transform.position = origPos;
        state = state.PATROL_RIGHT;
        StateEnterPatRight();
    }
    #endregion
}

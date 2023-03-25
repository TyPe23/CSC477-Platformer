using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using state = enemyStates;

public enum enemyStates
{
    IDLE,
    RESET,
    PATROL_RIGHT,
    PATROL_LEFT,
    CHASE
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
    private float xTarget;
    private Vector2 origPos;

    //Set in inspector
    public Transform tfPlayer;
    public float distFromPlayer;
    public float speedInc;
    private bool jump = false;
    public float patrolDist;
    #endregion

    #region LifeCycle
    // Start is called before the first frame update
    void Start()
    {
        statesStayMeths = new Dictionary<enemyStates, Action>()
        {
            {state.IDLE, StateStayIdle},
            {state.RESET, StateStayReset},
            {state.PATROL_LEFT, StateStayPatLeft},
            {state.PATROL_RIGHT, StateStayPatRight},
            {state.CHASE, StateStayChase},
        };

        statesEnterMeths = new Dictionary<enemyStates, Action>()
        {
            {state.IDLE, StateEnterIdle},
            {state.RESET, StateEnterReset},
            {state.PATROL_LEFT, StateEnterPatLeft},
            {state.PATROL_RIGHT, StateEnterPatRight},
            {state.CHASE, StateEnterChase},
        };

        statesExitMeths = new Dictionary<enemyStates, Action>()
        {
            {state.IDLE, StateExitIdle},
            {state.RESET, StateExitReset},
            {state.PATROL_LEFT, StateExitPatLeft},
            {state.PATROL_RIGHT, StateExitPatRight},
            {state.CHASE, StateExitChase},
        };

        state = state.IDLE;
        charCon = GetComponent<CharacterController2D>();
        animator = GetComponent<Animator>();
        origPos = transform.position;
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        print(state);
        statesStayMeths[state].Invoke();
    }

    private void OnTriggerEnter2D(Collider2D c)
    {
        if (c.CompareTag("Player"))
        {
            if ( state == state.PATROL_LEFT || state == state.PATROL_RIGHT)
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
                ChangeState(state.PATROL_RIGHT);
            }
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

    private void StateEnterIdle()
    {
        throw new NotImplementedException();
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
        if (Mathf.Abs(transform.position.x - tfPlayer.position.x) > distFromPlayer)
        {
            ChangeState(state.PATROL_RIGHT);
        }
        jump = false;
        charCon.Move(speed * Time.fixedDeltaTime, false, jump);
        animator.SetFloat("Idle Run", 1f);
    }

    private void StateStayIdle()
    {
        if (charCon.IsPlayerOnGround())
        {
            ChangeState(state.RESET);
        }
    }
    #endregion

    #region Helper Meths
    private IEnumerator WaitThenJump()
    {
        yield return new WaitForSeconds(2);
        jump = true;
    }
    #endregion
}

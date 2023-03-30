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
    public state state;
    public float speed;
    public float origSpeed;
    public float xTarget;
    private Vector2 origPos;

    //Set in inspector
    public CharacterController2D charCon;
    public Animator animator;
    public Transform tfPlayer;
    public float distFromPlayer;
    public float speedInc;
    private bool jump = false;
    public float patrolDist;
    private float resetTarget;
    public Player player;
    public GameObject vision;
    public GameObject alert;
    public bool idle;
    public AudioSource alerted;
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

        if (!idle)
        {
            state = state.PATROL_RIGHT;
            StateEnterPatRight();
        }
        else
        {
            state = state.IDLE;
            StateEnterIdle();
            charCon.Move(-0.1f, false, false);
        }
        origSpeed = speed;
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
            if ( state == state.PATROL_LEFT || state == state.PATROL_RIGHT || state == state.RESET || (state == state.IDLE && player.alive))
            {
                alerted.Play();
                ChangeState(state.CHASE);
            }
        }
        else if (c.gameObject.CompareTag("attack"))
        {
            ChangeState(state.DEAD);
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
        Rigidbody2D RB = GetComponent<Rigidbody2D>();
        RB.freezeRotation = true;
        RB.gravityScale = 3;
        GetComponent<CapsuleCollider2D>().enabled = true;
        vision.SetActive(true);
        animator.SetTrigger("Respawn");
    }

    private void StateExitIdle()
    {

    }
    #endregion

    #region Enter
    private void StateEnterChase()
    {
        speed += speedInc;
        StartCoroutine(Alert());
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
        Rigidbody2D RB = GetComponent<Rigidbody2D>();
        RB.freezeRotation = false;
        RB.gravityScale = 0;
        GetComponent<CapsuleCollider2D>().enabled = false;
        vision.SetActive(false);
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
            if (!idle)
            {
                ChangeState(state.PATROL_RIGHT);
            }
            else
            {
                ChangeState(state.IDLE);
                charCon.Move(-0.1f, false, false);
            }
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
    private IEnumerator Alert()
    {
        alert.SetActive(true);
        yield return new WaitForSeconds(2);
        alert.SetActive(false);
    }

    public void respawn()
    {
        transform.position = origPos;
        speed = origSpeed;
        if (!idle)
        {
            state = state.PATROL_RIGHT;
            StateEnterPatRight();
        }
        else
        {
            state = state.IDLE;
            StateEnterIdle();
            charCon.Move(-0.1f, false, false);
        }
    }
    #endregion
}

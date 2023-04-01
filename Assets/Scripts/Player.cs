using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class Player : MonoBehaviour
{
    #region Fields and Properties
    private float moveDir;

    [SerializeField] Image stickyHandUI;
    [SerializeField] Image moonBootsUI;
    [SerializeField] TextMeshProUGUI stickyHandControl;
    [SerializeField] TextMeshProUGUI moonBootsControl;

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
    private bool up = false;
    public AudioSource boing;
    public AudioSource walking;
    public AudioSource dead;
    #endregion

    #region Life Cycle
    private void Start()
    {
        charCon = GetComponent<CharacterController2D>();
        jump = false;
        spawnPoint = transform.position;
        origPoint = transform.position;
        stickyHandUI.enabled = false;
        stickyHandControl.enabled = false;
        moonBootsUI.enabled = false;
        moonBootsControl.enabled = false;
        walking.Play();
    }

    private void Update()
    {   
        if (stickyHand)
        {
            stickyHandUI.enabled = true;
            stickyHandControl.enabled = true;
        }

        if (maxJumps == 2)
        {
            moonBootsUI.enabled = true;
            animator.SetTrigger("Moon Boots Get");
            moonBootsControl.enabled = true;
        }
        
        if (charCon.IsPlayerOnGround())
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
                walking.volume = (Mathf.Abs(moveDir)) / 15;
            }
            else
            {
                walking.volume = 0;
            }

            charCon.Move(moveDir * speed * Time.fixedDeltaTime, crouch, jump);
            jump = false;
            animator.SetFloat("Idle Run", Mathf.Abs(moveDir));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Kill"))
        {
            death();
        }
    }
    #endregion

    #region Input
    public void LeftRight(InputAction.CallbackContext context)
    {
        moveDir = context.ReadValue<float>();
        Vector3 aimVec = Vector3.Normalize(new Vector3(moveDir, 0, 0));

        if (aimVec.x != 0 && !up)
        {
            aim = new Vector2(aimVec.x, aimVec.y);
        }
    }

    public void Up(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            up = true;
        }
        aim = Vector2.up;
        if (context.canceled)
        {
            up = false;
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (jumpsRemaining > 0 && context.started)
        {
            jumpsRemaining--;
            jump = true;
            animator.SetTrigger("Jump");
            boing.Play();
            charCon.m_Grounded = false;
        }
    }

    public void Crouch(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            crouch = true;
            animator.SetBool("Crouch", true);
        }
        else
        {
            crouch = false;
            animator.SetBool("Crouch", false);
        }
    }
    #endregion

    #region Helper Funcs
    public void death()
    {
        print("died");
        alive = false;
        animator.SetTrigger("Death");
        dead.Play();
        StartCoroutine(WaitThenRespawn());
    }

    private IEnumerator WaitThenRespawn()
    {
        print("waiting");
        yield return new WaitForSecondsRealtime(3);
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

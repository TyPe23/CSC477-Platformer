using UnityEngine;
using UnityEngine.InputSystem;

public class Grapple : MonoBehaviour
{
    private Vector3 startScale;
    public Vector3 targetScale;
    private Player charScript;
    private Rigidbody2D charRB;
    private bool thrown;

    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        startScale = transform.localScale;
        startScale.y = 0;
        charScript = player.GetComponent<Player>();
        charRB = player.GetComponent<Rigidbody2D>();
        thrown = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.localScale.y < targetScale.y && targetScale != startScale) 
        {
            transform.localScale += new Vector3(0, 0.5f * Time.deltaTime, 0);
        }
        else if (transform.localScale.y > startScale.y)
        {
            targetScale = startScale;
            transform.localScale -= new Vector3(0, 0.2f * Time.deltaTime, 0);
        }
        else
        {
            thrown = false;
            transform.localScale = startScale;
            charRB.gravityScale = 3;
        }
    }

    public void Throw(InputAction.CallbackContext context)
    {
        if (charScript.stickyHand && context.performed && thrown == false && !charScript.crouch)
        {
            if (charScript.aim == new Vector2(1, 0))
            {
                transform.localRotation = Quaternion.Euler(0, 0, -90);
            }
            else if (charScript.aim == new Vector2(-1, 0))
            {
                transform.localRotation = Quaternion.Euler(0, 0, -90);
            }
            else if (charScript.aim == new Vector2(0, 1))
            {
                transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                transform.localRotation = Quaternion.Euler(0, 0, 180);
            }

            targetScale = startScale + new Vector3(0, 0.18f, 0);

            thrown = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ground") && transform.localScale.y > 0)
        {
            charRB.gravityScale = 0;
            targetScale = transform.localScale;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("ground"))
        {
            charRB.velocity = new Vector2(charRB.velocity.x, 0);
            charRB.angularVelocity = 0f;
            player.transform.Translate(charScript.aim * Time.deltaTime * 10);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("ground"))
        {
            charRB.gravityScale = 3;
        }
    }
}

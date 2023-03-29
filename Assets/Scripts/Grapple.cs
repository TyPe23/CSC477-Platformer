using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Grapple : MonoBehaviour
{
    private Vector3 startScale;
    public Vector3 targetScale;
    private Vector3 startRot;
    private Player playerScript;

    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        startScale = transform.localScale;
        startScale.y = 0;
        startRot = transform.eulerAngles;
        playerScript = player.GetComponent<Player>();
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
            transform.localScale -= new Vector3(0, 0.5f * Time.deltaTime, 0);
        }
    }

    public void Throw(InputAction.CallbackContext context)
    {
        if (playerScript.stickyHand && context.performed)
        {
            if (playerScript.aim == new Vector2(1, 0))
            {
                transform.localRotation = Quaternion.Euler(0, 0, -90);
            }
            else if (playerScript.aim == new Vector2(-1, 0))
            {
                transform.localRotation = Quaternion.Euler(0, 0, -90);
            }
            else if (playerScript.aim == new Vector2(0, 1))
            {
                transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                transform.localRotation = Quaternion.Euler(0, 0, 180);
            }

            targetScale = startScale + new Vector3(0, 0.18f, 0);
        }
        else
        {
            targetScale = startScale;
        }
    }
}

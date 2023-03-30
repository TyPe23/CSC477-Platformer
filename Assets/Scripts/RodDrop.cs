using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RodDrop : MonoBehaviour
{
    public Lever lever;
    public Rigidbody2D body;
    public GameObject floor;
    public GameObject crater;
    private bool crash = false;

    // Update is called once per frame
    void Update()
    {
        if (lever.flipped && !crash)
        {
            crash = true;
            body.gravityScale = 1;
            floor.SetActive(false);
            crater.SetActive(true);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever_Door : MonoBehaviour
{
    public bool flipped;
    public Door[] activateDoor;

    // Start is called before the first frame update
    void Start()
    {
        flipped = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (flipped)
        {
            foreach (Door door in activateDoor)
            {
                door.togglePower();
            }

            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }
    }

    private void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.CompareTag("attack"))
        {
            foreach (Door door in activateDoor)
            {
                door.togglePower();
            }

            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);


            // play some sound here
        }
    }
}

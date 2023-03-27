using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever_Door : MonoBehaviour
{
    public bool flipped = false;
    public Door[] activateDoor;

    private void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.CompareTag("Player") && !flipped)
        {
            foreach (Door door in activateDoor)
            {
                door.togglePower();
            }

            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);

            flipped = true;
            // play some sound here
        }
    }
}

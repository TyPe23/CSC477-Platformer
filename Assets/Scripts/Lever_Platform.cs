using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever_Platform : MonoBehaviour
{
    public bool flipped = false;
    public PlatformRetract[] activatePlatform;

    private void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.CompareTag("Player") && !flipped)
        {
            foreach (PlatformRetract platform in activatePlatform)
            {
                platform.togglePower();
            }

            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);

            flipped = true;
            // play some sound here
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    public bool flipped = false;
    public movingObj[] objs;
    private AudioSource lever;

    private void Start()
    {
        lever = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.CompareTag("Player") && !flipped)
        {
            foreach (movingObj obj in objs)
            {
                obj.togglePower();
            }

            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);

            flipped = true;
            lever.Play();
            // play some sound here
        }
    }
}

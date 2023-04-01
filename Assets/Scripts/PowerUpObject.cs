using System.Collections;
using UnityEngine;


// Template to destroy a power up object and apply its effect
public class PowerUpObject : MonoBehaviour
{
    private Vector2 origPos;
    public AudioSource fanfare;
    public SpriteRenderer rend;
    private bool grabbed = false;

    public Powerup powerup; // Calls PowerAbstract.cs
    public void OnTriggerEnter2D(Collider2D collision)
    {
        // Checks if the colliding game object tag begins with "Player" (In this case it *IS* "Player")
        if (collision.gameObject.transform.tag.StartsWith("Player") && !grabbed)
        {
            grabbed = true;

            powerup.Apply(collision.gameObject);

            StartCoroutine(PlaySoundThenDestroy());
        }
    }

    private void Start()
    {
        origPos = transform.position;
    }

    private void Update()
    {
        transform.position = new Vector2(transform.position.x, origPos.y + Mathf.Sin(Time.time) * 0.125f);
    }

    private IEnumerator PlaySoundThenDestroy()
    {
        rend.enabled = false;
        fanfare.Play();
        yield return new WaitForSecondsRealtime(3);
        Destroy(gameObject);
    }
}

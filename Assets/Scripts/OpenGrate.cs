using UnityEngine;
using UnityEngine.Tilemaps;

public class OpenGrate : MonoBehaviour
{
    public Player player;
    private Tilemap destructableTiles;
    public AudioSource breakGrate;

    // Start is called before the first frame update
    void Start()
    {
        destructableTiles = GetComponent<Tilemap>();
    }

    private void OnCollisionStay2D(Collision2D c)
    {
        if (c.gameObject.CompareTag("Player") && player.crouch)
        {
            print("break!");
            Vector3 contactPoint = Vector3.zero;
            foreach (ContactPoint2D hit in c.contacts)
            {
                print(hit.normal);
                contactPoint.x = hit.point.x + 0.1f * hit.normal.x;
                contactPoint.y = hit.point.y + 0.1f * hit.normal.y;
                destructableTiles.SetTile(destructableTiles.WorldToCell(contactPoint), null);
                breakGrate.Play();
            }
        }
    }
}

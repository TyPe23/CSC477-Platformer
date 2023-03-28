using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StickyHand")]

public class StickyHand : Powerup
{
    public GameObject clickedObj;


    // public LayerMask grappableLayer;
    // This is commented out for if we want a specific layer for walls/objects that can be grappled.

    public override void Apply(GameObject target)
    {
        Debug.Log("StickyHand applied");
        // 
    }

    void Update()
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        RaycastHit2D hitData = Physics2D.Raycast(new Vector2(worldPos.x, worldPos.y), Vector2.zero, 0); // add grappableLayer at end here if wanting to use the LayerMask.

        if (hitData && Input.GetMouseButtonDown(0)) // Upon click if an object was clicked, assign it
        {
            clickedObj = hitData.transform.gameObject;
        }

        // Once click position is found we need to get the player from point A (current location) to point B (where the click was made).






        // 
        Debug.DrawRay(GameObject.FindGameObjectsWithTag("Player")[0].transform.position, worldPos, Color.green); // Debug ray    }
    }
}


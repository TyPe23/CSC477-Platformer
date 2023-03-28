using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StickyHand")]

public class StickyHand : Powerup
{
    public override void Apply(GameObject target)
    {
        if (target.ToString()[0] == 'P')
        {
            target.GetComponent<Player>().stickyHand = true;

        }
    }
}


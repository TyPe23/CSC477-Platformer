using UnityEngine;

// Allows us to make multiple powerup objects with properties we can manipulate in unity editor; We can have variations of the same power up
[CreateAssetMenu(menuName = "Powerups/MoonBoots")]
 
public class MoonBoots : Powerup // Calls PowerUpObject
{
    // New Jump Limit is initialized in the Unity Editor for the Moon Boots asset 
    public int new_jumpLimit;
    public override void Apply(GameObject target)
    {
        // Checks if target name begins with 'P' (Player) 
        // Be sure the Player is tagged 'Player '
        if (target.ToString()[0] == 'P') {
            target.GetComponent<Player>().maxJumps = new_jumpLimit; // Calls the maxJumps variable in the Player class 
        }
    }
}

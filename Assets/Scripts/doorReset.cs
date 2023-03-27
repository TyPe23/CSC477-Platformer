using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorReset : MonoBehaviour
{
    public Door[] doorList;

    public void resetLevel()
    {
        foreach (Door door in doorList)
        {
            door.resetPos();
        }
    }
}

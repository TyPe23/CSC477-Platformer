using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyReset : MonoBehaviour
{
    public Enemy[] enemyList;

    public void resetLevel()
    {
        foreach (Enemy enemy in enemyList)
        {
            enemy.respawn();
        }
    }
}

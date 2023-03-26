using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelReset : MonoBehaviour
{
    public GameObject[] objList;
    private Vector2[] spawnList;

    // Start is called before the first frame update
    void Start()
    {
        spawnList = new Vector2[objList.Length];
        for (int i = 0; i < objList.Length; i++)
        {
            print(objList[i]);
            spawnList[i] = objList[i].transform.position;
        }
        print(spawnList);
    }

    public void resetLevel()
    {
        for (int i = 0; i < objList.Length; i++)
        {
            objList[i].transform.position = spawnList[i];
        }
    }
}

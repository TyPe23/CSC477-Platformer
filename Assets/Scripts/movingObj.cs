using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movingObj : MonoBehaviour
{
    public float speed = 5;
    public Vector3 origPos;
    public bool poweredOn;
    public bool position1;
    public bool position2;
    public float moveDist = 8f;
    

    // Start is called before the first frame update
    void Start()
    {
        origPos = transform.position;
        position1 = true; 
        position2 = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (poweredOn)
        {
            if (position1)
            {
                transform.Translate(Vector3.left * speed * Time.deltaTime);
                if (transform.position.x <= origPos.x - moveDist)
                {
                    position1 = false;
                    position2 = true;
                }
            }
            if (position2)
            {
                transform.Translate(Vector3.right * speed * Time.deltaTime);
                if (transform.position.x >= origPos.x)
                {
                    position1 = true;
                    position2 = false;
                }
            }
        }
    }

    public void resetPos()
    {
        transform.position = origPos;
        poweredOn = false;
    }

    public void togglePower()
    {
        poweredOn = !poweredOn;
    }
}

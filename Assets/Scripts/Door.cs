using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField]
    private float speed = 5;
    private Vector3 origPos;
    public bool poweredOn;
    public float moveDist = 8f;
    

    // Start is called before the first frame update
    void Start()
    {
        origPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (poweredOn && (transform.position.y - origPos.y) <= moveDist)
        {
            transform.Translate(Vector3.up * speed * Time.deltaTime);
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

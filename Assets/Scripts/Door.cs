using UnityEngine;

public class Door : movingObj
{
    // Update is called once per frame
    void Update()
    {
        if (poweredOn && (transform.position.y - origPos.y) <= moveDist)
        {
            transform.Translate(Vector3.up * speed * Time.deltaTime);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class saw : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles += new Vector3(0, 0, 1000 * Time.deltaTime);
    }
}

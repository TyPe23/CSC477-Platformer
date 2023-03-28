using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePos : MonoBehaviour
{
    public Vector3 worldPos;
    [SerializeField] private Vector3 screenPos;
    private int layer = 6;
    private int layerMask;
    

    void Start()
    {
        layerMask = 1 >> layer; // Bit-wise shift from 0110 --> 0011; using layer 3 which is Ground
    }

    public void Update()
    {
        screenPos = Input.mousePosition;

        Ray ray = Camera.main.ScreenPointToRay(screenPos);

        if (Physics.Raycast(ray, out RaycastHit hitdata, 200.0f, layerMask))
        {
            worldPos = hitdata.point;
        }

        transform.position = worldPos;
        Debug.DrawLine(ray.origin, ray.origin + ray.direction * 5, Color.red); // Debug ray
    }
}
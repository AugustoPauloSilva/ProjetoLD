using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePointer : MonoBehaviour
{
    Vector3 pointerPosition;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        pointerPosition = Camera.main.ViewportToScreenPoint(Input.mousePosition);
        transform.position = pointerPosition;
    }
}

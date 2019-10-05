using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePointer : MonoBehaviour
{
    public int size = 16;
    void Start()
    {
        Cursor.visible = false;
    }

    void Update()
    {
        transform.position = Input.mousePosition+new Vector3(size/2,-size/2,0);
        // transform.position = Input.mousePosition;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceMouse : MonoBehaviour
{
    Vector3 mouseVector;
    Vector3 mousePos;
    Vector3 aux;
    float strength = 5f;
    float armRotate;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        mouseVector = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z);
        mousePos = Camera.main.ScreenToWorldPoint(mouseVector);
        aux = mousePos-transform.position;
        aux.z = 0;
        aux = aux.normalized;
        armRotate = Mathf.Atan(aux.y/aux.x)*180f/3.14f;
        if (aux.x < 0) armRotate = armRotate + 180;
        // armRotate = Vector3.Angle(Vector3.right,aux);
        // if (aux.y < 0) armRotate = -armRotate;
        // armRotate = (armRotate-transform.rotation.z)*Time.deltaTime;
        armRotate = armRotate-transform.rotation.z;
        transform.rotation = Quaternion.Euler(new Vector3(0,0,armRotate));
    }
}

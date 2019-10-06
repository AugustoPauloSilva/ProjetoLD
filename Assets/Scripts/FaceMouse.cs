using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceMouse : MonoBehaviour
{
    Vector3 mouseVector;
    Vector3 mousePos;
    Vector3 aux;
    float armRotate;
    public GameObject arm;
    public static GameObject player;
    public static Transform armPosition;
    // public static RaycastHit2D hit;
    // Start is called before the first frame update
    void Start()
    {
        player = gameObject;
        armPosition = arm.transform;
    }

    // Update is called once per frame
    void Update()
    {
        mouseVector = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z);
        mousePos = Camera.main.ScreenToWorldPoint(mouseVector);
        aux = mousePos-arm.transform.position;
        aux.z = 0;
        aux = aux.normalized;
        armRotate = Mathf.Atan(aux.y/aux.x)*180f/3.14f;
        if (aux.x < 0) {
            armRotate = armRotate + 180;
        }
        armRotate = armRotate-arm.transform.rotation.z;
        arm.transform.rotation = Quaternion.Euler(new Vector3(0,0,armRotate));
        // FaceMouse.hit = Physics2D.Raycast(arm.transform.position,mousePos-arm.transform.position);
    }
    
    // void OnDrawGizmos()
    // {
    //     Gizmos.DrawRay(arm.transform.position,mousePos-arm.transform.position);
    // }
}

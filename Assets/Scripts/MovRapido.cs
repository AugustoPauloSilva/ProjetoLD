using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovRapido : MonoBehaviour
{
    private Vector2 speed;
    public float usedSpeed = 200f;
    Rigidbody2D body;
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        speed = Vector2.zero;
    }

    void Update()
    {

    }

    void FixedUpdate() {
        speed = Vector3.zero;
        if (Input.GetKey(KeyCode.D)){
            speed.x = usedSpeed*Time.fixedDeltaTime;
        }
        else if (Input.GetKey(KeyCode.A)){
            speed.x = -usedSpeed*Time.fixedDeltaTime;
        }
        if (Input.GetKey(KeyCode.W)){
            speed.y = usedSpeed*Time.fixedDeltaTime;
        }
        else if (Input.GetKey(KeyCode.S)){
            speed.y = -usedSpeed*Time.fixedDeltaTime;
        }
        body.velocity = speed;
    }

}

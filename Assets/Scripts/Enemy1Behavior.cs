using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1Behavior : MonoBehaviour
{
    public IsOnGround groundRight;
    public IsOnGround groundLeft;
    public float speed = 10f;
    Rigidbody2D body;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if(!groundRight.isGrounded || !groundLeft.isGrounded)
            speed = -speed;
        body.velocity = new Vector2(speed,body.velocity.y);
    }

    void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag == "Play")
            other.gameObject.GetComponent<PlayerInput>().TakeDamage();
        if(Vector2.Angle(other.GetContact(0).normal,Vector2.up) >= 10f) 
            speed = -speed;
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1Behavior : MonoBehaviour
{
    public IsOnGround groundRight;
    public IsOnGround groundLeft;
    public float speed = 50f;
    public float turnTimer = 15f;
    float turnCount = 0f;
    Rigidbody2D body;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    void Update()
    {

    }

    void FixedUpdate()
    {
        if(!groundRight.isGrounded || !groundLeft.isGrounded && turnCount == 0){
            speed = -speed;
            turnCount = turnTimer;
        }
        if (turnCount > 0) turnCount--;
        body.velocity = new Vector2(speed*Time.fixedDeltaTime,body.velocity.y);
    }

    void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag == "Play"){
            other.gameObject.GetComponent<PlayerInput>().TakeDamage();
            if (speed>0 && (Vector2.Angle(other.GetContact(0).normal,Vector2.right)<10f))
                speed = -speed;
            else if (speed<0 && (Vector2.Angle(other.GetContact(0).normal,Vector2.left)<10f))
                speed = -speed;
        }
        if(Vector2.Angle(other.GetContact(0).normal,Vector2.up) >= 10f) 
            speed = -speed;
    }
}

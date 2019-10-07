using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1Behavior : MonoBehaviour
{
    public IsOnGround groundRight;
    public IsOnGround groundLeft;
    public float speed = 50f;
    public float turnTimer = 15f;
	public int maxhealth = 3;
	public int health;
	public float maxTimeOtherDamage = 0.5f;
	private float timeOtherDamage = 0;
	
    float turnCount = 0f;
    Rigidbody2D body;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
		health = maxhealth;
    }

    void Update()
    {
		if(health <= 0){
			Destroy(gameObject);
		}
    }

    void FixedUpdate()
    {
        if(!groundRight.isGrounded || !groundLeft.isGrounded && turnCount == 0){
            speed = -speed;
            turnCount = turnTimer;
        }
        if (turnCount > 0) turnCount--;
        body.velocity = new Vector2(speed*Time.fixedDeltaTime, 0);
		
		timeOtherDamage -= Time.fixedDeltaTime;
    }

    void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag == "Attack") return;
		
        if(other.gameObject.tag == "Play"){
            other.gameObject.GetComponent<PlayerInput>().TakeDamage(1, false, 0);
            if (speed>0 && (Vector2.Angle(other.GetContact(0).normal,Vector2.right)<10f))
                speed = -speed;
            else if (speed<0 && (Vector2.Angle(other.GetContact(0).normal,Vector2.left)<10f))
                speed = -speed;
        }
        if(Vector2.Angle(other.GetContact(0).normal,Vector2.up) >= 10f) 
            speed = -speed;
    }
	
	public void TakeDamage(int damage){
		//Animacao, ficar piscando por maxDamageTime
		if(timeOtherDamage <= 0){
			health -= damage;
			timeOtherDamage = maxTimeOtherDamage;	//Tempo de ivulnerabilidade, o player n se mexe
		}
	}
}

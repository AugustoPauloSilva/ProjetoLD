using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    //Constantes
    public float usedSpeed = 300f;
    public float jumpSpeed = 1300f;
    public float AccGrav = 35f;
	public float maxFallSpeed = -700f;
	public float dashSpeed = 2000f;
   	public int dashDelay = 0;
	public float maxDashTime = 5;
	public int verticalSpeed = -200;
	public float floatingSpeed = 40;
	public int health;		
	public int maxHealth = 3;
	public bool isGrounded = false;
 	public bool secondJumpAcquired = false;
	public bool dashAcquired = false;
	public float dashTime;
	public float fallDistance = -300f;
	public int fallDamage = 1;					
	private bool jump = false;		
	private int nCollisions = 0;
   	private bool dashRight = false;
    private bool dashLeft = false;
    private bool secondJumpAvailabe = true;
    private Vector2 speed;
	private bool dashAvailable = false;
	private Vector3 lastPosition;
	
    Rigidbody2D body;
	FaceMouse mouse;
	Vector2 normal;
	Animator anim;
	SpriteRenderer spriteRender;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        speed = Vector2.zero;
        mouse = GetComponent<FaceMouse>();
		anim = GetComponent<Animator>();
		spriteRender = GetComponent<SpriteRenderer>();
		lastPosition = Vector3.zero;
    }

    void Update()
    {
		if (Input.GetKeyDown(KeyCode.W) && isGrounded) {
			jump = true;
			anim.SetBool("Jump", true);
		}	
		if (Input.GetKeyDown(KeyCode.W) && !isGrounded && secondJumpAcquired && secondJumpAvailabe)
		{
			jump = true;
			secondJumpAvailabe = false;
		}

		if (Input.GetKeyDown(KeyCode.Space) && dashDelay <= 0)
		{
			if (mouse.arm.transform.rotation.z < 0.7f && mouse.arm.transform.rotation.z >= -0.7f){
				dashRight = true;
				dashTime = maxDashTime;
				spriteRender.flipX = true;
			}
			else {
				dashLeft = true;
				dashTime = maxDashTime;
				spriteRender.flipX = false;
			}
		}
		
		if (speed.x < 0 && mouse.arm.transform.rotation.z < 0.7f && mouse.arm.transform.rotation.z >= -0.7f) {
			anim.SetBool("Moonwalking", true);
		}
		else if (speed.x > 0 && mouse.arm.transform.rotation.z >= 0.7f){
			anim.SetBool("Moonwalking", true);
		}
		else anim.SetBool("Moonwalking", false);
		

		if(isGrounded == false && speed.y < verticalSpeed){
				anim.SetBool("Fall", true);
				anim.SetBool("Jump", false);	
		}
		else {
			anim.SetBool("Fall", false);
			anim.SetBool("Jump", true);
		}
		
		if(isGrounded == false){
		anim.SetBool("OnAir", true);
		}
		else anim.SetBool("OnAir", false);
		
    }

    void OnCollisionStay2D(Collision2D col)
    {
		normal = col.GetContact(0).normal;
		if (Vector2.Angle(normal, new Vector2(0f, 1f)) < 10f)
        {
            isGrounded = true;
			anim.SetBool("OnGround", true);
			secondJumpAvailabe = true;
			dashAvailable = true;
        }
        else if ((Vector2.Angle(normal, new Vector2(1f, 0f)) < 10f) && nCollisions == 1)	
			isGrounded = false;
        else if ((Vector2.Angle(normal, new Vector2(-1f, 0f)) < 10f) && nCollisions == 1)
            isGrounded = false;
    }
	
	 void OnCollisionEnter2D(Collision2D col)
    {
		nCollisions++;
		normal = col.GetContact(0).normal;
		if (Vector2.Angle(normal, new Vector2(0f, 1f)) < 10f){
           speed.y = 0;
		   lastPosition = transform.position;		
		}
		if (Vector2.Angle(normal, new Vector2(0f, -1f)) < 10f)
            speed.y = 0;
    }
	
	void OnCollisionExit2D(Collision2D col){
        isGrounded = false;
		nCollisions--;
		anim.SetBool("OnGround", false);
	}

    void FixedUpdate()
    {
		if (dashTime <= 0)
		{
			speed.x = 0f;

			if (Input.GetKey(KeyCode.D))
			{
				speed.x = usedSpeed * Time.deltaTime;
				spriteRender.flipX = true;
				anim.SetBool("Walk", true);
			}
			
			if (Input.GetKey(KeyCode.A))
			{
				speed.x = -usedSpeed * Time.deltaTime;
				spriteRender.flipX = false;
				anim.SetBool("Walk", true);
			}
			
			if (!(Input.GetKey(KeyCode.D))&& !(Input.GetKey(KeyCode.A))){
				anim.SetBool("Walk", false);
			}

			if (Input.GetKey(KeyCode.W) && !isGrounded)
			{
				speed.y += floatingSpeed * Time.deltaTime;	
			}
			
			if (Input.GetKey(KeyCode.S) && !isGrounded && speed.y > 0) speed.y = 0;

			if (jump)
			{
				speed.y = jumpSpeed * Time.deltaTime;
				jump = false;
			}
			if (!isGrounded){
				speed.y -= AccGrav * Time.deltaTime;
				if (speed.y < maxFallSpeed) speed.y = maxFallSpeed;
			}
				
				
			anim.SetBool("Dash", false);
		}
		else {
			dashTime-= Time.deltaTime;
			speed.y = 0;
		}

		if (dashRight && dashAvailable)
		{
			anim.SetBool("Dash", true);
			speed.x = dashSpeed * Time.deltaTime;
			dashRight = false;	
			dashDelay = 30;
			dashAvailable = false;
		}

		if (dashLeft && dashAvailable)
		{
			anim.SetBool("Dash", true);
			speed.x = -dashSpeed * Time.deltaTime;
			dashLeft = false;
			dashDelay = 30;
			dashAvailable = false;
		}
		
		if(transform.position.y <= fallDistance){	//Rotina de cair do mundo		
			transform.position = lastPosition;	//A posicao eh da primeira interacao com o ultimo objeto que o player ficou em pe		
			speed = Vector2.zero;		
			health -= fallDamage;	//Perde fallDamage de vida		
		}
			
		if (dashDelay > 0) dashDelay--;
		body.velocity = speed;
	}
	
	public void TakeDamage (){		
		Debug.Log("Doeu!");		
	}
	
}

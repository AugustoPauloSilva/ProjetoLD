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
	public int maxDashTime = 5;
	public int verticalSpeed = -200;
	public float floatingSpeed = 40;
	public int health;
	public int maxHealth;

	public bool isGrounded = false;
 	public bool secondJumpAcquired = false;
	public bool dashAcquired = false;
	private bool jump = false;
	public int dashTime;
	private int nCollisions = 0;
   	private bool dashRight = false;
    private bool dashLeft = false;
    private bool secondJumpAvailabe = true;
    public Vector2 speed;
    private Vector2 pos;
	
    Rigidbody2D body;
	FaceMouse mouse;
	Vector2 normal;
	Animator anim;
	SpriteRenderer spriteRender;

    void Start()
    {
        pos = transform.position;
        body = GetComponent<Rigidbody2D>();
        speed = Vector2.zero;
        mouse = GetComponent<FaceMouse>();
		anim = GetComponent<Animator>();
		spriteRender = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
		if (Input.GetKeyDown(KeyCode.W) && isGrounded) {
			jump = true;
			anim.SetBool("Jump", true);
		}
		else anim.SetBool("Jump", false);		
		if (Input.GetKeyDown(KeyCode.W) && !isGrounded && secondJumpAcquired && secondJumpAvailabe)
		{
			jump = true;
			secondJumpAvailabe = false;
		}

		if (Input.GetKeyDown(KeyCode.Space) && dashDelay <= 0)
		{
			print(mouse.arm.transform.rotation.z);
			if (mouse.arm.transform.rotation.z < 0.7f && mouse.arm.transform.rotation.z >= -0.7f)
				dashRight = true;
			else
				dashLeft = true;
		}
		
		if(isGrounded == false && speed.y < verticalSpeed){
				anim.SetBool("Fall", true);
		}
		else anim.SetBool("Fall", false);
		
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
			secondJumpAvailabe = true;
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
		if (Vector2.Angle(normal, new Vector2(0f, 1f)) < 10f)
           speed.y = 0;
		if (Vector2.Angle(normal, new Vector2(0f, -1f)) < 10f)
            speed.y = 0;
    }
	
	void OnCollisionExit2D(Collision2D col){
        isGrounded = false;
		nCollisions--;
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
			} else anim.SetBool("Walk", false);

			if (Input.GetKey(KeyCode.W) && !isGrounded)
			{
				speed.y += floatingSpeed * Time.deltaTime;	
			}
			
			if (Input.GetKey(KeyCode.S) && !isGrounded)
			{
                if (speed.y > 0) speed.y = 0;
                //speed.y -= floatingSpeed * Time.deltaTime;
			}

			if (jump)
			{
				speed.y = jumpSpeed * Time.deltaTime;
				jump = false;
			}
			if (!isGrounded){
				speed.y -= AccGrav * Time.deltaTime;
				if (speed.y < maxFallSpeed) speed.y = maxFallSpeed;
			}
				
		}
		else {
			dashTime--;
			speed.y = 0;
		}

		if (dashRight)
		{
			speed.x = dashSpeed * Time.deltaTime;
			dashRight = false;
			dashTime = maxDashTime;
			dashDelay = 30;
		}

		if (dashLeft)
		{
			speed.x = -dashSpeed * Time.deltaTime;
			dashLeft = false;
			dashTime = maxDashTime;
			dashDelay = 30;
		}

		if (dashDelay > 0) dashDelay--;
		body.velocity = speed;
	}

	public void TakeDamage (){
		Debug.Log("Doeu!");
	}
}

//Coisas p fazer
//Adaptar as animacoes
//Barrinha para dash, se ele esta disponivel
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float usedSpeed = 300f;
    public float jumpSpeed = 700f;
    public float AccGrav = 35f;
    public bool isGrounded = false;
    public bool secondJumpAcquired = false;
    public bool dashAcquired = false;
    public int dashDelay = 0;
	public float maxFallSpeed = -700f;
	public float dashSpeed = 2000f;
	public int maxDashTime = 5;
	
    FaceMouse mouse;
	Vector2 normal;

	bool jump = false;
    public int dashTime;
    private bool dashRight = false;
    private bool dashLeft = false;
    private bool secondJumpAvailabe = true;
    private Vector2 speed;
    private Vector2 pos;
    Rigidbody2D body;

    void Start()
    {
        pos = transform.position;
        body = GetComponent<Rigidbody2D>();
        speed = Vector2.zero;
        mouse = GetComponent<FaceMouse>();
    }

    void Update()
    {
		if (Input.GetKeyDown(KeyCode.W) && isGrounded) jump = true;
		else if (Input.GetKeyDown(KeyCode.W) && !isGrounded && secondJumpAcquired && secondJumpAvailabe)
		{
			jump = true;
			secondJumpAvailabe = false;
		}

		if (Input.GetKeyDown(KeyCode.Space) && dashDelay <= 0)
		{
			if (mouse.arm.transform.rotation.z*180 < 90f && mouse.arm.transform.rotation.z*180 >= -90f)
				dashRight = true;
			else
				dashLeft = true;
		}
    }

    void OnCollisionEnter2D(Collision2D col)
    {
		normal = col.GetContact(0).normal;
		if (col.gameObject.tag == ("Ground") && Vector2.Angle(normal, new Vector2(0f, 1f)) < 10f)
        {
            isGrounded = true;
			secondJumpAvailabe = true;
        }
    }
	
	void OnCollisionExit2D(Collision2D col){
		if (col.gameObject.tag == ("Ground"))
        {
            isGrounded = false;
        }
	}

    void FixedUpdate()
    {
		if (dashTime <= 0)
		{
			speed.x = 0f;

			if (Input.GetKey(KeyCode.D))
			{
				speed.x = usedSpeed * Time.deltaTime;
			}
			else if (Input.GetKey(KeyCode.A))
			{
				speed.x = -usedSpeed * Time.deltaTime;
			}

			if (jump)
			{
				speed.y = jumpSpeed * Time.deltaTime;
				jump = false;
			}
			if (!isGrounded){
				speed.y -= AccGrav * Time.fixedDeltaTime;
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
}

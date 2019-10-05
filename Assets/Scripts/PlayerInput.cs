//Dash funciona na barra de espaço
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float usedSpeed = 100f;
    public float jumpSpeed = 1000f;
    public float AccGrav = 10;
    public bool isGrounded = false;
    public bool secondJumpAcquired = false;
    public bool dashAcquired = false;
    public float dashSpeed = 3000f;
    public int dashTime = 0;
    public int dashDelay = 0;
    FaceMouse bunda;

    bool jump = false;
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
        bunda = GetComponent<FaceMouse>();
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
            if (bunda.arm.transform.rotation.z*180 < 90f && bunda.arm.transform.rotation.z*180 >= -90f)
            {
                Debug.Log("Direita " + bunda.arm.transform.rotation.z);
                dashRight = true;
            }
            else
            {
                Debug.Log("Esquerda");
                dashLeft = true;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        Vector2 normal = col.GetContact(0).normal;

        if (Vector2.Angle(normal, new Vector2(0f, 1f)) < 10f)
        {
            isGrounded = true;
            secondJumpAvailabe = true;
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
                isGrounded = false;
                jump = false;
            }

            if (!isGrounded) speed.y -= AccGrav * Time.fixedDeltaTime;
        }

        else dashTime--;

        if (dashRight)
        {
            speed.x = dashSpeed * Time.deltaTime;
            dashRight = false;
            dashTime = 3;
            dashDelay = 30;
        }

        if (dashLeft)
        {
            speed.x = -dashSpeed * Time.deltaTime;
            dashLeft = false;
            dashTime = 3;
            dashDelay = 30;
        }

        if (dashDelay > 0) dashDelay--;
        body.velocity = speed;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float usedSpeed = 100f;
    public float jumpSpeed = 400f;
    public float AccGrav = 10;
    public bool isGrounded = false;
    public bool secondJumpAcquired = false;

    bool jump = false;

    private bool secondJumpAvailabe = true;
    private Vector2 speed;
    private Vector2 pos;
    Rigidbody2D body;

    // Start is called before the first frame update
    void Start()
    {
        pos = transform.position;
        body = GetComponent<Rigidbody2D>();
        speed = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            jump = true;
        }
        else if (Input.GetKeyDown(KeyCode.W) && !isGrounded && secondJumpAcquired && secondJumpAvailabe)
        {
            jump = true;
            secondJumpAvailabe = false;
        }
        else if (Input.GetKey(KeyCode.S))
        {

        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == ("Ground") && isGrounded == false)
        {
            isGrounded = true;
            secondJumpAvailabe = true;
        }
    }

    void FixedUpdate()
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
        if (jump){
            speed.y = jumpSpeed * Time.deltaTime;
            isGrounded = false;
            jump = false;
        }
        if (!isGrounded) speed.y -= AccGrav * Time.fixedDeltaTime;
        body.velocity = speed;
    }

}

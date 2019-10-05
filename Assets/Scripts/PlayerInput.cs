using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float usedSpeed = 100f;
    public float jumpSpeed = 400f;
    public float AccGrav = 10;
    public bool isGrounded;

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

    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == ("Ground") && isGrounded == false)
        {
            isGrounded = true;
        }
    }

    void FixedUpdate()
    {
        speed.x = 0f;
        if (Input.GetKey(KeyCode.D))
        {
            speed.x = usedSpeed * Time.fixedDeltaTime;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            speed.x = -usedSpeed * Time.fixedDeltaTime;
        }
        if (Input.GetKey(KeyCode.W) && isGrounded)
        {
            speed.y = jumpSpeed * Time.fixedDeltaTime;
            isGrounded = false;
        }
        else if (Input.GetKey(KeyCode.S))
        {

        }
        if(!isGrounded) speed.y -= AccGrav * Time.fixedDeltaTime;
        body.velocity = speed;
    }

}

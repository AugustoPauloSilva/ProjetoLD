using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsOnGround : MonoBehaviour
{
    public bool isGrounded = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.tag != "Play" && other.tag != "Drag" && other.tag != "Destructible") 
            isGrounded = false;
    }
    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag != "Play" && other.tag != "Drag" && other.tag != "Destructible") 
            isGrounded = true;
    }
}

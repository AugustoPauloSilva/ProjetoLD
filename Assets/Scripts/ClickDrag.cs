using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickDrag : MonoBehaviour
{
    public float maxSpeed = 1500f;
    public float smallSpeed = 100f;
    public float bigDistance = 5f;
    public float smallDistance = 0.5f;
    float usedSpeed = 0f;
    bool isDragging = false;
    Rigidbody2D body;
    Vector3 mousePos;
    RaycastHit2D hit2;

    public void clickReaction(){
        // if (hit2.collider.gameObject.tag == "Play") isDragging = !isDragging;
        isDragging = !isDragging;
    }

    public void clickReaction2(){
        isDragging = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
            Input.mousePosition.y, -Camera.main.transform.position.z));
        // hit2 = FaceMouse.hit;
        if (isDragging){
            Vector2 aux = mousePos-transform.position;
            if (aux.magnitude > bigDistance) usedSpeed = Mathf.Lerp(usedSpeed,maxSpeed,0.05f);
            else if (aux.magnitude > smallDistance) usedSpeed = Mathf.Lerp(usedSpeed,smallSpeed,0.05f);
            else usedSpeed = 0;
            aux = aux.normalized;
            body.velocity = aux*usedSpeed*Time.fixedDeltaTime;
        }
    }

    void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Play" || other.gameObject.tag == "Enemy") 
            isDragging = false;
    }

    void OnCollisionStay2D(Collision2D other) {
        if (Vector2.Angle(other.GetContact(0).normal,Vector2.up) < 10f && !isDragging) 
            body.velocity = Vector2.zero;
    }
}

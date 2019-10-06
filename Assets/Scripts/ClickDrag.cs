using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickDrag : MonoBehaviour
{
    public float maxSpeed = 100f;
    public float bigDistance = 2f;
    float usedSpeed = 0f;
    bool isDragging = false;
    Rigidbody2D body;
    Vector3 mousePos;
    RaycastHit2D hit2;

    public void clickReaction(){
        if (hit2.collider.gameObject.tag == "Play") isDragging = !isDragging;
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
        hit2 = FaceMouse.hit;
        if (isDragging){
            Vector2 aux = mousePos-transform.position;
            if (aux.magnitude > bigDistance) usedSpeed = Mathf.Lerp(usedSpeed,maxSpeed,0.1f);
            else {
                Vector2 aux2 = Vector2.Lerp(transform.position,mousePos,0.001f);
                aux2 = mousePos;
                body.MovePosition(aux2);
                body.velocity = Vector2.zero;
                return;
            }
            aux = aux.normalized;
            body.velocity = aux*usedSpeed*Time.fixedDeltaTime;
        }
    }
}

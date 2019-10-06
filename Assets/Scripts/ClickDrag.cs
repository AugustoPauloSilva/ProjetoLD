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
    Vector3 mouseVector;

    public void clickReaction(){
        isDragging = !isDragging;
    }

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDragging){
            mouseVector = new Vector3(Input.mousePosition.x, 
                Input.mousePosition.y, -Camera.main.transform.position.z);
            mousePos = Camera.main.ScreenToWorldPoint(mouseVector);
            Vector2 aux = mousePos-transform.position;
            if (aux.magnitude > bigDistance) usedSpeed = Mathf.Lerp(usedSpeed,maxSpeed,0.1f);
            else {
                Vector2 aux2 = Vector2.Lerp(transform.position,mousePos,0.001f);
                aux2 = mousePos;
                body.MovePosition(aux2);
                return;
            }
            aux = aux.normalized;
            body.velocity = aux*usedSpeed;
        }
    }
}

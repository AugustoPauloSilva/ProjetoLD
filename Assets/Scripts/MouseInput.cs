using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInput : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.tag == "Drag" && Input.GetMouseButtonDown(0) && 
                FaceMouse.player.GetComponent<PlayerInput>().usingTelekinesis){
            if (!isOver) GetComponent<ClickDrag>().clickReaction2();
            else isOver = false;
        }
    }

    void OnMouseOver() {
        if (gameObject.tag == "Destructible" && Input.GetMouseButtonDown(1)){
            GetComponent<ClickObstacle>().clickReaction();
        }
        else if (gameObject.tag == "Drag" && Input.GetMouseButtonDown(0) && 
                FaceMouse.player.GetComponent<PlayerInput>().usingTelekinesis){
            GetComponent<ClickDrag>().clickReaction();
            isOver = true;
        }
    }
}

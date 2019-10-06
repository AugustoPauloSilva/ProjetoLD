using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehavior : MonoBehaviour
{
    public float walkFrequency = 50f;
    public float walkDistance = 500f;
    public float walkSpeed = 200f;
    float walked = 0f;
    PlayerInput playerScript;
    // Start is called before the first frame update
    void Start()
    {
        playerScript = FaceMouse.player.GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate() {
        
    }
}

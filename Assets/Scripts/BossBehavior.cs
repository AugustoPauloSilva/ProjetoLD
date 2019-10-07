using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehavior : MonoBehaviour
{
    public float walkFrequency = 300f;
    public float walkDistance = 800f;
    public float walkSpeed = 400f;
    public float meleeRange = 25f;
    public float meleeDelay = 30f;
    public GameObject arm;
    float walked = 0f;
    float walkTimer = 0f;
    float meleeTimer = 0f;
    float distance;
    float direction = 1;
    float usedAngle = 90f;
    float initAngle = 90f;
    float finishAngle = -20f;
    bool hasAttacked = true;
    bool meleeAtack = false;
    PlayerInput playerScript;
    Rigidbody2D body;
    // Start is called before the first frame update
    void Start()
    {
        playerScript = FaceMouse.player;
        body = GetComponent<Rigidbody2D>();
        walked = walkDistance;
    }

    // Update is called once per frame
    void Update()
    {
        if (walked >= walkDistance && !hasAttacked){
            meleeAtack = false;
            if (Mathf.Abs(distance) < meleeRange){
                meleeAtack = true;
                usedAngle = initAngle;
                hasAttacked = true;
                meleeTimer = 0;
            }
            else{

            }
        }
    }

    void FixedUpdate() {
        distance = playerScript.transform.position.x-transform.position.x;
        direction = distance;
        direction = (int)(direction/Mathf.Abs(direction));
        if (walkTimer > walkFrequency){
            walkTimer = 0;
            walked = 0;
            usedAngle = initAngle;
            hasAttacked = false;
            body.velocity = new Vector2(direction*walkSpeed*Time.fixedDeltaTime,0);
        }
        else if (walked >= 0 && walked < walkDistance){
            walked += walkSpeed*Time.fixedDeltaTime;
            body.velocity = new Vector2(direction*walkSpeed*Time.fixedDeltaTime,0);
            if (walked >= walkDistance) body.velocity = Vector2.zero;
        }
        else{
            walkTimer++;
            body.velocity = Vector2.zero;
            if (meleeAtack){
                meleeTimer++;
                if(meleeTimer < meleeDelay) return;
                arm.SetActive(true);
                if (direction == 1) {
                    usedAngle = Mathf.Lerp(usedAngle,finishAngle,0.2f);
                    arm.transform.rotation = Quaternion.Euler(0,0,usedAngle);
                    if (usedAngle < finishAngle+5f){
                        usedAngle = initAngle;
                        arm.transform.rotation = Quaternion.Euler(0,0,usedAngle);
                        meleeAtack = false;
                        arm.SetActive(false);
                    }
                }
                else{
                    usedAngle = Mathf.Lerp(usedAngle,180-finishAngle,0.2f);
                    arm.transform.rotation = Quaternion.Euler(0,0,usedAngle);
                    if (usedAngle > 175f-finishAngle){
                        usedAngle = initAngle;
                        arm.transform.rotation = Quaternion.Euler(0,0,usedAngle);
                        meleeAtack = false;
                        arm.SetActive(false);
                    }
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Play"){
            other.GetComponent<PlayerInput>().TakeDamage(1, false, 0);
        }
    }

    void OnCollisionStay2D(Collision2D other) {
        if (other.gameObject.tag == "Play"){
            walked = walkDistance;
            walkTimer = 0;
            hasAttacked = true;
            usedAngle = initAngle;
            arm.transform.rotation = Quaternion.Euler(0,0,usedAngle);
            body.velocity = Vector2.zero;
            other.gameObject.GetComponent<PlayerInput>().TakeDamage(1, false, 0f);
        }
    }
}

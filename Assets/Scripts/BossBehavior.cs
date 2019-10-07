using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehavior : MonoBehaviour
{
    public float walkFrequency = 200f;
    public float walkDistance = 1200f;
    public float walkSpeed = 1000f;
    public float meleeRange = 15f;
    public float meleeDelay = 10f;
    public float rangedSpeed = 2000f;
    public GameObject arm;
    public GameObject ranged;
    public GameObject[] rangeds;
    float walked = 0f;
    float walkTimer = 0f;
    float meleeTimer = 0f;
    float distance;
    float direction = 1;
    float usedAngle = 90f;
    float initAngle = 90f;
    float finishAngle = -20f;
    float rangedFinish = 100f;
    float rangedTravel = 0f;
    bool hasAttacked = true;
    bool meleeAttack = false;
    bool rangedAttack = false;
    Vector3 rangedOffset;
    PlayerInput playerScript;
    Rigidbody2D body;

    void Start()
    {
        playerScript = FaceMouse.player;
        body = GetComponent<Rigidbody2D>();
        walked = walkDistance;
        rangedOffset = Vector3.zero;
        rangedOffset.x = -7f;
    }

    void Update()
    {
        if (walked >= walkDistance && !hasAttacked){
            meleeAttack = false;
            rangedAttack = false;
            if (Mathf.Abs(distance) < meleeRange){
                meleeAttack = true;
                hasAttacked = true;
                usedAngle = initAngle;
                meleeTimer = 0;
            }
            else if (Mathf.Abs(distance) < rangedFinish/2){
                rangedAttack = true;
                hasAttacked = true;
                if (direction == 1) ranged.transform.position += 5*rangedOffset;
                else ranged.transform.position -= 5*rangedOffset;
            }
        }
    }

    void FixedUpdate() {
        distance = playerScript.transform.position.x-transform.position.x;

        if (walkTimer > walkFrequency){
            direction = distance;
            direction = (int)(direction/Mathf.Abs(direction));
            walkTimer = 0;
            walked = 0;
            usedAngle = initAngle;
            hasAttacked = false;
            body.velocity = new Vector2(direction*walkSpeed*Time.fixedDeltaTime,0);
        }
        else if (walked >= 0 && walked < walkDistance){
            walked += walkSpeed*Time.fixedDeltaTime;
            body.velocity = new Vector2(direction*walkSpeed*Time.fixedDeltaTime,0);
            ranged.transform.position = transform.position;
            if (walked >= walkDistance) body.velocity = Vector2.zero;
        }
        else{
            walkTimer++;
            body.velocity = Vector2.zero;
            if (meleeAttack){
                meleeActivate();
            }
            else if (rangedAttack){
                rangedActivate();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Play"){
            other.GetComponent<PlayerInput>().TakeDamage(1, false, 0);
        }
    }

    void OnTriggerStay2D(Collider2D other) {
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

    void meleeActivate(){
        meleeTimer++;
        if(meleeTimer < meleeDelay) return;
        arm.SetActive(true);
        if (direction == 1) {
            usedAngle = Mathf.Lerp(usedAngle,finishAngle,0.2f);
            arm.transform.rotation = Quaternion.Euler(0,0,usedAngle);
            if (usedAngle < finishAngle+5f){
                usedAngle = initAngle;
                arm.transform.rotation = Quaternion.Euler(0,0,usedAngle);
                meleeAttack = false;
                arm.SetActive(false);
            }
        }
        else{
            usedAngle = Mathf.Lerp(usedAngle,180-finishAngle,0.2f);
            arm.transform.rotation = Quaternion.Euler(0,0,usedAngle);
            if (usedAngle > 175f-finishAngle){
                usedAngle = initAngle;
                arm.transform.rotation = Quaternion.Euler(0,0,usedAngle);
                meleeAttack = false;
                arm.SetActive(false);
            }
        }
    }

    void rangedActivate(){
        rangedTravel += rangedSpeed*Time.fixedDeltaTime/25;
        if (direction == 1){
            if (rangedTravel > 12f) rangeds[4].SetActive(true);
            if (rangedTravel > 24f) rangeds[3].SetActive(true);
            if (rangedTravel > 36f) rangeds[2].SetActive(true);
            if (rangedTravel > 48f) rangeds[1].SetActive(true);
            if (rangedTravel > 60f) rangeds[0].SetActive(true);
            ranged.transform.position += new Vector3
                (rangedSpeed*Time.fixedDeltaTime/25f,0,0);
            if (ranged.transform.position.x-transform.position.x > rangedFinish){
                rangedAttack = false;
                // ranged.SetActive(false);
                rangedTravel = 0;
                rangeds[0].SetActive(false);
                rangeds[1].SetActive(false);
                rangeds[2].SetActive(false);
                rangeds[3].SetActive(false);
                rangeds[4].SetActive(false);
            }
        }
        else{
            if (rangedTravel > 12f) rangeds[0].SetActive(true);
            if (rangedTravel > 24f) rangeds[1].SetActive(true);
            if (rangedTravel > 36f) rangeds[2].SetActive(true);
            if (rangedTravel > 48f) rangeds[3].SetActive(true);
            if (rangedTravel > 60f) rangeds[4].SetActive(true);
            ranged.transform.position -= new Vector3
                (rangedSpeed*Time.fixedDeltaTime/25f,0,0);
            if (ranged.transform.position.x-transform.position.x < -rangedFinish){
                rangedAttack = false;
                // ranged.SetActive(false);
                rangedTravel = 0f;
                rangeds[0].SetActive(false);
                rangeds[1].SetActive(false);
                rangeds[2].SetActive(false);
                rangeds[3].SetActive(false);
                rangeds[4].SetActive(false);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehavior : MonoBehaviour
{
    public float walkPeriod = 100f;
    public float walkDistance = 800f;
    public float walkSpeed = 1700f;
    public float meleeRange = 12f;
    public float meleeDelay = 5f;
    public float rangedDelay = 12f;
    public float rangedSpeed = 1900f;
    public float bombYSpeed = 50f;
    public float bossDiffScale = 50f;
    public int attackCycle = 3;
    public int stunTime = 50;
    public GameObject arm;
    public GameObject ranged;
    public GameObject[] rangeds;
    public GameObject bombOrigin;
    float walked = 0f;
    float walkTimer = 0f;
    float meleeTimer = 0f;
    float rangedTimer = 0f;
    float distance;
    float direction = 1;
    float usedAngle = 90f;
    float initAngle = 90f;
    float finishAngle = -20f;
    float rangedFinish = 90f;
    float rangedTravel = 0f;
    int attackCount = 0;
    int stunCount = 0;
    bool hasAttacked = true;
    bool meleeAttack = false;
    bool rangedAttack = false;
    bool bombAttack = false;
    bool isStunned = false;
    Vector3 rangedOffset;
    PlayerInput playerScript;
    Rigidbody2D body;
	public int maxhealth = 3;
	public int health;
	public float maxTimeOtherDamage = 0.2f;
	private float timeOtherDamage = 0;

    void Start()
    {
        playerScript = FaceMouse.player;
        body = GetComponent<Rigidbody2D>();
        walked = walkDistance;
        rangedOffset = Vector3.zero;
        health = maxhealth;
        rangedOffset.x = -7f;

        walkPeriod += bossDiffScale*2;
        walkDistance -= bossDiffScale*4;
        walkSpeed -= bossDiffScale*8;
        rangedSpeed -= bossDiffScale*4;
    }

    void Update()
    {
        if (walked >= walkDistance && !hasAttacked){
            meleeAttack = false;
            rangedAttack = false;
            bombAttack = false;
            attackCount++;
            if (attackCount > attackCycle){
                bombAttack = true;
                hasAttacked = true;
                attackCount = 0;
            }
            else if (Mathf.Abs(distance) < meleeRange){
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
                rangedTimer = 0;
            }
        }
    }

    void FixedUpdate() {
        distance = playerScript.transform.position.x-transform.position.x;
        if (timeOtherDamage>0) timeOtherDamage = timeOtherDamage*Time.fixedDeltaTime;
        else timeOtherDamage = 0;

        if (isStunned){
            stunCount++;
            body.velocity = Vector2.zero;

            if (stunCount > stunTime){
                gameObject.tag = "Boss";
                isStunned = false;
            }
            return;
        }
        if (walkTimer > walkPeriod){
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
            else if (bombAttack){
                bombActivate();
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
        rangedTimer++;
        if (rangedTimer < rangedDelay) return;
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
    void bombActivate(){
        Vector3 aux2 = transform.position;
        aux2.y += 7f;
        aux2.x += 3*direction;
        aux2.z = -1;
        GameObject aux = Instantiate(bombOrigin,aux2,transform.rotation);
        aux.GetComponent<Rigidbody2D>().velocity = new Vector2(direction*0.6f*bombYSpeed,1.2f*bombYSpeed);
        if (attackCycle >= 5){
            aux2.x += 2*direction;
            aux2.y -= 5f;
            aux2.z = -1;
            aux = Instantiate(bombOrigin,aux2,transform.rotation);
            aux.GetComponent<Rigidbody2D>().velocity = new Vector2(direction*0.5f*bombYSpeed,bombYSpeed);
        }
        bombAttack = false;
    }

	public void TakeDamage(int damage){
		//Animacao, ficar piscando por maxDamageTime
		if(timeOtherDamage <= 0 && gameObject.tag != "Boss"){
			health -= damage;
			timeOtherDamage = maxTimeOtherDamage;	//Tempo de ivulnerabilidade, o player n se mexe
            gameObject.tag = "Boss";
            isStunned = false;
            walkPeriod -= bossDiffScale;
            walkDistance += bossDiffScale*2;
            walkSpeed += bossDiffScale*4;
            rangedSpeed += bossDiffScale*2;
            attackCycle++;
            if (health <= 0) Destroy(gameObject);
		}
	}

    public void takeBomb(){
        isStunned = true;
        gameObject.tag = "Enemy";
        stunCount = 0;
        rangeds[0].SetActive(false);
        rangeds[1].SetActive(false);
        rangeds[2].SetActive(false);
        rangeds[3].SetActive(false);
        rangeds[4].SetActive(false);
        hasAttacked = true;
        meleeAttack = false;
        rangedAttack = false;
        bombAttack = false;
        // if (attackCycle < 5){
        //     walkPeriod -= bossDiffScale;
        //     walkDistance += bossDiffScale*2;
        //     walkSpeed += bossDiffScale*4;
        //     rangedSpeed += bossDiffScale*2;
        //     attackCycle++;
        // }
    }
}

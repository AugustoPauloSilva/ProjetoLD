﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBehavior : MonoBehaviour
{
    public float explosionScale = 4f;
    bool isExploding = false;
    bool endExplosion = false;
    float usedScale;
    float initScale;
    bool playerHit = false;
    bool bossHit = false;
    bool enemyHit = false;
    void Start()
    {
        usedScale = transform.localScale.x;
        initScale = transform.localScale.x;
    }

    void FixedUpdate() {
        if (isExploding){
            if (!endExplosion){
                usedScale = Mathf.Lerp(usedScale,explosionScale,0.2f);
                transform.localScale = new Vector3(usedScale,usedScale,transform.localScale.z);
                if (usedScale > explosionScale*0.9) endExplosion = true;
            }
            else if (usedScale > initScale*0.75){
                usedScale = Mathf.Lerp(usedScale,initScale/2,0.2f);
                transform.localScale = new Vector3(usedScale,usedScale,transform.localScale.z);
            }
            else{
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.tag == "Play" && !playerHit){
            other.GetComponent<PlayerInput>().TakeDamage(1, false, 0);
            playerHit = true;
        }
        else if (other.tag == "Boss" && !bossHit){
            other.GetComponent<BossBehavior>().takeBomb();
            bossHit = true;
            enemyHit = true;
        }
        else if (other.tag == "Enemy" && !enemyHit){
            if (other.GetComponent<BossBehavior>() != null) 
                other.GetComponent<BossBehavior>().TakeDamage(1);
            else if (other.GetComponent<Enemy1Behavior>() != null) 
                other.GetComponent<Enemy1Behavior>().TakeDamage(1);
            enemyHit = true;
        }
    }

    void OnCollisionEnter2D(Collision2D other) {
        isExploding = true;
        GetComponent<Collider2D>().isTrigger = true;
        GetComponent<ClickDrag>().isDragging = false;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }
}

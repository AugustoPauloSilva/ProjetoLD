using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vida : MonoBehaviour
{
	public int maxHealth = 10;
	public bool isDead = false;
	public int ivunerabilityTime = 0;
	
	public int health;
	private float angleOfPush;
	private GameObject obj;
	private bool isPushed;
    
	void Start()
    {
        obj = GetComponent<GameObject>();
		health = maxHealth;
    }

    void Update()
    {
        
    }
	
	public Vida TakeDamage(int damage, bool _isPushed, float _angleOfPush){
		if(ivunerabilityTime == 0){
			health -= damage;
			ivunerabilityTime = 30;
		}
		isPushed = _isPushed;
		angleOfPush = _angleOfPush;
		return this;
	}
	
	public void Attack(Vida oponent, int damage, bool _isPushed, float _angleOfPush){
		oponent.TakeDamage(damage, _isPushed, _angleOfPush);
		return;
	}
	
	void FixedUpdate()
    {
		if(ivunerabilityTime > 0) ivunerabilityTime--;
        if(health <= 0){
			isDead = true;
		}
    }
}

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
	public int maxhealth = 3;
	public int health;
	public float maxTimeOtherDamage = 0.2f;
	private float timeOtherDamage = 0;
	
    // Start is called before the first frame update
    void Start()
    {
        // playerScript = FaceMouse.player.GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate() {
        
    }
	public void TakeDamage(int damage){
		//Animacao, ficar piscando por maxDamageTime
		if(timeOtherDamage <= 0){
			health -= damage;
			timeOtherDamage = maxTimeOtherDamage;	//Tempo de ivulnerabilidade, o player n se mexe
		}
	}
}

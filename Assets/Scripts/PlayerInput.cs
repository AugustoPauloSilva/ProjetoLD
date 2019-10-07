using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    //Constantes
    public float usedSpeed = 300f;
    public float jumpSpeed = 1300f;
    public float AccGrav = 35f;
	public float maxFallSpeed = -700f;
	public float dashSpeed = 2000f;
   	public int dashDelay = 0;
	public float maxDashTime = 5;
	public float floatingSpeed = 40;
	public float dashTime;
	public float fallDistance = -300f;
	public int fallDamage = 1;
	public float maxTimeOtherDamage = 0.5f;
	public float maxAttackDelay = 0.1f;
	
	//Variaveis
	public bool isGrounded = false;
 	public bool secondJumpAcquired = false;
	public bool dashAcquired = false;
	public bool usingTelekinesis = false;
	private bool jump = false;		
	private int nCollisions = 0;
   	private bool dashRight = false;
    private bool dashLeft = false;
    private bool secondJumpAvailabe = true;
    private Vector2 speed;
	private bool dashAvailable = false;
	private Vector3 lastPosition;
	private float timeOtherDamage;
	public float maxHealth = 10;
	public float health;
	public float attackDelay = 0;
	public bool battleMode = true; //True = battlemode, false = magicmode
	public bool attack = false;
	
    private Rigidbody2D body;
	private FaceMouse mouse;
	private Vector2 normal;
	private Animator anim;
	private SpriteRenderer spriteRender;
	public GameObject Animations;
	[SerializeField] private HealthBar healthBar;
	public GameObject claw;
	
    void Start(){
        body = GetComponent<Rigidbody2D>();
        speed = Vector2.zero;
        mouse = GetComponent<FaceMouse>();
		anim = GetComponent<Animator>();
		anim = Animations.GetComponent<Animator>();
		spriteRender = Animations.GetComponent<SpriteRenderer>();
		lastPosition = Vector3.zero;
		health = maxHealth;
    }

    void Update(){
		if (Input.GetKeyDown(KeyCode.W) && isGrounded) {
			jump = true;
			anim.SetBool("Jump", true);
		}	
		if (Input.GetKeyDown(KeyCode.W) && !isGrounded && secondJumpAcquired && secondJumpAvailabe)
		{
			jump = true;
			secondJumpAvailabe = false;
		}
		if (Input.GetKeyDown(KeyCode.Keypad1)){
			usingTelekinesis = !usingTelekinesis;
		}

		if (Input.GetKeyDown(KeyCode.Space) && dashDelay <= 0 && dashAvailable)
		{
			if (mouse.arm.transform.rotation.z < 0.7f && mouse.arm.transform.rotation.z >= -0.7f){
				dashRight = true;
				dashTime = maxDashTime;
				spriteRender.flipX = true;
			}
			else {
				dashLeft = true;
				dashTime = maxDashTime;
				spriteRender.flipX = false;
			}
			
			dashAvailable = false;
		}
		
		if (speed.x < 0 && mouse.arm.transform.rotation.z < 0.7f && mouse.arm.transform.rotation.z >= -0.7f) {
			anim.SetBool("Moonwalking", true);
		}
		else if (speed.x > 0 && mouse.arm.transform.rotation.z >= 0.7f){
			anim.SetBool("Moonwalking", true);
		}
		else anim.SetBool("Moonwalking", false);
		

		if(isGrounded == false && speed.y < 0){
				anim.SetBool("Fall", true);
				anim.SetBool("Jump", false);	
		}
		else {
			anim.SetBool("Fall", false);
			anim.SetBool("Jump", true);
		}
		
		if(isGrounded == false){
		anim.SetBool("OnAir", true);
		}
		else anim.SetBool("OnAir", false);
		
		if(health <= 0){	//Rotina de diminuir vida e checar se esta vivo
			healthBar.SetSize(0);
			print("Game over");
		}
		else healthBar.SetSize(health/maxHealth);
		
		if(Input.GetKeyDown(KeyCode.Q)) battleMode = true;
		
		if(Input.GetKeyDown(KeyCode.E)) battleMode = false;
		
		if(Input.GetKeyDown(KeyCode.Mouse0) && (battleMode = true) && (attackDelay <= 0)){ // Ativa a variavel ataque
			attack = true;
		}
    }

	//Colisoes
    void OnCollisionStay2D(Collision2D col){
		normal = col.GetContact(0).normal;
		if (Vector2.Angle(normal, new Vector2(0f, 1f)) < 10f)
        {
            isGrounded = true;
			anim.SetBool("OnGround", true);
			secondJumpAvailabe = true;
			dashAvailable = true;
        }
        else if ((Vector2.Angle(normal, new Vector2(1f, 0f)) < 10f) && nCollisions == 1)	
			isGrounded = false;
        else if ((Vector2.Angle(normal, new Vector2(-1f, 0f)) < 10f) && nCollisions == 1)
            isGrounded = false;
    }
	
	 void OnCollisionEnter2D(Collision2D col){
		nCollisions++;
		normal = col.GetContact(0).normal;
		if (Vector2.Angle(normal, new Vector2(0f, 1f)) < 10f){
			speed.y = 0;
			if(col.gameObject.tag == "Ground")
				lastPosition = transform.position;
		}
		if (Vector2.Angle(normal, new Vector2(0f, -1f)) < 10f)
			speed.y = 0;
		if(col.otherCollider.gameObject.tag == "Attack" && col.gameObject.tag == "Enemy"){ // rotina de ataque
			print("ola");
		}
    }
	
	void OnCollisionExit2D(Collision2D col){
        isGrounded = false;
		nCollisions--;
		anim.SetBool("OnGround", false);
	}

    void FixedUpdate(){
		//if(timeOtherDamage <= 0){	//Checa se o cooldown de tomar dano ja saiu
			if (dashTime <= 0f){	//Checa se o cooldown do dash ja acabou
				//Coisas que nao podem ser feitas durante o dash estao aqui
				speed.x = 0f;

				if (Input.GetKey(KeyCode.D))
				{
					speed.x = usedSpeed * Time.deltaTime;
					spriteRender.flipX = true;
					anim.SetBool("Walk", true);
				}
				
				if (Input.GetKey(KeyCode.A))
				{
					speed.x = -usedSpeed * Time.deltaTime;
					spriteRender.flipX = false;
					anim.SetBool("Walk", true);
				}
				
				if (!(Input.GetKey(KeyCode.D))&& !(Input.GetKey(KeyCode.A))){
					anim.SetBool("Walk", false);
				}

				if (Input.GetKey(KeyCode.W) && !isGrounded)
				{
					speed.y += floatingSpeed * Time.deltaTime;	
				}
				
				if (Input.GetKey(KeyCode.S) && !isGrounded && speed.y > 0) speed.y = 0;

				if (jump)
				{
					speed.y = jumpSpeed * Time.deltaTime;
					jump = false;
				}
				if (!isGrounded){
					speed.y -= AccGrav * Time.deltaTime;
					if (speed.y < maxFallSpeed) speed.y = maxFallSpeed;
				}
				
				anim.SetBool("Dash", false);
			}
			else {	//Coisas que devem ser feitas durante o dash estao aqui
				dashTime -= Time.deltaTime;
				speed.y = 0;	//Enquanto esta no dash a vel em y eh 0
			}
			//Coisas que devem ser feitas com ou sem dash ativado
			if (dashRight)
			{
				anim.SetBool("Dash", true);
				speed.x = dashSpeed * Time.deltaTime;
				dashRight = false;	
				dashDelay = 30;	
			}

			if (dashLeft)
			{
				anim.SetBool("Dash", true);
				speed.x = -dashSpeed * Time.deltaTime;
				dashLeft = false;
				dashDelay = 30;
			}
			
			if(transform.position.y <= fallDistance){	//Rotina de cair do mundo		
				transform.position = lastPosition;	//A posicao eh da primeira interacao com o ultimo objeto que o player ficou em pe
				speed = Vector2.zero;
				jump = true;
				TakeDamage(fallDamage, false, 0);
			}
				
			if (dashDelay > 0) dashDelay--;	//Recarregar uso do dash
			body.velocity = speed;
			
			if(attack){
				attackDelay = maxAttackDelay;
				
			}
			attackDelay -= Time.deltaTime;
		//}
		 timeOtherDamage -= Time.deltaTime;
	}
	
	public void TakeDamage(int damage, bool _isPushed, float _angleOfPush){
		//Animacao, ficar piscando por maxDamageTime
		if(timeOtherDamage <= 0){	
			health -= damage;
			if(_isPushed){
				speed.x = Mathf.Cos(_angleOfPush*Mathf.PI/180);
				speed.y = Mathf.Sin(_angleOfPush*Mathf.PI/180);
			}
			timeOtherDamage = maxTimeOtherDamage;	//Tempo de ivulnerabilidade, o player n se mexe
		}
	}
}

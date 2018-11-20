﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    public float speed = 10.0f;
    public float jumpForce;
	public int Player_current_lvl;
	public int Player_current_exp;

  private Rigidbody2D rb;
	private bool canMove;
	public GameObject SE;
  public bool facingRight = true;
  
	private float timeBtwAttack;
	public float startTimeBtwAttack;

	public Transform attackPos;
	public LayerMask whatIsEnemies;
	public float attackRange;
	public int damage;

    private bool isGrounded;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;
	public LayerMask groundLayers;

	public Animator animator;
	public Animator animator2;
	public Animator animator3;

	public GameObject Main, Knight, Mage;
	public int PlayerSelect;

    private int extraJumps;
    public int extraJumpsValue;

	private bool attack;
	private PlayerHealth playerHealth;

    //Updating UI Selection of Shape Visual;
    
    public GameObject actionbar_baseform;
    public GameObject actionbar_shape1;
    public GameObject actionbar_shape2;
    public Image baseform_inactive;
    public Image shape1_inactive;
    public Image shape2_inactive;


	// Use this for initialization
	void Start () {
		playerHealth = gameObject.GetComponent<PlayerHealth> ();
		Player_current_lvl = 0;
		canMove = true;
		attack = false;
		PlayerSelect = 1;
		Main = GameObject.Find ("Main");
		Knight = GameObject.Find ("Knight_P");
		//animator.SetBool ("isJumping", false);
        extraJumps = extraJumpsValue;
        rb = GetComponent<Rigidbody2D>();
	}


    // Update is called once per frame
    private void Update() {
		/*isGrounded =Physics2D.OverlapArea (new Vector2 (transform.position.x - 0.5f, transform.position.y - 0.5f),
			new Vector2 (transform.position.x + 0.5f, transform.position.y - 0.5f), groundLayers);*/

		isGrounded =Physics2D.OverlapArea (new Vector2 (transform.position.x - 0.5f, transform.position.y - 0.5f),
			new Vector2 (transform.position.x + 0.5f, transform.position.y - 0.5f), groundLayers);

		HandleJumpAndFall ();
		//Debug.Log ();
		MoveHor ();
		Jump ();
        crouch();
		selectForm ();
		Shift ();

        // if(currentForm == knight)
        KnightBlock();
        KnightDash();

		if(isGrounded == true)
		{
			CharAttack ();
		}
    }



    void FixedUpdate () {
		

		
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

	void OnDrawGizmosSelected() {
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere (attackPos.position, attackRange);
	}

	void MoveHor()
	{
		// left & right movement
		/*if (Input.GetButtonDown ("Horizontal")) {
			Audio.PlaySound ("PlayMove");
		}*/
		float xTranslation = Input.GetAxis ("Horizontal");
		animator.SetFloat ("Speed", Mathf.Abs (xTranslation)); //set the speed for the animator
		animator2.SetFloat ("Speed", Mathf.Abs (xTranslation));
		animator3.SetFloat ("Speed", Mathf.Abs (xTranslation));

		if (playerHealth.currentHealth > 0) {
            if (attack)
            {
                rb.velocity = new Vector2(0.0f, 0.0f);
            } else
			rb.velocity = new Vector2 (xTranslation * speed, rb.velocity.y);

		}
			// flips sprite if moving the other direction
			if ((facingRight == true && xTranslation < 0) || (facingRight == false && xTranslation > 0))
				Flip ();


	}

	/*void Melee(){
		Sword.enabled = !Sword.enabled;
	}*/

	void CharAttack() {

		if (Input.GetButtonDown("Attack") && timeBtwAttack <= 0 &&
			(PlayerSelect==1 || PlayerSelect ==2)) {
			if (Input.GetButtonDown ("Attack")) {
				Audio.PlaySound ("PlayerAttack");
				animator.SetBool ("isAttacking", true);
				animator2.SetBool ("isAttacking", true);
				AttDelay ();
				timeBtwAttack = startTimeBtwAttack;
				Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll (attackPos.position, attackRange, whatIsEnemies);
				DamageFactor ();
				for (int i = 0; i < enemiesToDamage.Length; i++) {
					enemiesToDamage [i].GetComponent <Enemy> ().TakeDamage (damage);
				}

			} /*else if (Input.GetButtonDown ("Attack") && timeBtwAttack <= 0 &&
                attack = true;
                Invoke("resetAttack", .25f);

            } /*else if (Input.GetButtonDown ("Attack") && timeBtwAttack <= 0 &&
				PlayerSelect ==3) {



			}*/

		} else {
			
			timeBtwAttack -= Time.deltaTime;
			animator.SetBool ("isAttacking", false);
			animator2.SetBool ("isAttacking", false);
		}

	}

    void resetAttack()
    {
        attack = false;
    }

	void selectForm(){
		if (Input.GetButtonDown ("Base") && PlayerSelect != 1) { //Grab input and then select a model for the player 
			Instantiate (SE, transform.position, transform.rotation = Quaternion.identity);
			Audio.PlaySound ("Shift");
			PlayerSelect = 1;
		} else if (Input.GetButtonDown ("Knight") && PlayerSelect != 2) {
			Instantiate (SE, transform.position, transform.rotation = Quaternion.identity);
			Audio.PlaySound ("Shift");
			PlayerSelect = 2;
		} else if (Input.GetButtonDown ("Mage") && PlayerSelect != 3) {
			Instantiate (SE, transform.position, transform.rotation = Quaternion.identity);
			Audio.PlaySound ("Shift");
			PlayerSelect = 3;
		}
	}


	void Shift(){
		if (PlayerSelect == 1) { //the actually changing of the models
			speed = 7.0f; //base form will the fastest
			jumpForce = 13.5f;
			Main.SetActive (true); 
			Knight.SetActive (false);
			Mage.SetActive (false);

            baseshapeimage.color = new Color32(212, 240, 241, 255);
            shape1image.color = new Color32(0, 0, 0, 255);
            shape2image.color = new Color32(0, 0, 0, 255);

            actionbar_baseform.SetActive(true);
            actionbar_shape1.SetActive(false);
            actionbar_shape2.SetActive(false);

            baseform_inactive.enabled = false;
            shape1_inactive.enabled = true;
            shape2_inactive.enabled = true;

        } else if (PlayerSelect == 2) { //just using else statement for now. will change whenever we start to put in more forms
			speed = 3.5f; //knight will the slowest
			jumpForce = 0.0f; // knights will be unable to jump
			Main.SetActive (false);
			Knight.SetActive (true);
			Mage.SetActive (false);

            baseshapeimage.color = new Color32(0, 0, 0, 255);
            shape1image.color = new Color32(212, 240, 241, 255);
            shape2image.color = new Color32(0, 0, 0, 255);
      
            actionbar_baseform.SetActive(false);
            actionbar_shape1.SetActive(true);
            actionbar_shape2.SetActive(false);

            baseform_inactive.enabled = true;
            shape1_inactive.enabled = false;
            shape2_inactive.enabled = true;

        } else if (PlayerSelect ==3) {
			speed = 5.0f; //knight will the slowest
			jumpForce = 11.0f; // knights will be unable to jump
			Main.SetActive (false);
			Knight.SetActive (false);
			Mage.SetActive (true);

            baseshapeimage.color = new Color32(0, 0, 0, 255);
            shape1image.color = new Color32(0, 0, 0, 255);
            shape2image.color = new Color32(212, 240, 241, 255);

            actionbar_baseform.SetActive(false);
            actionbar_shape1.SetActive(false);
            actionbar_shape2.SetActive(true);

            baseform_inactive.enabled = true;
            shape1_inactive.enabled = true;
            shape2_inactive.enabled = false;

        }
        
}

    void crouch(){
        if (Input.GetButton("Crouch"))
        {
            animator.SetBool("isCrouch", true);
        } else
        {

            animator.SetBool("isCrouch", false);
        }

    }

	void Jump(){
		if(isGrounded == true ) {
			animator.SetBool ("isJumping", false);
			animator.SetBool ("isFalling", false);
			extraJumps = extraJumpsValue;
		}
		if (isGrounded == false) {
			HandleJumpAndFall ();
		}
		if (Input.GetButtonDown("Jump") && extraJumps > 0 ) {
			if (PlayerSelect ==1){
				Audio.PlaySound ("Jumping");
			}
			HandleJumpAndFall ();
			//animator.SetBool ("isJumping", false);
			//animator.SetBool ("isJumping", true);
			rb.velocity = Vector2.up * jumpForce;
			extraJumps--;
		} else if(Input.GetButtonDown("Jump") && extraJumps == 0 && isGrounded == true ) {
			if (PlayerSelect ==1){
				Audio.PlaySound ("Jumping");
			}
			HandleJumpAndFall();
			//animator.SetBool ("isJumping", false);
			//animator.SetBool ("isJumping", true);
			rb.velocity = Vector2.up * jumpForce;
		}


	}

	void DamageFactor()
	{
		if (PlayerSelect == 1) {
			damage = 45;
		} else if (PlayerSelect == 2) {
			damage = 100;
		}
	}

	void AttDelay(){
		if (PlayerSelect == 1) {
			startTimeBtwAttack = .5f;
		} else if (PlayerSelect == 2) {
			startTimeBtwAttack = 1.0f;
		}
	}
	/*void OnDrawGizmosSelected(){

	}*/

	void HandleJumpAndFall(){
		if (isGrounded == false) {
			if (rb.velocity.y > 0) {
				animator.SetBool ("isJumping", true);
			} else if (rb.velocity.y < 0) {
				animator.SetBool ("isJumping", false);
				animator.SetBool ("isFalling", true);
			}
		}
	}

	public IEnumerator Knockback(float KnockDur){

		float timer = 0;
		while (KnockDur > timer) {

			timer += Time.deltaTime;
			rb.AddForce (new Vector3 (-.3f, 1300f, transform.position.z));
		}

		yield return 0;
	}

	void OnCollisionEnter2D (Collision2D other){
		if (other.transform.tag == "MovingPlatform") {
			transform.parent = other.transform;
		}
	}

	void OnCollisionExit2D (Collision2D other){
		if (other.transform.tag == "MovingPlatform") {
			transform.parent = null;
		}
	}

	/*void addExp(int exp) {
		Player_current_exp += exp;
		lvlcheck ();
		Debug.Log (Player_current_exp);
	}

	void lvlcheck(){
		if (Player_current_exp == 20) {
			Player_current_lvl = +1;
			Player_current_exp = 0;
		}
	}*/

    // Knight abilities: block, dash

    void KnightBlock()
    {
        // Set animator to trigger
        if (Input.GetButton("Block"))
        {
            Debug.Log("Is Blocking");
            animator2.SetBool("isBlocking", true);
            playerHealth.isBlocking = true;
        }
        else
        {
            animator2.SetBool("isBlocking", false);
            playerHealth.isBlocking = false;
        }
    }
    /*
    int buttonCount = 0;
    float buttonTimer = 0.5f;
    */

    float dashTimer = 0f;
    bool canDash = true;

    int buttonCount = 0;
    float buttonTimer = 0.5f;

    void KnightDash()
    {
        if (Input.anyKeyDown)
        {
            if (buttonTimer > 0 && buttonCount == 1)
            {
                rb.velocity += new Vector2(rb.velocity.x * 5, 0.1f);
            }
            else
            {
                buttonTimer = 0.5f;
                buttonCount += 1;
            }
        }
        if (buttonTimer > 0)
        {
            buttonTimer -= 1 * Time.deltaTime;
        }
        else
        {
            buttonCount = 0;
        }
        /*
        if (Input.GetKey(KeyCode.LeftShift) && canDash)
        {
            if (facingRight)
            {
                rb.AddForce(Vector3.right * 500);

            }
            else
            {
                rb.AddForce(Vector3.left * 100);
            }

            dashTimer += Time.deltaTime * 3;
        }
        if (dashTimer > .5f)
        {
            canDash = false;
        }
        if (dashTimer < .5f && dashCooldown == false)
        {
            canIDash = true;
        }
        if (dashTimer <= 0)
        {
            dashCooldown = false;
        }
        */

    }
}
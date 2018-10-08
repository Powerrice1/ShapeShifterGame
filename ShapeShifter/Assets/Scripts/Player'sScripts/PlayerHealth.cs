﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {

	public int maxHealth;
	public int currentHealth;
	private PlayerController player;
    public Animator animator;

	// Use this for initialization
	void Start () {
		currentHealth = maxHealth;
		player = gameObject.GetComponent<PlayerController> ();
	}

	// Update is called once per frame
	void Update () {
		if (currentHealth <= 0) {
			Destroy (gameObject);
            animator.SetBool("playerDies", true);
		}
	}

	void FixedUpdate() {
		
	}

	public void TakeDamage(int dam){
		player.animator.Play ("Hurt");
		currentHealth -= dam;
		Debug.Log ("damage TAKEN");
	}
	 

}
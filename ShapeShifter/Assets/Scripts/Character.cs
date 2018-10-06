﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour {

    public Animator MyAnimator { get; private set; }

    [SerializeField]
    protected float movementSpeed;

    protected bool facingRight;

    
    public int currentHealth;

    public bool Attack { get; set; }


	// Use this for initialization
	public virtual void Start () {
        Debug.Log("Character start");
        facingRight = true;
        MyAnimator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}




    public void ChangeDirection() {
        Debug.Log("Changing Direction");
        facingRight = !facingRight;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, 1);
    }
}

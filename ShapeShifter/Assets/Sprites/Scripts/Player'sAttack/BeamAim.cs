﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamAim : MonoBehaviour {

	public GameObject WarningShot;
	public GameObject Laser;
	public Transform LaserPoint;

    public int ManaCost;
	private float timeBtwShots;
	public float startTimeBtwShots;


    private Mana pMana;

    // Use this for initialization
    void Start () {
        pMana = gameObject.GetComponentInParent<Mana>();
    }
	
	// Update is called once per frame
	void Update () {

		Vector3 mouse = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		transform.position = new Vector3 (mouse.x, transform.position.y, transform.position.z);

		if (timeBtwShots <= 0 && pMana.currentMana >= ManaCost) {
			if (Input.GetMouseButtonDown (1)) {
				//player.animator3.SetTrigger ("BasicAtt");
				Destroy (Instantiate (WarningShot, LaserPoint.position, transform.rotation), 1.75f);
				//yield return new WaitForSeconds (.35f);
				Destroy (Instantiate (Laser, LaserPoint.position, transform.rotation), 5.0f);
				timeBtwShots = startTimeBtwShots;
                pMana.UseMana(ManaCost);
            }
		} else {
			timeBtwShots -= Time.deltaTime;
		}

	}
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Grab and move script for a single target object.
// Must be attached to an object with a HandTracking component.
public class Grabbing : MonoBehaviour {

	public float grabDistance = 0.1f;
	[HideInInspector] public bool grabbed=false;
	public GameObject ball;
	private OVRInput.Controller controller;

	private HandTracking ht;
	private Kinetics kball;
	private Renderer rball;
	private Renderer rglove;

	// Use this for initialization
	void Start () 
	{
		// We query the handcontroller script to determine which touch controller we should use
		ht = gameObject.GetComponent<HandTracking> ();
		if (ht == null) {
			Debug.LogError("No HandTracking component found");
		}

		rball = ball.GetComponent<Renderer> ();
		kball = ball.GetComponent<Kinetics> ();
		rglove = gameObject.GetComponent<Renderer> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		Vector3 gloveCurrentPosition = mop.GetObjectPosition(rglove);

		float trigger;
		if (ht.controller == OVRInput.Controller.LTouch) {
			trigger = OVRInput.Get (OVRInput.RawAxis1D.LHandTrigger) + OVRInput.Get (OVRInput.RawAxis1D.LIndexTrigger);
		} else {
			trigger = OVRInput.Get (OVRInput.RawAxis1D.RHandTrigger) + OVRInput.Get (OVRInput.RawAxis1D.RIndexTrigger);
		}

		if (!grabbed && (trigger > 0)) {
			float distance = Vector3.Distance (mop.GetObjectPosition (rball), gloveCurrentPosition);
			if (distance < grabDistance) {
				grabbed = true;
				kball.kineticsActive = false;
			}
		}

		if (grabbed && (trigger == 0)) {
			grabbed = false;
			kball.kineticsActive = true;
		}

		if (grabbed) {
			mop.SetObjectPosition (rball, gloveCurrentPosition);
		}
	}
}

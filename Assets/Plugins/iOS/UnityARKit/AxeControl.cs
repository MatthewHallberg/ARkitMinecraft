using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeControl : MonoBehaviour {

	Animation anim;
	Camera camera;

	// Use this for initialization
	void Start () {

		anim = GetComponent<Animation> ();
		camera = Camera.main;
	}

	public void UseWeapon(){

		anim.Play ();
		//cast ray from center of screen to see if it hits a block
		Ray ray = camera.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit)) {

			blockHealthBehavior thisBlockHealth;

			if (hit.transform.gameObject.GetComponent<blockHealthBehavior> () != null){
				thisBlockHealth = hit.transform.gameObject.GetComponent<blockHealthBehavior> ();

				GetComponent<AudioSource> ().Play ();

				if (thisBlockHealth.health == 0) {
					thisBlockHealth.Explode ();
				} else {
					thisBlockHealth.health--;
				}
			}

		} else {
			print ("I'm looking at nothing!");
		}
	}
	
	// Update is called once per frame
	void Update () {
		
		
	}
}

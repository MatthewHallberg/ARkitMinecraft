using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtBlockBehavior : MonoBehaviour {

	private Vector3 thisPosition;

	void Start(){
		
		transform.parent = GameObject.Find ("GrassBlockParent").transform;
		transform.localPosition = thisPosition;
		transform.localScale = new Vector3 (1f, 1f, 1f);
	}

	public void UpdateBlock(Vector3 newPosition){

		thisPosition = newPosition;
	}

}

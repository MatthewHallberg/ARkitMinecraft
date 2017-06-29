using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blockHealthBehavior : MonoBehaviour {

	public int health = 2;
	public GameObject explosion;
	
	public void Explode(){

		GameObject explosionObject = Instantiate (explosion,this.transform.position,Quaternion.identity);
		explosionObject.transform.localScale = new Vector3(explosionObject.transform.localScale.x/3,explosionObject.transform.localScale.y/3,explosionObject.transform.localScale.z/3);
		Destroy (gameObject);
	}
}

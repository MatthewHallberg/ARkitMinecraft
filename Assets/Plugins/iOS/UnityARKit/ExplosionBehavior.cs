using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBehavior : MonoBehaviour {

	public float radius = 5.0F;
	public float power = 10.0F;

	void Start()
	{
		StartCoroutine (WaitToExplode ());
	}

	IEnumerator WaitToExplode(){

		yield return new WaitForSeconds (.1f);
		Vector3 explosionPos = transform.position;
		Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
		foreach (Collider hit in colliders)
		{
			Rigidbody rb = hit.GetComponent<Rigidbody>();

			if (rb != null)
				rb.AddExplosionForce(power, explosionPos, radius, 3.0F);
		}

		yield return new WaitForSeconds (1f);
		Destroy (gameObject);
	}
}
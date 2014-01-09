using UnityEngine;
using System.Collections;

public class testCollision : MonoBehaviour {
	void OnCollisionEnter(Collision test) {
		print("FHAHHLFHILH:");
		gameObject.rigidbody.isKinematic = true;
	}
}

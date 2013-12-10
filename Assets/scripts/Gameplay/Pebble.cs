using UnityEngine;
using System.Collections;

public class Pebble : MonoBehaviour {

	
	private Transform thisTransform;
	private Rigidbody thisRigidbody;
	public WaveCreator instFootWave;

	void Awake()
	{
		thisTransform = transform;
		thisRigidbody = rigidbody;
	}
	void Update () {
		if(rigidbody.velocity == Vector3.zero && rigidbody.angularVelocity == Vector3.zero)
		{
			Destroy(gameObject);
		}
	}

	public void switchON()
	{
		thisRigidbody.isKinematic = false;
		thisRigidbody.useGravity = true;
		//thisTransform.position = new Vector3(3.5f,3.5f,6f);
		//rigidbody.velocity = new Vector3(velX, 10, 0);
		
		//rigidbody.velocity = new Vector3(Random.Range(-2,2), 3.5f, 0); // randomize ball position
	}
	public void switchOFF()
	{
		thisRigidbody.isKinematic = true;
		thisRigidbody.useGravity = false;
	}

	void OnTriggerEnter(Collider other) {
		if(/*other.gameObject.CompareTag("soundStopper") ||*/ other.gameObject.CompareTag("Player"))//if(other.gameObject.name == "Tiles")
		{
//			thisRigidbody.isKinematic = true;
//			thisRigidbody.useGravity = false;
			//thisRigidbody.AddForce(new Vector3(10f,20f,0));
		}
	}
	public void setPosition (float x, float y, float z) {
		thisTransform.position = new Vector3(x,y,z);
	}
	public void throwPebble (float power, int dir=1) {
		switchON();
		//thisRigidbody.AddForce(new Vector3(10f,20f,0));
		thisRigidbody.AddForce(new Vector3(8f*power*dir,8f*power,0f));
	}
}

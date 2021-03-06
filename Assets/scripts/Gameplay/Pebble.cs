using UnityEngine;
using System.Collections;

public class Pebble : MonoBehaviour {
	
	
	private Transform thisTransform;
	private Rigidbody thisRigidbody;
	[HideInInspector] public WaveCreator soundEmitt;
	private GameObject GOsoundEmitt;
	private bool isSounding;
	private Transform callerObj;
	public FESound _CollisionSound;
	
	void Awake()
	{
		thisTransform = transform;
		thisRigidbody = rigidbody;
		GOsoundEmitt = Instantiate(Resources.Load("Prefabs/04Gameplay/SoundWavesPeeble")) as GameObject;
		soundEmitt = GOsoundEmitt.GetComponent<WaveCreator>();
		soundEmitt.gameObject.name = "_pebbleWave";
		soundEmitt.gameObject.transform.parent = GameObject.Find("Level/Waves/").transform;
		//soundEmitt = Instantiate(instWave) as WaveCreator;
		soundEmitt.createCircle(thisTransform);
		soundEmitt.setParent(thisTransform);
		_CollisionSound = GameObject.Find("Player/SFX_Pebble").GetComponent<FESound>();
		if (_CollisionSound == null)
		{
			Debug.LogError ("Error to find player, plz fix");
		}
	}
	
	void Update () {
		if(rigidbody.velocity.x >= -0.1f && rigidbody.velocity.x <= 0.1f && rigidbody.velocity.y >= -0.1f && rigidbody.velocity.y <= 0.1f && rigidbody.angularVelocity == Vector3.zero)
		{
			StartCoroutine("killPebble");
		}
	}
	IEnumerator killPebble() {
		yield return new WaitForSeconds(soundEmitt.getLifeTime());
		enabled = false;
		soundEmitt.destroyCircle();
		Destroy(gameObject);
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
		if(/*other.gameObject.CompareTag("soundStopper") ||*/ other.gameObject.CompareTag("pebbleKiller"))//if(other.gameObject.name == "Tiles")
		{
			//			thisRigidbody.isKinematic = true;
			//			thisRigidbody.useGravity = false;
			//thisRigidbody.AddForce(new Vector3(10f,20f,0));
			soundEmitt.circleWalkToSprint();
			/*if(!isSounding) {*/isSounding=true;soundEmitt.resetCircle();/*}*/
//			if (other.gameObject.GetComponent<Environment>() == null)
//			{
//				Debug.Log("There's no environment comp on the object");
//			}
//			else
//			{
//				Environment _env = other.gameObject.GetComponent<Environment>();
				_CollisionSound.playSound();
//			}
		}
		if(other.gameObject.CompareTag("Enemy") && !callerObj.CompareTag("Enemy") && other.GetComponent<Enemy>().getTarget().tag != "Pebble") {
			soundEmitt.circleWalkToSprint();isSounding=true;soundEmitt.resetCircle();print("QUI M'A JET2 CE CAILLOU");
				other.GetComponent<Enemy>().setTarget(callerObj);
				other.GetComponent<Enemy>().activeChasing();
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
	public void setCallerObject (Transform obj) {
		callerObj = obj;
	}
	public Transform getCallerObject () {
		return callerObj;
	}
}

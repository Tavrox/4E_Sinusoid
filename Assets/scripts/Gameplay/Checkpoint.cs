using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour {

	private Player _player;

	// Use this for initialization
	void Start () {
		_player = GameObject.FindWithTag("Player").GetComponent<Player>();
	}
	
	// Update is called once per frame
//	void Update () {
//	
//	}
	void OnTriggerEnter(Collider other) 
	{
		if(other.gameObject.CompareTag("Player")) 
		{
			_player.spawnPos =  new Vector3(transform.position.x,transform.position.y,_player.transform.position.z);
		}
	}

}

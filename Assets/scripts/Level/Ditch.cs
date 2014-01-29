using UnityEngine;
using System.Collections;

public class Ditch : MonoBehaviour {

	// Update is called once per frame
//	void Update () {
//	
//	}
	void OnTriggerEnter(Collider other) 
	{
		if(other.gameObject.CompareTag("Player")) 
		{
			GameEventManager.TriggerGameOver(gameObject);
		}
	}
}

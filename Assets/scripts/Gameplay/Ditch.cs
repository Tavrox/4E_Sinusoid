using UnityEngine;
using System.Collections;

public class Ditch : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameEventManager.GameOver += GameOver;
	}
	
	// Update is called once per frame
//	void Update () {
//	
//	}
	void OnTriggerEnter(Collider other) 
	{
		if(other.gameObject.CompareTag("Player")) 
		{
			GameEventManager.TriggerGameOver();
		}
	}

	private void GameOver () {

	}
}

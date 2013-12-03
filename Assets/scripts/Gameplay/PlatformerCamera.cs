using UnityEngine;
using System.Collections;

public class PlatformerCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void dirCamera()
	{
		//GameObject player = GameObject.FindGameObjectWithTag("Player");
		GetComponent<Camera>().transform.position = new Vector3(0f,0f,0f);
	}
}

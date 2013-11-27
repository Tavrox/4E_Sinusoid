using UnityEngine;
using System.Collections;

public class FEDebug : MonoBehaviour {

	private Player objToDebug;
	private GameObject objToDebug2;
	private GameObject objToDebug3;

	// Use this for initialization
	void Start () 
	{
		objToDebug = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
//		objToDebug2;
//		objToDebug3
	}
	
	// Update is called once per frame
	void Update () 
	{

	}

	void OnGUI()
	{



		// Object 1 //
		
		objToDebug.moveVel = GUI.HorizontalSlider (new Rect (25, 25, 100, 30), objToDebug.moveVel, 0f, 10f);

		// Object 2 //

		// Object 3 //

	}
}

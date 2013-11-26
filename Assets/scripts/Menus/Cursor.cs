using UnityEngine;
using System.Collections;

public class Cursor : MonoBehaviour {
	
	private OTSprite childSprite;
	private Ray ray;
	private RaycastHit hit;
	
	// Use this for initialization
	void Start () 
	{
		childSprite = GetComponentInChildren<OTSprite>();
		childSprite.frameName = "";
	}
	// Update is called once per frame
	void Update () 
	{
		transform.position = new Vector3(OT.view.mouseWorldPosition.x, OT.view.mouseWorldPosition.y, -1f);
	}
}

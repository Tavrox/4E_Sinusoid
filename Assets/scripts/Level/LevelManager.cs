using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {
	
	public OTSprite background;
	[SerializeField] private Player player;
	[SerializeField] private Camera myCamera;
	
	public int ID;
	public int nextLvlID;
	public int previousLvlID;

	// Use this for initialization
	void Start () 
	{
		player = GameObject.FindWithTag("Player").GetComponent<Player>();
		//myCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		myCamera.transform.position = new Vector3 (player.transform.position.x, 0, player.transform.position.z);
		myCamera.nearClipPlane = -1000;
	}
}

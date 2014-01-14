using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour {

	public int ID;
	public int nextLvlID;
	public int previousLvlID;
	public Checkpoint lastCheckpointValidated;
	private Player _player;
	private List<GameObject> gameplayObjects;

	// Use this for initialization
	void Start () 
	{
		_player = GameObject.FindWithTag("Player").GetComponent<Player>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		FETool.anchorToObject(FETool.findWithinChildren(this.gameObject, "TopPlane"), Camera.main.gameObject, "xy");
	}

	public void setActivatedCheckpoint(Checkpoint _check)
	{
		lastCheckpointValidated = _check;
	}

	public void addGameplayObjects(List<GameObject> _list)
	{
		gameplayObjects = _list;
	}

	public void revivePlayer()
	{
		/*
		Transform _cam = GameObject.Find("UI/Main Camera").transform.transform;
		if (_cam == null)
		{Debug.Log("Camera hasn't been found");}
		OTTween _tween = new OTTween(_cam,2f).Tween("position", new Vector3(_Anchor.transform.position.x, _Anchor.transform.position.y, _cam.position.z), OTEasing.StrongOut );
		*/
	}

	public void leaveForMenu()
	{
		// Save informations of player, cp, level reached.
		// go tmenu
		Application.LoadLevel("MainMenu");
	}
}

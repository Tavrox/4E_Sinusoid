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
//		GameEventManager.GameStart += GameStart;
//		GameEventManager.GameOver += GameOver;
//		GameEventManager.GamePause += GamePause;
//		GameEventManager.GameUnpause += GameUnpause;
		GameEventManager.Respawn += Respawn;

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

	public void Respawn()
	{
		_player.transform.position = new Vector3(lastCheckpointValidated.transform.position.x, lastCheckpointValidated.transform.position.y, _player.transform.position.z); ;
		_player.enabled = true;
	}

	public void leaveForMenu()
	{
		// Save informations of player, cp, level reached.
		// go tmenu
		Application.LoadLevel("MainMenu");
	}
}

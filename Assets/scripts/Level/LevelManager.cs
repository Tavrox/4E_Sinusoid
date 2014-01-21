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
	private GameObject _hidingPlane;

	// Use this for initialization
	void Start () 
	{
		GameEventManager.GameStart += GameStart;
		GameEventManager.Respawn += Respawn;

		if (GameObject.FindWithTag("Player").GetComponent<Player>() != null)
		{
			_player = GameObject.FindWithTag("Player").GetComponent<Player>();
		}
		_hidingPlane = FETool.findWithinChildren(this.gameObject, "TopPlane");


		GameEventManager.TriggerGameStart();
	}
	
	// Update is called once per frame
	void Update () 
	{
		FETool.anchorToObject(_hidingPlane, Camera.main.gameObject, "xy");
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
		_player.transform.position = new Vector3(lastCheckpointValidated.transform.position.x, lastCheckpointValidated.transform.position.y, _player.transform.position.z);
		_player.enabled = true;
		lastCheckpointValidated.playSoundRespawn();

	}

	public void leaveForMenu()
	{
		Application.LoadLevel("MainMenu");
	}

	public void GameStart()
	{
		if (FEDebug.testMode == false)
		{
			_player.transform.position = new Vector3(lastCheckpointValidated.transform.position.x, lastCheckpointValidated.transform.position.y - 3.5f, _player.transform.position.z);
		}
	}
}

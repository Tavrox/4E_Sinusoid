using UnityEngine;
using System.Collections;

public class LevelDoor : Checkpoint {
	
	public enum doorType { BeginLevel, EndLevel }
	private LevelManager lvlManager;
	public doorType myDoorType;
	public bool isLevelDoor;
	
	// Use this for initialization
	void Start () {
		lvlManager = GameObject.FindWithTag("LevelManager").GetComponent<LevelManager>();
		GameEventManager.NextLevel += NextLevel;
		GameEventManager.PreviousLevel += PreviousLevel;
	}
//	void Update () {
//		if(null) FindObjectOfType(typeof(LevectorMoveDoor));	
//	}
	void OnTriggerEnter(Collider other)
    {
		if(other.gameObject.CompareTag("Player") && myDoorType == doorType.BeginLevel) 
		{	
			GameEventManager.TriggerPreviousLevel();
		}
		if (other.gameObject.CompareTag("Player") && myDoorType == doorType.EndLevel)
		{
			GameEventManager.TriggerNextLevel();
		}
    }
	
	private void PreviousLevel () {
		Application.LoadLevel(lvlManager.previousLvlID);
	}

	private void NextLevel ()
	{
		Application.LoadLevel(lvlManager.nextLvlID);
	}
	
}

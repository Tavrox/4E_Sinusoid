using UnityEngine;
using System.Collections;

public class LevelBrick : MonoBehaviour {

	public enum brickEnum
	{
		Turret,
		Crate,
		WalkerPoints,
		Rusher,
		Drop,
		Ditch,
		LevelEntry,
		LevelExit,
		Checkpoint
	};
	public brickEnum brickType;

	// Use this for initialization
	void Start () {
	
	}

}

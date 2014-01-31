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
	private bool hasDiedOnce;
	private DistanceChecker _distCheck;
	public bool isOccupied = false;

	// Use this for initialization
	public virtual void Start ()
	{	

	}

}

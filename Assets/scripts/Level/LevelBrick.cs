using UnityEngine;
using System.Collections;

public class LevelBrick : MonoBehaviour {

	public enum brickEnum
	{
		Turret,
		Crate,
		Walker,
		Rusher,
		Drop,
		Ditch
	};
	public brickEnum brickType;

	// Use this for initialization
	void Start () {
	
	}

}

using UnityEngine;
using System.Collections;

public class UIThing : MonoBehaviour {

	public enum Thingtypes
	{
		PebbleCount,
		Tutorial,
		Sidebar,
		MuteSound,
		MuteMusic,
		ReloadFromCP
	};
	public Thingtypes thingTypesList;
	private LevelManager _LevMan;

	// Use this for initialization
	void Start () {
		
		_LevMan = GameObject.FindWithTag("LevelManager").GetComponent<LevelManager>();
	}

	void OnMouseDown()
	{
		switch (thingTypesList)
		{
			case (Thingtypes.ReloadFromCP) :
			{
				_LevMan.revivePlayer();
				break;
			}

		}
	}
}

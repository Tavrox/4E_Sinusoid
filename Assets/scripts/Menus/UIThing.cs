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
		ReloadFromCP,
		None,
		BackToMainMenu,
		PauseGame,
		ResumeGame
	};
	public Thingtypes thingTypesList;
	private LevelManager _LevMan;
	public bool anchoredToScreen;
	public bool isIngame = false;
	private OTSprite spr;

	// Use this for initialization
	void Start () {

		GameEventManager.GameOver += GameOver;
		GameEventManager.Respawn += Respawn;
		if (collider != null)
		{collider.enabled = false;}
		_LevMan = GameObject.FindWithTag("LevelManager").GetComponent<LevelManager>();
		spr = gameObject.GetComponentInChildren<OTSprite>();
		if (spr != null && isIngame == false )
		{
			spr.alpha = 0f;
		}
	}

	void Update()
	{
		if (anchoredToScreen)
		{
			FETool.anchorToObject(this.gameObject, Camera.main.gameObject, "xy");
		}
	}

	void OnMouseDown()
	{
		switch (thingTypesList)
		{
		case (Thingtypes.ReloadFromCP) :
		{
			GameEventManager.TriggerRespawn();
			break;
		}
		case (Thingtypes.BackToMainMenu) :
		{
			_LevMan.leaveForMenu();
			break;
		}
		case (Thingtypes.ResumeGame) :
		{
			GameEventManager.TriggerGameUnpause();
			break;
		}
		}
	}

	public void makeAppear()
	{
		if (spr != null)
		{new OTTween(spr, 1f).Tween("alpha", 1f);}
		if (collider != null)
		{collider.enabled = true;}
	}

	public void makeDisappear()
	{
		if (spr != null)
		{new OTTween(spr, 1f).Tween("alpha", 0f);}
		if (collider != null)
		{collider.enabled = false;}
	}
	
	private void GameOver()
	{
		
	}
	private void Respawn()
	{
		
	}
}

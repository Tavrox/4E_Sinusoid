using UnityEngine;
using System.Collections;

public class IngameUI : MonoBehaviour {

	public enum ListAction
	{
		LaunchScene,
		MuteSound,
		LowerSound,
		RaiseSound,
		FunStuff, // To do funny miscellaneous stuff in menus :)
	}
	public ListAction action;
	private Object prefabSprite;
//	private OTSprite childSpr;
	public static bool exists;
	
	void Start () 
	{
		GameEventManager.GameStart += GameStart;
		GameEventManager.GameOver += GameOver;
		GameEventManager.GamePause += GamePause;
		GameEventManager.GameUnpause += GameUnpause;
//		childSpr = GetComponentInChildren<OTSprite>();
		
		exists = true;
	}
	
	// Update is called once per frame
	void Update () 
	{

	}
	private void OnMouseOver()
	{
	}
	
	private void OnMouseDown()
	{
//		switch (action)
//		{
//
//		}
	}
	private void checkExistingMenu()
	{
		
	}
	private void GameStart () 
	{
		
	}
	private void GameOver () 
	{
		
	}
	private void GamePause()
	{
		
	}
	private void GameUnpause()
	{
		
	}
}

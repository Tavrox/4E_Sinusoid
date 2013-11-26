using UnityEngine;
using System.Collections;

public class MenuUI : MonoBehaviour {
	
	public enum ListMenu
	{
		Notebook,
		Trombinoscope,
		CloseMenu,
	}
	public ListMenu menu;
	public static bool exists;
	
	// Use this for initialization
	void Start () {
	
		GameEventManager.GameStart += GameStart;
		GameEventManager.GameOver += GameOver;
		GameEventManager.GamePause += GamePause;
		GameEventManager.GameUnpause += GameUnpause;
		
		exists = true;
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
	private void OnMouseDown()
	{
		switch (menu)
		{
			case (ListMenu.CloseMenu) :
				{
					GameEventManager.TriggerGameUnpause();
					break;
				}
		}
	}
	private void GameStart () 
	{
		
	}
	private void GameOver () 
	{
		
	}
	
	private void GamePause()
	{
		if (this == null)
		{
			switch (menu)
			{
				case (ListMenu.CloseMenu) :
					{
						
						break;
					}
				case (ListMenu.Trombinoscope) :
					{
						break;
					}
				case (ListMenu.Notebook) :
					{
						break;
					}
			}
		}
		
	}
	private void GameUnpause()
	{
		switch (menu)
		{
			case (ListMenu.CloseMenu) :
				{
					
					break;
				}
			case (ListMenu.Trombinoscope) :
				{

					break;
				}
			case (ListMenu.Notebook) :
				{
					break;
				}
		}
	}
	
	private void GameDialog()
	{

	}
}

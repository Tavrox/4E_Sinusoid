using UnityEngine;
using System.Collections;

public class MenuButton : MonoBehaviour {
	
	public OTTextSprite label;
	public OTAnimatingSprite sprite;
	public enum ListAction
	{
		LaunchScene,
		MuteSound,
		LowerSound,
		RaiseSound,
		FunStuff, // To do funny miscellaneous stuff in menus :)
	}
	public ListAction action;

	void OnMouseOver()
	{
		if(Input.GetMouseButtonDown(0))
		{

		}
	}
}

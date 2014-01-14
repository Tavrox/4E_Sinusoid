using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SubUI : MonoBehaviour {

	public enum listSubUI
	{
		GameOver,
		Ingame,
		Pause
	};
	public listSubUI subuis;
	public Dictionary<string,GameObject> ContainedObject = new Dictionary<string, GameObject>();
	public bool anchoredToScreen;


	// Use this for initialization
	void Start () {
	
		switch (subuis)
		{
		case listSubUI.GameOver :
		{
			ContainedObject.Add("BackToMenu", FETool.findWithinChildren(this.gameObject, "BackToMenu"));
            ContainedObject.Add("GameOver", FETool.findWithinChildren(this.gameObject, "GameOver"));
            ContainedObject.Add("Retry", FETool.findWithinChildren(this.gameObject, "Retry"));
			break;
		}
		case listSubUI.Ingame:
		{
			ContainedObject.Add("Pause", FETool.findWithinChildren(this.gameObject, "Pause"));
			ContainedObject.Add("PebbleCounter", FETool.findWithinChildren(this.gameObject, "PebbleCounter"));
			ContainedObject.Add("Sidebar", FETool.findWithinChildren(this.gameObject, "Sidebar"));
			ContainedObject.Add("Tutorial", FETool.findWithinChildren(this.gameObject, "Tutorial"));
			break;
		}
		case listSubUI.Pause :
		{
			break;
		}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (anchoredToScreen)
		{
			FETool.anchorToObject(this.gameObject, Camera.main.gameObject, "xy");
		}
	}

	public void revealSub()
	{
		foreach (KeyValuePair<string, GameObject> _obj in ContainedObject)
		{
			_obj.Value.GetComponent<UIThing>().makeAppear();
		}
	}

	public void hideSub()
	{
		foreach (KeyValuePair<string, GameObject> _obj in ContainedObject)
		{
			_obj.Value.GetComponent<UIThing>().makeDisappear();
		}
	}
}

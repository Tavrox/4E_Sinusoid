using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuThing : MonoBehaviour {
	
	public enum ListMenu
	{
		Main,
		ChooseLevel,
		HighScores,
		EndLevel,
		Play,
		GoToCredits,
		Title,
		BackToMenu,
		BackLevel,
		GoToMainMenu,
		None,
		BackToMenuFromLevelChooser,
		GoLevel1,
		GoLevel2,
		GoLevel3,
		GoLevel4,
		GoLevel5
	};
	public ListMenu menu;
	private bool clickable = false;
	public bool animate = true;
	private OTSprite spr;
	private List<GameObject> menuObjects;
	public string _str = "Enter your name";
	
	// Use this for initialization
	void Start () {

		menuObjects = new List<GameObject>();

		if (GetComponentInChildren<OTSprite>() != null)
		{
			spr = GetComponentInChildren<OTSprite>();
		}

		switch (menu)	
		{
			case (ListMenu.Main):
			{
			
			break;
			}
			case (ListMenu.Play):
			{
			
			break;	
			}
			case (ListMenu.GoToMainMenu):
			{
			
			break;
			}
			case (ListMenu.GoToCredits):
			{
			
				break;
			}
			case (ListMenu.Title) :
			{
				MasterAudio.TriggerPlaylistClip("intro");
				MasterAudio.ResumeAllPlaylists();

				break;
			}
		}
		if (animate == true)
		{
			InvokeRepeating("animateItem", 0f, 2.5f);
		}
	
	}

	private void OnMouseDown()
	{
		menuObjects = new List<GameObject>();
		switch (menu)
		{
			case (ListMenu.Main):
			{
				
				break;
			}
			case (ListMenu.Play):
			{
				translateCamera(800f);
				menuObjects.Add(findObject("Title"));
				menuObjects.Add(findObject("Play"));
				menuObjects.Add(findObject("Credits"));
				fadeOutObjects(menuObjects);
				MasterAudio.PlaySound("door_open");
				menuObjects.Clear();
				break;
			}
			case (ListMenu.BackToMenu):
			{
				translateCamera(0f);
				menuObjects.Add(findObject("Title"));
				menuObjects.Add(findObject("Play"));
				menuObjects.Add(findObject("Credits"));
				fadeInObjects(menuObjects);
				MasterAudio.PlaySound("door_open");
				menuObjects.Clear();
				break;
			}
			case (ListMenu.GoToCredits):
			{
				menuObjects.Add(findObject("Title"));
				menuObjects.Add(findObject("Play"));
				menuObjects.Add(findObject("Credits"));
				fadeOutObjects(menuObjects);
				MasterAudio.PlaySound("door_open");
				menuObjects.Clear();
				translateCamera(-800f);
				break;
			}
			case (ListMenu.GoLevel1) :
			{
				MasterAudio.PlaySound("door_close");
				Application.LoadLevel(1);
				break;
			}
			case (ListMenu.GoLevel2) :
			{
				menuObjects.Add(findObject("Transition"));
				fadeInObjects(menuObjects);
				menuObjects.Clear();
				MasterAudio.PlaySound("door_open");
				StartCoroutine(loadLevel(2));
				break;
			}
		case (ListMenu.GoLevel3) :
		{
			menuObjects.Add(findObject("Transition"));
			fadeInObjects(menuObjects);
			menuObjects.Clear();
			MasterAudio.PlaySound("door_open");
			StartCoroutine(loadLevel(3));
			break;
		}
		}
	}
	
	private void triggerCredits()
	{

	}

	IEnumerator loadLevel(int _lvl)
	{
		yield return new WaitForSeconds(2f);
		Application.LoadLevel(_lvl);
	}

	private GameObject findObject(string _str)
	{
		GameObject result = GameObject.Find("Menu/" + _str);
		return result;
	}

	private void fadeOutObjects(List<GameObject> _objects)
	{
		foreach (GameObject _obj in _objects)
		{
			if (_obj != null)
			{
				OTSprite objectSprite = _obj.GetComponentInChildren<OTSprite>();
				_obj.collider.enabled = false;
				OTTween _tween = new OTTween(objectSprite,1f).Tween("alpha",0f);	
				if (objectSprite.alpha == 0)
				{
					_obj.SetActive(false);
				}
			}
		}
	}

	private void fadeInObjects(List<GameObject> _objects)
	{
		foreach (GameObject _obj in _objects)
		{
			if (_obj != null)
			{
				OTSprite objectSprite = _obj.GetComponentInChildren<OTSprite>();
				OTTween _tween = new OTTween(objectSprite,1f).Tween("alpha",1f);	
				if (objectSprite.alpha == 1)
				{
					_obj.SetActive(true);
				}
				_obj.collider.enabled = true;
			}
		}
	}

	private void translateCamera(float posX)
	{
		Transform _cam = GameObject.Find("UI/Main Camera").transform.transform;
		OTTween _tween = new OTTween(_cam,2f).Tween("position", new Vector3(posX, _cam.position.y, _cam.position.z), OTEasing.StrongOut );
	}

	private void goToMenu()
	{

	}

	private void animateItem()
	{
		int randRange = Random.Range(-30,30);
		OTTween _tween = new OTTween(spr,2.5f)
			.Tween("size", new Vector2(spr.size.x - randRange, spr.size.y - randRange) )
		.PingPong();
	}
	/*
	private void revealIntro(OTTween _tween)
	{
		OTTween _introOut = new OTTween(_intro, 2.5f).Tween("color", Color.white).Wait(2f);
		OTTween _titleOut = new OTTween(_title, 2.5f).Tween("alpha", 1f).Wait(2f);
		_titleOut.OnFinish(fadeEverything);
	}
	*/
}

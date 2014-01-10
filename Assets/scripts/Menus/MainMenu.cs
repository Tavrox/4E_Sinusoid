using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainMenu : MonoBehaviour {
	
	private List<GameObject> listMenus;

	// Use this for initialization
	void Start () {

		// listMenus = buildChildrenList();	
	}

	
	public void swapToMenu()
	{

	}

	private List<GameObject> buildChildrenList()
	{

		List<GameObject> childrenList = new List<GameObject>();
		/***** TO FIX ****
		GameObject[] listCompo = GetComponentsInChildren<GameObject>();
		foreach (GameObject _compo in listCompo)
		{

			if (_compo.tag != "Submenu")
			{
				childrenList.Add(_compo.gameObject);
				Debug.Log(_compo.gameObject.name);
			}
			Debug.Log(_compo.name);
		}
		*/
		return childrenList;
	}
}

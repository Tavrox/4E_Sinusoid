using UnityEngine;
using System.Collections;

public class FETool : MonoBehaviour {

	public static GameObject findWithinChildren(GameObject _go, string _fetch)
	{
		GameObject result = GameObject.Find(_go.name + "/" + _fetch);
		if (result == null)
		{Debug.LogWarning("The object "+  _fetch + " couldn't be found.");
			return new GameObject(_fetch + "NOTFOUND");
		}
		return result;
	}

	public static void anchorToObject(GameObject _obj1, GameObject _obj2, string _args = "xyz")
	{
		float initx = _obj1.gameObject.transform.position.x;
		float inity = _obj1.gameObject.transform.position.y;
		float initz = _obj1.gameObject.transform.position.z;
		
		float posx = _obj2.gameObject.transform.position.x;
		float posy = _obj2.gameObject.transform.position.y;
		float posz = _obj2.gameObject.transform.position.z;

		switch (_args)
		{
		case ("x"):
		{
			_obj1.transform.position = new Vector3(posx, inity, initz);
			break;
		}
		case ("y"):
		{
			_obj1.transform.position = new Vector3(initx, posy, initz);
			break;
		}
		case ("z"):
		{
			_obj1.transform.position = new Vector3(initx, inity, posz);
			break;
		}
		case ("xy"):
		{
			_obj1.transform.position = new Vector3(posx, posy, initz);
			break;
		}
		case ("xz"):
		{
			_obj1.transform.position = new Vector3(posx, inity, posz);
			break;
		}
		case ("yz"):
		{
			_obj1.transform.position = new Vector3(initx, posy, posz);
			break;
		}
		case ("xyz"):
		{
			_obj1.transform.position = new Vector3(posx, posy, posz);
			break;
		}
		}
	}


}

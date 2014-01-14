using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System;

public class ObjectImporter : MonoBehaviour {
	
	private int levelWidth;
	private int levelHeight;
	private int tileWidth;
	private int tileHeight;

	private string url;
	public TextAsset levelToLoad;
	private XmlDocument xmlDoc;
	private XmlNodeList mapNodes;
	private XmlNodeList tileNodes;
	private XmlNodeList tilesetNodes;
	private XmlNodeList itemNodes;
	private LevelManager _LevMan;

	 
	private enum tileList
	{
		Metal,
		Background
	};
	private tileList tileToGet;

	public enum objectList
	{
		Checkpoint,
		WalkerPoints,
		Rusher,
		LevelEntry,
		LevelExit,
		Ditch

	};
	private objectList obj;

	private List<GameObject> listTiles = new List<GameObject>();

	void Start()
	{
		activateTiles();
	}
	
	private void initXML()
	{
		xmlDoc = new XmlDocument();
		xmlDoc.LoadXml(levelToLoad.text);
		Debug.Log("Xml successfully loaded" + xmlDoc);
		initTilesets();
	}

	private void activateTiles()
	{
		if (listTiles != null)
		{
			foreach (GameObject _tiles in listTiles)
			{ 
				if (_tiles != null)
				{
					_tiles.SetActive(true);
				}
				else
				{
					listTiles.Remove(_tiles);
				}
			}
		}
		else 
		{
			print ("lists are null" + listTiles);
		}
	}

	#region TilesetImport

	private void initTilesets()
	{
		mapNodes = xmlDoc.SelectNodes("map");
		foreach(XmlNode node in mapNodes)
		{
			levelWidth = int.Parse(node.Attributes.GetNamedItem("width").Value);
			levelHeight = int.Parse(node.Attributes.GetNamedItem("height").Value);
			tileWidth = int.Parse(node.Attributes.GetNamedItem("tilewidth").Value);
			tileHeight = int.Parse(node.Attributes.GetNamedItem("tileheight").Value);
		}
		tilesetNodes = xmlDoc.SelectNodes("map/tileset");
		Debug.Log("Setupped Level [LW:"+levelWidth+"][LH:"+levelHeight+"]");
		Debug.Log("Setupped Tiles [TW:"+tileWidth+"][TH:"+tileHeight+"]");
	}

	private void clearTiles()
	{
		foreach(GameObject _tile in listTiles)
		{
			DestroyImmediate(_tile);
			listTiles.Remove(_tile);
		}
	}

	private void removeSpecificTiles()
	{
		foreach(GameObject _tile in listTiles)
		{
			if (_tile.name == "05(Clone)")
			{
				DestroyImmediate(_tile);
			}
		}
	}

	private void refreshTiles()
	{
		clearTiles();
		getTiles();

	}

	private void showParameters()
	{
		initXML();
		tilesetNodes = xmlDoc.SelectNodes("map/tileset");
		XmlNode tilesetParams = tilesetNodes.Item(1);
		Debug.Log (tilesetParams);
		Debug.Log (tilesetNodes);
	}

	private void getTiles()
	{
		Debug.Log("Start getting tiles");
		initXML();
		tileList _currTile = tileToGet;
		tileNodes = xmlDoc.SelectNodes("map/layer");
		tilesetNodes = xmlDoc.SelectNodes("map/tileset");
		XmlNode tilesetParams = tilesetNodes[1];
		int _currWidth = 0;
		int _currHeight = 0;
		int _currDepth = 0;
		int _width = 0;
		int _height = 0;
		string namePrefab = "";
		string nameCase = "";
		
		foreach (XmlNode node in tileNodes)
		{
			if (node.Attributes.GetNamedItem("name").Value == _currTile.ToString())
			{
				Debug.Log ("Enter : " + _currTile.ToString());
				GameObject _container = new GameObject(_currTile.ToString());
				_container.transform.parent = GameObject.Find("Level/TilesLayout").transform;

				foreach (XmlNode child in node.ChildNodes)
				{
					foreach (XmlNode children in child.ChildNodes)
					{
						int modVal = int.Parse(children.Attributes.GetNamedItem("gid").Value);
						string stringVal = "";
						if (modVal < 10)
						{ stringVal = "0" + modVal.ToString(); }
						else
						{ stringVal = modVal.ToString(); }
						namePrefab = "Tiles/" + _currTile.ToString() + "/" + stringVal;

						if (_currWidth >= levelWidth * tileWidth)
						{
							_currWidth = -0;
							_currHeight -= tileHeight;
						}
						_currWidth += tileWidth;
							
						if (Resources.Load(namePrefab) != null)
						{
							GameObject _instance = Instantiate(Resources.Load(namePrefab)) as GameObject;
							_instance.transform.parent = _container.transform ;
							_instance.transform.position = new Vector3 (_currWidth, _currHeight, _currDepth);
							listTiles.Add(_instance);
						}
						else
						{
							if (modVal != 0)
							{
								Debug.Log("The tile " + "[Tiles/" + _currTile.ToString() + stringVal + "] hasn't been found ! I quit.");
//								break;
							}
						}
					}
				}
			}
		}
		Debug.Log("Finish getting tiles");
	}
	#endregion

	
	[ContextMenu ("Get Objects")]
	private void getObjects()
	{
		Debug.Log("Start getting item");
		initXML();
		objectList _currObj = obj;
		itemNodes = xmlDoc.SelectNodes("map/objectgroup");
		foreach (XmlNode node in itemNodes)
		{
			foreach (XmlNode children in node.ChildNodes)
			{
				foreach (objectList _obj in Enum.GetValues(typeof(objectList)))
				{
					if (Resources.Load("Bricks/" + _obj.ToString()) != null)
					{
						if(children.Attributes.GetNamedItem("type") != null)
						{
							if (children.Attributes.GetNamedItem("type").Value == _obj.ToString())
							{
								GameObject _instance = Instantiate(Resources.Load("Bricks/" + children.Attributes.GetNamedItem("type").Value)) as GameObject;
								_instance.transform.parent = this.transform;
								float _posX = float.Parse(children.Attributes.GetNamedItem("x").Value) + 50;
								float _posY = float.Parse(children.Attributes.GetNamedItem("y").Value) - 50;
								if (children.Attributes.GetNamedItem("width") != null)
								{
									float _objWidth = float.Parse(children.Attributes.GetNamedItem("width").Value) / 2f;
									float _objHeight =  float.Parse(children.Attributes.GetNamedItem("height").Value) / 2f;
									_posX += _objWidth;
									_posY += _objHeight;
								}
								_instance.transform.position = new Vector3 (_posX / 50f, _posY / - 50f , -5f);
								if (children.Attributes.GetNamedItem("name").Value != null)
								{
									_instance.name = children.Attributes.GetNamedItem("name").Value;
								}
								Debug.Log("Created a " + children.Attributes.GetNamedItem("type").Value + " at position (X" + _instance.transform.position.x + "/Y" +_instance.transform.position.y+")");
							}
						}
					}
					else 
					{
						Debug.Log("Couldn't find the object at : " + "Bricks/Objects/" + _obj.ToString());
					}
				}
			}
		}
		Debug.Log("Finish getting item");
	}

	public void buildLevel()
	{
//		chosenVariation = Random.Range(minVariation, maxVariation);
		getObjects();
	}

	IEnumerator Wait(float waitTime)
	{
		yield return new WaitForSeconds(waitTime);
	}
}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System;

public class TileImporter : MonoBehaviour {
	
	private int levelWidth;
	private int levelHeight;
	private int tileWidth;
	private int tileHeight;

	private string url;
	public int levelID = 1;
	public string specificAddition = "";
	public bool specificTrigger = false;
	public int chosenVariation = 0;
	public int minVariation;
	public int maxVariation;
	private XmlDocument xmlDoc;
	private XmlNodeList mapNodes;
	private XmlNodeList tileNodes;
	private XmlNodeList tilesetNodes;
	private XmlNodeList itemNodes;

	 
	public enum tileList
	{
		Metal,
		Background
	};
	public tileList tileToGet;

	public enum objectList
	{
		Lever,
		Crate,
		Walker,
		Patroler,
		TurretU,
		TurretD,
		TurretL,
		TurretR,
		Key,
		TriggeredDoor,
		TimedBtn,
		SequencedBtn,
		TeleportIn,
		TeleportOut,
		TrapKill,
		Collibox,
		BaseArcU,
		BaseArcD,
		BaseArcL,
		BaseArcR,
		ArcHorizontal,
		ArcVertical,
		EndDoor,
	};
	public objectList obj;

	public List<GameObject> listTiles = new List<GameObject>();

	void Start()
	{
		activateTiles();
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
	
	private void initXML()
	{
		xmlDoc = new XmlDocument();
		string url = "file:///" + Application.dataPath + "/"+"maps/" + levelID.ToString() + ".xml";
		xmlDoc.Load(url);
		Debug.Log("Xml successfully loaded" + xmlDoc);
		initTilesets();
	}

	public void OnInspectorGUI()
	{
		if (GUILayout.Button("ClearMetal"))
		{
			Debug.Log("omg");
		}
	}

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

	[ContextMenu ("Clear Tiles")]
	private void clearTiles()
	{
		foreach(GameObject _tile in listTiles)
		{
			DestroyImmediate(_tile);
			listTiles.Remove(_tile);
		}
	}
	
	[ContextMenu ("Remove Specific Tiles")]
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

	[ContextMenu ("Refresh Tiles")]
	private void refreshTiles()
	{
		clearTiles();
		getTiles();

	}

	[ContextMenu ("Show Parameters")]
	private void showParameters()
	{
		initXML();
		tilesetNodes = xmlDoc.SelectNodes("map/tileset");
		XmlNode tilesetParams = tilesetNodes.Item(1);
		Debug.Log (tilesetParams);
		Debug.Log (tilesetNodes);
	}
	
	[ContextMenu ("Get Tiles")]
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

						if (specificTrigger == true)
						{
							namePrefab = "Tiles/" + _currTile.ToString() + "/" + specificAddition;
						}

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


	
	[ContextMenu ("Get Objects")]
	private void getObjects()
	{
		Debug.Log("Start getting item");
		initXML();
		objectList _currObj = obj;
		itemNodes = xmlDoc.SelectNodes("map/objectgroup");
		GameObject _parentVariation = new GameObject(chosenVariation.ToString());
		_parentVariation.transform.parent = GameObject.Find("Level/Gameplay").transform;
		foreach (XmlNode node in itemNodes)
		{
			if (node.Attributes.GetNamedItem("name").Value == chosenVariation.ToString())
			{
				foreach (XmlNode children in node.ChildNodes)
				{
					foreach (objectList _obj in Enum.GetValues(typeof(objectList)))
					{
						if (Resources.Load("Objects/" + _obj.ToString()) != null)
						{
							if(children.Attributes.GetNamedItem("type").Value != null)
							{
								if (children.Attributes.GetNamedItem("type").Value == _obj.ToString())
								{
									GameObject _instance = Instantiate(Resources.Load("Objects/" + children.Attributes.GetNamedItem("type").Value)) as GameObject;
									_instance.transform.position = new Vector3 (float.Parse(children.Attributes.GetNamedItem("x").Value) + 50, float.Parse(children.Attributes.GetNamedItem("y").Value) * -1, -5f);
									if (children.Attributes.GetNamedItem("name").Value != null)
									{
										_instance.name = children.Attributes.GetNamedItem("name").Value;
									}
									_instance.transform.parent = _parentVariation.transform;
									if (_obj.ToString() == objectList.Collibox.ToString())
									{
										print ("boxcollider enabled");
										float width = float.Parse(children.Attributes.GetNamedItem("width").Value) ;
										float height = float.Parse(children.Attributes.GetNamedItem("height").Value);
										_instance.GetComponent<BoxCollider>().size = new Vector3 ( width, height, 100f); 
										_instance.transform.position =
											new Vector3 (
											(float.Parse(children.Attributes.GetNamedItem("x").Value)) + width / 2f,
											(float.Parse(children.Attributes.GetNamedItem("y").Value) * -1) - height / 2f,
											-5f);

									}
									Debug.Log("Created a " + children.Attributes.GetNamedItem("type").Value + " at position (X" + _instance.transform.position.x + "/Y" +_instance.transform.position.y+")");
								}
								else
								{
		//							print ("didnt find object");
								}
							}
						}
						else 
						{
							Debug.Log("The object " + _obj.ToString() + " hasn't been found ! Fix that error NOW.");
						}
					}
				}
			}
			else
			{
				print (node.Attributes.GetNamedItem("name").Value + "not picked");
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
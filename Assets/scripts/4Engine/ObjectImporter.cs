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
	public LevelBrick specificBrickToImport;
	 
	private enum tileList
	{
		Metal,
		Background
	};
	private tileList tileToGet;
	
	public List<string> ListLayers = new List<string>();
	public List<string> ListObjects = new List<string>();
	public Dictionary<LevelBrick, GameObject> ExportedObjects = new Dictionary<LevelBrick, GameObject>();
	
	private void initXML()
	{
		xmlDoc = new XmlDocument();
		xmlDoc.LoadXml(levelToLoad.text);
		Debug.Log("Xml successfully loaded" + xmlDoc);
		initTilesets();
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


	private void setupObjects()
	{
		initXML();
		itemNodes = xmlDoc.SelectNodes("map/objectgroup");

		ListObjects.Clear();
		ListLayers.Clear();

		ListLayers.Add("LevelBricks");


		foreach (LevelBrick.brickEnum _brick in Enum.GetValues(typeof(LevelBrick.brickEnum)))
		{
			ListObjects.Add(_brick.ToString());
		}

		foreach (string _names in ListLayers)
		{
			GameObject _go = new GameObject(_names.ToString());
			_go.transform.parent = this.gameObject.transform;
		}
	}
	
	[ContextMenu ("Get Objects")]
	private void getObjects()
	{
		Debug.Log("Start getting item");
		clearObjects();
		setupObjects();
		initXML();
		itemNodes = xmlDoc.SelectNodes("map/objectgroup");
		string path = "Prefabs/03Level/";
		Vector2 offset = new Vector2 (6f ,-1f);
		foreach (XmlNode node in itemNodes)
		{
			GameObject objectContainer = FETool.findWithinChildren(this.gameObject, node.Attributes.GetNamedItem("name").Value);
			foreach (XmlNode children in node.ChildNodes)
			{
				if(children.Attributes.GetNamedItem("type") != null)
				{
					Debug.Log("step1");
					string childType = children.Attributes.GetNamedItem("type").Value;
					if (Resources.Load(path + childType) != null)
					{
						GameObject _instance = Instantiate(Resources.Load(path + childType)) as GameObject;
						if (objectContainer.name != "BUG")
						{ _instance.transform.parent = objectContainer.transform;}
						float _posX = float.Parse(children.Attributes.GetNamedItem("x").Value) / 50f;
						float _posY = (float.Parse(children.Attributes.GetNamedItem("y").Value) / 50f) *-1f;
						if (children.Attributes.GetNamedItem("width") != null)
						{
							if (childType == LevelBrick.brickEnum.Ditch.ToString())
							{
								float _objWidth = float.Parse(children.Attributes.GetNamedItem("width").Value);
								float _objHeight =  float.Parse(children.Attributes.GetNamedItem("height").Value);
								_instance.transform.localScale = new Vector3(_objWidth / 50f, _objHeight / 50f, 30f);
							}
						}
						if (children.Attributes.GetNamedItem("name") != null)
						{
							_instance.name = children.Attributes.GetNamedItem("name").Value;
						}
						_instance.transform.position = new Vector3 (_posX + offset.x, _posY + offset.y , -5f);
						Debug.Log("Created a " + children.Attributes.GetNamedItem("type").Value + " at position (X" + _instance.transform.position.x + "/Y" +_instance.transform.position.y+")");
					}
					else 
					{
						Debug.Log("Couldn't find prefab "+ children.Attributes.GetNamedItem("type").Value);
					}
				}
			}
		}
		Debug.Log("Finish getting item");
	}

	[ContextMenu ("Get Specific Brick")]
	private void getSpecificBrick()
	{
		Debug.Log("Start getting item");
		clearObjects();
		setupObjects();
		initXML();
		itemNodes = xmlDoc.SelectNodes("map/objectgroup");
		string path = "Prefabs/03Level/";
		foreach (XmlNode node in itemNodes)
		{
			GameObject objectContainer = FETool.findWithinChildren(this.gameObject, node.Attributes.GetNamedItem("name").Value);
			foreach (XmlNode children in node.ChildNodes)
			{
				if(children.Attributes.GetNamedItem("type") != null)
				{
					if (children.Attributes.GetNamedItem("type").Value == specificBrickToImport.ToString())
					{
						string childType = children.Attributes.GetNamedItem("type").Value;
						if (Resources.Load(path + childType) != null)
						{
							GameObject _instance = Instantiate(Resources.Load(path + childType)) as GameObject;
							_instance.transform.parent = objectContainer.transform;
							float _posX = float.Parse(children.Attributes.GetNamedItem("x").Value) + 50;
							float _posY = float.Parse(children.Attributes.GetNamedItem("y").Value) - 50;
							if (children.Attributes.GetNamedItem("width") != null)
							{
								float _objWidth = float.Parse(children.Attributes.GetNamedItem("width").Value) / 2f;
								float _objHeight =  float.Parse(children.Attributes.GetNamedItem("height").Value) / 2f;
								_posX += _objWidth;
								_posY += _objHeight;
							}
							_instance.transform.position = new Vector3 (_posX / 50f, _posY / - 51f , -5f);
							if (children.Attributes.GetNamedItem("name") != null)
							{
								_instance.name = children.Attributes.GetNamedItem("name").Value;
							}
							Debug.Log("Created a " + children.Attributes.GetNamedItem("type").Value + " at position (X" + _instance.transform.position.x + "/Y" +_instance.transform.position.y+")");
						}
					}
					else 
					{
						Debug.Log("Couldn't find prefab "+ children.Attributes.GetNamedItem("type").Value);
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

	private void clearObjects()
	{
		foreach (string _str in ListLayers)
		{
			GameObject _go = FETool.findWithinChildren(this.gameObject, _str.ToString());
			DestroyImmediate(_go);
		}
	}

	IEnumerator Wait(float waitTime)
	{
		yield return new WaitForSeconds(waitTime);
	}
}
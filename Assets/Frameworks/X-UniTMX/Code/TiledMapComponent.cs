/*!
 * X-UniTMX: A tiled map editor file importer for Unity3d
 * https://bitbucket.org/Chaoseiro/x-unitmx
 * 
 * Copyright 2013 Guilherme "Chaoseiro" Maia
 * Released under the MIT license
 * Check LICENSE.MIT for more details.
 */
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Collections;
using X_UniTMX;
using System.Xml;

[AddComponentMenu("Tiled Map/Tiled Map Component")]
public class TiledMapComponent : MonoBehaviour {
	public TextAsset MapTMX;
	public bool GenerateCollider = false;
	public float[] CollidersZDepth;
	public float[] CollidersWidth = new float[] {50f};
	public string[] CollidersLayerName;
	public bool[] CollidersIsInner;
	public bool MakeUniqueTiles = true;
	private Map tiledMap;

	public Map TiledMap
	{
		get { return tiledMap; }
		set { tiledMap = value; }
	}

	// Use this for initialization
	public void Initialize (string fullPath, string mapPath) {
		// Loads tile map
		XmlDocument document = new XmlDocument();
		document.LoadXml(MapTMX.text);
		tiledMap = new Map(document, MakeUniqueTiles, fullPath, mapPath, this.gameObject);//, MeshRendererPrefab);
		CollidersWidth[0] = 50f;
		CollidersWidth[1] = 50f;
		CollidersWidth[2] = 50f;
		CollidersWidth[3] = 50f;
		CollidersLayerName[0] = "GroundColl";
		CollidersLayerName[1] = "WoodColl";
		CollidersLayerName[2] = "DitchColl";
		CollidersLayerName[3] = "GrabColl";
	}

	public void GenerateColliders()
	{
		for (int i = 0; i < CollidersLayerName.Length; i++)
		{
			GameObject _parent = new GameObject(CollidersLayerName[i]);
			_parent.transform.parent = gameObject.transform;
			MapObjectLayer collisionLayer = (MapObjectLayer)tiledMap.GetLayer(CollidersLayerName[i]);
			if (collisionLayer != null)
			{
				List<MapObject> colliders = collisionLayer.Objects;
				foreach (MapObject collider in colliders)
				{
					switch (collider.MapObjectType)
					{
					case MapObjectType.Box :
					{
						switch(collisionLayer.Name)
						{
						case ("GroundColl") :
						{
							GameObject result = tiledMap.GenerateBoxCollider(collider, CollidersZDepth[i], CollidersWidth[i]);
							result.transform.parent = _parent.transform;
							GameObject pebResult = tiledMap.GeneratePebbleCollider(collider, CollidersZDepth[i], CollidersWidth[i]);
							pebResult.transform.parent = _parent.transform;
							break;
						}
						default :
						{
							GameObject result = tiledMap.GenerateSpecialCollider(collider, collisionLayer.Name, CollidersZDepth[i], CollidersWidth[i]);
							affectColliders( result, _parent);
							break;
						}
						}
						break;
					}
					case MapObjectType.Ellipse:
					{
						tiledMap.GenerateEllipseCollider(collider, CollidersZDepth[i], CollidersWidth[i]);
						break;
					}	
					case MapObjectType.Polygon:
					{
						GameObject res1 = tiledMap.GeneratePolygonCollider(collider, CollidersZDepth[i], CollidersWidth[i], CollidersIsInner[i]);
						GameObject res2 = tiledMap.GeneratePolygonPebble(collider, CollidersZDepth[i], CollidersWidth[i], CollidersIsInner[i]);
						break;
					}
					case MapObjectType.Polyline:
					{
						tiledMap.GeneratePolylineCollider(collider, CollidersZDepth[i], CollidersWidth[i], CollidersIsInner[i]);
						break;
					}
					}
				}
			}
			else
			{
				Debug.LogError("There's no Layer \"" + CollidersLayerName[i] + "\" in tile map.");
			}
		}
		
	}

	public void affectColliders(GameObject _gameo, GameObject _parent)
	{
		_gameo.transform.parent = _parent.transform;
	}
}

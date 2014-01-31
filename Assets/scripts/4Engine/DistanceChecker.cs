using UnityEngine;
using System.Collections;

public class DistanceChecker : MonoBehaviour {

	private float distanceToPlayer = 0f;
	public float distanceForFadeOut = 15f;
	private Transform referralDistance;
	private Transform distToTrack;
	private LevelBrick _brick;
	public LevelBrick.brickEnum brickType;
	public bool brickActivated;
	private bool brickOccupied;

	// Use this for initialization
	void Start () 
	{
		referralDistance = gameObject.transform;
		distToTrack = GameObject.FindGameObjectWithTag("Player").transform;
		_brick = GameObject.Find("Level/ObjectsImporter/Pool/"+ brickType.ToString()).GetComponent<LevelBrick>();
		if (_brick == null)
		{
			Debug.LogError("An object hasn't been found" + "Level/ObjectsImporter/Pool/"+ brickType.ToString() );
		}
		InvokeRepeating("checkDistance", 0f, 1f);
	}

	private void checkDistance()
	{		
		Vector2 thisObjPos = new Vector2 (gameObject.transform.position.x, gameObject.transform.position.y);
		Vector2 posToTrack = new Vector2 (distToTrack.position.x, distToTrack.position.y);
		distanceToPlayer = Vector2.Distance(thisObjPos, posToTrack);
//		print (gameObject.name + gameObject.transform.position + distanceToPlayer );
		if (distanceToPlayer > distanceForFadeOut)
		{
			disableThings();
		}
		else 
		{
			enableThings();
		}
	}

	private void disableThings()
	{
		if (_brick != null && _brick.isOccupied != true)
		{
			_brick.isOccupied  = false;
			_brick.gameObject.SetActive(false);
		}
	}
	private void enableThings()
	{
		if (_brick != null)
		{
			_brick.gameObject.SetActive(true);
			_brick.gameObject.transform.position = gameObject.transform.position;
			_brick.isOccupied = true;
			if (brickType == LevelBrick.brickEnum.Turret)
			{
				_brick.GetComponent<Turret>().resetTurret();
			}
		}
	}
}

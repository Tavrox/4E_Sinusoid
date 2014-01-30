using UnityEngine;
using System.Collections;

public class LevelBrick : MonoBehaviour {

	public enum brickEnum
	{
		Turret,
		Crate,
		WalkerPoints,
		Rusher,
		Drop,
		Ditch,
		LevelEntry,
		LevelExit,
		Checkpoint
	};
	public brickEnum brickType;
	private float distanceToPlayer = 0f;
	public float distanceForFadeOut = 5f;
	private Transform referralDistance;
	private Transform distToTrack;
	private bool hasDiedOnce;

	// Use this for initialization
	void Start () 
	{	
		InvokeRepeating("checkDistance", 0f, 0.1f); 
	}

	private void checkDistance()
	{
		//		Debug.DrawLine(gameObject.transform.position, distToTrack.position, Color.blue);
		//		Debug.DrawLine(gameObject.transform.position, referralDistance.position, Color.black);
		// WORK IN PROGRESS
		//		distanceToPlayer = (Vector2.Distance(thisObjPos, referralPos)) / (Vector2.Distance(thisObjPos, posToTrack )) ;
		//		distanceToPlayer = (Vector2.Distance(thisObjPos, posToTrack )) / (Vector2.Distance(thisObjPos, referralPos)) ;
		//		Debug.Log(gameObject.name + thisObjPos);
		//		Debug.Log(referralDistance.name + referralPos);
		//		Debug.Log("PosToTrack" + posToTrack);
		//		Debug.Log("ObjPos>>ReferralPos" + Vector2.Distance(thisObjPos, referralPos));
		//		Debug.Log("ObjPos>>Trackpos" + Vector2.Distance(thisObjPos, posToTrack));
		//		Debug.Log("Ratio" + distanceToPlayer);

		Vector2 thisObjPos = new Vector2 (gameObject.transform.position.x, gameObject.transform.position.y);
		Vector2 referralPos = new Vector2 (referralDistance.transform.position.x, referralDistance.transform.position.y);
		Vector2 posToTrack = new Vector2 (distToTrack.position.x, distToTrack.position.y);
		distanceToPlayer = Vector2.Distance(thisObjPos, posToTrack );
		if (distanceToPlayer < 15f)
		{

		}
		else
		{

		}
	}

}

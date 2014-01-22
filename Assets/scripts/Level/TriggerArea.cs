using UnityEngine;
using System.Collections;

public class TriggerArea : MonoBehaviour {

	private LevelBrick linkedObject;

	// Use this for initialization
	void Start () {

		linkedObject = gameObject.transform.parent.gameObject.GetComponent<LevelBrick>();
	
	}

	void OnTriggerStay(Collider other)
	{
		if (linkedObject.brickType == LevelBrick.brickEnum.Turret && other.CompareTag("Player") == true)
		{
			linkedObject.GetComponent<Turret>().changeTarget(other.gameObject);
		}
	}
}

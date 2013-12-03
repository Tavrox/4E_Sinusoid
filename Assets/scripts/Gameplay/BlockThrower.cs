using UnityEngine;
using System;

public class BlockThrower : MonoBehaviour {

	
	public float angleMax = 180, numberOfObjects=10;
	public int offset = 0;
	public Blob prefabBlob;
	//public Vector3 startPosition;

	private float angleUnitaire, rayon;
	private int nbObjectsToAdd;
	private Blob instanceBlob;
	//private Vector3 nextPosition;

	public void createCircle () {
		//nextPosition = startPosition;
		angleUnitaire = (angleMax / numberOfObjects);
		nbObjectsToAdd = offset / Convert.ToInt32(angleUnitaire);

		if(angleMax != 360) numberOfObjects++;
		
		for(int i = (0+nbObjectsToAdd); i < numberOfObjects+nbObjectsToAdd; i++) {
			instanceBlob = Instantiate(prefabBlob) as Blob;

			//instanceBlob.setPosition(new Vector3(50f,50f,50f));
			//			instanceBlob.setX(player.transform.position.x);
			//			instanceBlob.setY(player.transform.position.y);
			instanceBlob.setCos(Mathf.Cos((angleUnitaire*i)*Mathf.Deg2Rad));
			instanceBlob.setSin(Mathf.Sin((angleUnitaire*i)*Mathf.Deg2Rad));
		}

		if(angleMax != 360) numberOfObjects--;
	}
}

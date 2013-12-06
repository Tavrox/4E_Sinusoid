using UnityEngine;
using System;
using System.Collections.Generic;

public class WavesCreator : MonoBehaviour {
	
	
	public float angleMax = 180, numberOfObjects=10;
	public int offset = 0;
	public WaveElt prefabWaveElt;
	//public Vector3 startPosition;
	
	private float angleUnitaire, rayon;
	private int nbObjectsToAdd;
	private WaveElt instanceWaveElt;
	public List<WaveElt> lights = new List<WaveElt>();
	//private Vector3 nextPosition;
	
	public void createCircle () {
		//nextPosition = startPosition;
		//print ("AAAAAAAAAAAAA***///-*/-*/"+offset);
		angleUnitaire = (angleMax / numberOfObjects);
		nbObjectsToAdd = offset / Convert.ToInt32(angleUnitaire);
		//print ("BBBBBBBBBBBBB***///-*/-*/"+offset);
		if(angleMax != 360) numberOfObjects++;
		
		for(int i = (0+nbObjectsToAdd); i < numberOfObjects+nbObjectsToAdd; i++) {
			instanceWaveElt = Instantiate(prefabWaveElt) as WaveElt;
			
			//instanceWaveElt.setPosition(new Vector3(50f,50f,50f));
			//			instanceWaveElt.setX(player.transform.position.x);
			//			instanceWaveElt.setY(player.transform.position.y);
			instanceWaveElt.setCos(Mathf.Cos((angleUnitaire*i)*Mathf.Deg2Rad));
			instanceWaveElt.setSin(Mathf.Sin((angleUnitaire*i)*Mathf.Deg2Rad));
			lights.Add(instanceWaveElt);
			//instanceWaveElt.endLife();
		}
		
		if(angleMax != 360) numberOfObjects--;
	}
	public void specialCircle () {
		foreach (WaveElt light in lights) {
			//print ("a");
			light.setSpecial();
		}
	}
	public void resetCircle () {
		foreach (WaveElt light in lights) {
			//print ("a");
			light.startLife();
		}
	}
	public void destroyCircle () {
		foreach (WaveElt light in lights) {
			Destroy(light.gameObject);
		}
		Destroy(gameObject);
	}
	
	public void circleWalkToSprint () {
		foreach (WaveElt light in lights) {
			light.setSprintState();
		}
	}
	public void circleSprintToWalk () {
		foreach (WaveElt light in lights) {
			light.setWalkState();
		}
	}
	public float getLifeTime() {
		return lights[0].lifeTime;
	}
	public float getAlpha() {
		return lights[0].getAlpha();
	}
}

using UnityEngine;
using System;
using System.Collections.Generic;

public class WaveCreator : MonoBehaviour {
	
	
	public float angleMax = 180, numberOfObjects=10;
	public int offset = 0;
	//public WaveElt prefabWaveElt;
	private GameObject GOwaveElt;
	//public Vector3 startPosition;
	
	private float angleUnitaire, rayon;
	private bool testAlive;
	private int nbObjectsToAdd;
	private WaveElt instanceWaveElt;
	public List<WaveElt> lights = new List<WaveElt>();
	//private Vector3 nextPosition;
	
	public void createCircle (Transform caller) {
		//nextPosition = startPosition;
		//print ("AAAAAAAAAAAAA***///-*/-*/"+offset); 
		angleUnitaire = (angleMax / numberOfObjects);
		nbObjectsToAdd = offset / Convert.ToInt32(angleUnitaire);
		//print ("BBBBBBBBBBBBB***///-*/-*/"+offset);
		if(angleMax != 360) numberOfObjects++;
		
		for(int i = (0+nbObjectsToAdd); i < numberOfObjects+nbObjectsToAdd; i++) {
			//instanceWaveElt = Instantiate(prefabWaveElt) as WaveElt;
			//GOwaveElt = Instantiate(Resources.Load("Prefabs/04Gameplay/SoundWaveEltProj")) as GameObject;
			GOwaveElt = Instantiate(Resources.Load("Prefabs/04Gameplay/SoundWaveElt")) as GameObject;
			GOwaveElt.gameObject.transform.parent = GameObject.Find("Level/Waves/"+gameObject.name).transform;
			instanceWaveElt = GOwaveElt.GetComponentInChildren<WaveElt>();
			//instanceWaveElt.setPosition(new Vector3(50f,50f,50f));
			//			instanceWaveElt.setX(player.transform.position.x);
			//			instanceWaveElt.setY(player.transform.position.y);
			instanceWaveElt.setCos(Mathf.Cos((angleUnitaire*i)*Mathf.Deg2Rad));
			instanceWaveElt.setSin(Mathf.Sin((angleUnitaire*i)*Mathf.Deg2Rad));
			instanceWaveElt.setCallerObject(caller);
			lights.Add(instanceWaveElt);
			//instanceWaveElt.endLife(); 
		}
		
		if(angleMax != 360) numberOfObjects--;
	}
	public void setParent(Transform caller) {
		foreach (WaveElt light in lights) {
			light.setCallerObject(caller);
		}
	}
	public void specialCircle () {
		foreach (WaveElt light in lights) {
			//print ("a");
			light.setSpecial();
		}
	}
	public void resetCircle (float parentScaleX = 0, bool parentDirLeft = false, bool isCharacter = false) {
		foreach (WaveElt light in lights) {
			if(isCharacter)	light.setCharacterPositionOffset(parentScaleX,parentDirLeft);
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
		//print ("setSPRINT");
		foreach (WaveElt light in lights) {
			light.setSprintState();
		}
	}
	public void circleSprintToWalk () {
		//print ("setWALK");
		foreach (WaveElt light in lights) {
			light.setWalkState();
		}
	}
	public void circleGroundedToFall () {
		//print ("setFALL");
		foreach (WaveElt light in lights) {
			light.setFallState();
		}
	}
	public void circleFallToGrounded () {
		//print ("setGROUND");
		foreach (WaveElt light in lights) {
			light.setGroundedState();
		}
	}
	public float getLifeTime() {
		return lights[0].lifeTime;
	}
	public float getAlpha() {
		return lights[0].getAlpha();
	}
	public void setAlpha() {
		foreach (WaveElt light in lights) {
			light.setAlpha(0f);
		}
	}
	public void addSpeed(float multiplier) {
		foreach (WaveElt light in lights) {
			light.setSpeed(light.speedSound+multiplier);
		}
	}
	public bool isActive() {
		testAlive = false;
		foreach (WaveElt light in lights) {
			if(light.alive == true) {
				testAlive = true;
				break;
			}
		}
		return testAlive;
	}
	public void setCharacterMoveOffset (float offsetValue) {
		foreach (WaveElt light in lights) {
			light.setCharacterMoveOffset (offsetValue);
		}
	}
}

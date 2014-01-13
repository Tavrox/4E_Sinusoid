using UnityEngine;
using System.Collections;

public class Enemy : Character {
	
	protected Transform target; //the enemy's target
	protected bool chasingPlayer, endChasingPlayer, patroling;
	
	public void setTarget(Transform obj) {
		target = obj;
	}
	
	public void activeChasing() {
		chasingPlayer = true;
		patroling = false;
	}
	public void stopChasing() {
		chasingPlayer = false;
		endChasingPlayer = true;
	}
	public void targetReached() {
		setTarget(GameObject.FindWithTag("Player").transform);
		stopChasing();
	}
}

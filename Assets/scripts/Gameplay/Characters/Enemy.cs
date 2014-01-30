using UnityEngine;
using System.Collections;

public class Enemy : Character {
	
	public Transform target; //the enemy's target
	protected bool chasingPlayer, endChasingPlayer, patroling, endPFReached, attacking;
	
	public void setTarget(Transform obj) {
		target = obj;
	}
	public Transform getTarget() {
		return target;
	}
	public bool getChasingPlayer() {
		return chasingPlayer;
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
		setTarget(transform);
		stopChasing();
	}
	public bool getEndPFReached () {
		return endPFReached;
	}
	public bool getAttacking () {
		return attacking;
	}
}

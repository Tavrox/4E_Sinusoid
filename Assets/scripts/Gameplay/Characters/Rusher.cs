﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Rusher : Enemy {
	
	[HideInInspector] public Vector3 position;
	[HideInInspector] public Transform trans;

	/***** ENNEMI BEGIN *****/

	private Vector3 direction;
	
	public float targetDetectionArea = 3f;
	private float blockDetectionArea = 2f;
	
	private RaycastHit hitInfo; //infos de collision
	private Ray detectTargetLeft, detectTargetRight; //point de départ, direction
	
	private bool goLeft = true, defineRushDirection;

	//private int waypointId = 0;
	//public Transform[] waypoints;
	//public List<Transform> waypoints= new List<Transform>(), endChaseArea= new List<Transform>();
	//public float[] timePauseWP;
	//private float timeToWait=0;
	//public Pebble instPebble;
	//public WaveCreator instFootWave,instInstruWave;
	//public GameObject instPebbleBar;
	//private GameObject pebbleBar;

	private GameObject GOinstFootWave, GOinstInstruWave;
	public float footStepDelay = 0.6f;
	
	[SerializeField] private Rect hp_display;
	private WaveCreator soundEmitt1, soundEmitt2, soundInstru1/*, soundInstru2,soundEmitt3*/;
	private int cptWave=1, pebbleDirection = 1;
	private bool blockCoroutine, first, toSprint, toWalk, specialCast, playerDirLeft, waypointReached;
	private Pebble pebble1;
	private float powerPebble;
	private float waypointDetectionWidth;
	public LayerMask projectorMask;
	
	[HideInInspector] public bool paused = false;
	
	public FESound AttackSound;
	public FESound IdleSound;
	public FESound RunSound;
	public FESound StartSound;
	public FESound StopSound;
	
	// Use this for initialization
	public override void Start () 
	{
		base.Start();
		
		GameEventManager.GameStart += GameStart;
		GameEventManager.GameOver += GameOver;
		GameEventManager.GamePause += GamePause;
		GameEventManager.GameUnpause += GameUnpause;

		spawnPos = transform.position;

		GOinstFootWave = Instantiate(Resources.Load("Prefabs/04Gameplay/SoundWavesEmitter")) as GameObject;
		soundEmitt1 = GOinstFootWave.GetComponent<WaveCreator>();soundEmitt1.gameObject.name = "_footWaveRusher1";//footsteps wave 1
		GOinstFootWave = Instantiate(Resources.Load("Prefabs/04Gameplay/SoundWavesEmitter")) as GameObject;
		soundEmitt2 = GOinstFootWave.GetComponent<WaveCreator>();soundEmitt2.gameObject.name = "_footWaveRusher2";//footsteps wave 2
		GOinstInstruWave = Instantiate(Resources.Load("Prefabs/04Gameplay/SoundWavesInstru")) as GameObject;
		soundInstru1 = GOinstInstruWave.GetComponent<WaveCreator>();soundInstru1.gameObject.name = "_instruWaveRusher1"; //intru wave 1
		
		soundEmitt1.gameObject.transform.parent = soundEmitt2.gameObject.transform.parent = soundInstru1.gameObject.transform.parent = GameObject.Find("Level/Waves/").transform;
//		soundEmitt1 = Instantiate(instFootWave) as WaveCreator;
//		soundEmitt2 = Instantiate(instFootWave) as WaveCreator;
//		soundInstru1 = Instantiate(instInstruWave) as WaveCreator;
		soundEmitt1.createCircle(thisTransform);soundEmitt1.setParent(thisTransform);
		soundEmitt2.createCircle(thisTransform);soundEmitt2.setParent(thisTransform);
		soundInstru1.createCircle(thisTransform);soundInstru1.specialCircle();soundInstru1.setParent(thisTransform);

		//enabled = false;

		runSpeed = 0.5f;

		IdleSound.playDistancedSound("idle");
		
		setTarget(transform); //target reset
		patroling = true;
		waypointDetectionWidth = thisTransform.gameObject.GetComponentInChildren<Transform>().GetComponentInChildren<OTSprite>().transform.localScale.x/4;//transform.localScale.x/4;
	}
	private void setIniState() {
		thisTransform.position = spawnPos;
		cptWave = 1;
		goLeft = true;
		//waypointId = 0;
		//timeToWait=0;
		pebbleDirection = 1;
		defineRushDirection = chasingPlayer = false;
		blockCoroutine = false;
		StopCoroutine("waitB4FootStep");StopCoroutine("footStep");
		isLeft = isRight = isJump = isGoDown = isCrounch = false;
		movingDir = moving.None;
	}
	// Update is called once per frame
	public void Update () 
	{
//		isLeft = false;
//		isRight = false;
//		isJump = false;
//		isShot = false;
//		isPass = false;
//		movingDir = moving.None;
//
//		if(!endChasingPlayer) {
//			if(chasingPlayer) {ChasePlayer();}
//			else if(patroling) {Patrol();}
//		}
//		else {
//			goBackToPatrol();
//		}
//		checkInput();
		//GameObject _instance = Instantiate(Resources.Load(namePrefab)) as GameObject;
		if(!grounded) UpdateMovement();
		offsetCircles ();
		detectPlayer();
		if(chasingPlayer && target!=null && !blockedLeft && !blockedRight) {ChasePlayer();}
		//detectEndChaseArea();
		//detectEndPlatform();

		if(isLeft || isRight) {
			//print("left&right");
			if(!blockCoroutine && grounded) StartCoroutine("waitB4FootStep");
		}
	}

	/************************
	 *						*
	 *  DETECTION RAYCASTS 	*
	 *						*
	 ***********************/
	private void detectPlayer() {
		detectTargetLeft = new Ray(thisTransform.position, Vector3.left);
		detectTargetRight = new Ray(thisTransform.position, Vector3.right);
		Debug.DrawRay(thisTransform.position, Vector3.left*targetDetectionArea);
		Debug.DrawRay(thisTransform.position, Vector3.right*targetDetectionArea);
		if (Physics.Raycast(detectTargetLeft, out hitInfo, targetDetectionArea, projectorMask) || Physics.Raycast(detectTargetRight, out hitInfo, targetDetectionArea, projectorMask)) {
			if(hitInfo.collider.name == "Player" && !endChasingPlayer) {
				setTarget(transform); //target reset
				activeChasing();
				StartSound.playVariationSound("attack");

			}
		}
	}

	/************************
	 *						*
	 *    IA MANAGEMENT		*
	 *						*
	 ***********************/
	private void ChasePlayer () {
		if(!defineRushDirection) {
			defineRushDirection = true;
			if (thisTransform.position.x > target.position.x) {
				isLeft = true;
				isRight = false;
				facingDir = facing.Left;
				UpdateMovement();
			}
			else if (thisTransform.position.x < target.position.x) {
				//direction = Vector3.right;
				isRight = true; 
				isLeft = false;
				facingDir = facing.Right;
				UpdateMovement();
			}
			else {
				isLeft = isRight = false;
				defineRushDirection = false;
				targetReached();
			}
		}
		else {
			UpdateMovement();
			if ((isLeft && target.position.x > thisTransform.position.x-waypointDetectionWidth) || (isRight && target.position.x < thisTransform.position.x+waypointDetectionWidth)) {
				isLeft = isRight = false;
				defineRushDirection = false;
				targetReached();
			}
		}
	}

	/************************
	 *						*
	 *  WAVES MANAGEMENT	*
	 *						*
	 ***********************/

	/* ---- FOOSTEPS ---- */
	IEnumerator waitB4FootStep()
	{
		yield return new WaitForSeconds(0.1f);
		if(!blockCoroutine && grounded) StartCoroutine("footStep");
	}
	IEnumerator footStep()
	{
		blockCoroutine =true;
		
		/*if(Input.GetKeyDown("left shift")) {
//			moveVel = 2 * moveVel;
//			footStepDelay = footStepDelay / 2;
			soundEmitt1.circleWalkToSprint();
			soundEmitt2.circleWalkToSprint();
		}
		if(Input.GetKeyUp("left shift")) {
//			moveVel = moveVel / 2;
//			footStepDelay = footStepDelay * 2;
			soundEmitt1.circleSprintToWalk();
			soundEmitt2.circleSprintToWalk();
		}*/
		playerDirLeft = (facingDir == facing.Right) ? false : true;
		if(cptWave == 1) {cptWave++;soundEmitt1.resetCircle(transform.localScale.x/1.5f,playerDirLeft, true);}
		else if (cptWave == 2) {cptWave=1;soundEmitt2.resetCircle(transform.localScale.x/1.5f,playerDirLeft, true);}
		yield return new WaitForSeconds(footStepDelay);
		
		blockCoroutine = false;
	}

	/* ---- SPECIAL CIRCLE ---- */
	IEnumerator specialCircleCast()
	{
		specialCast = true;
		yield return new WaitForSeconds(1f);
		
		if(first) {first=first;soundInstru1.resetCircle();}
		//else {first=!first;soundInstru2.resetCircle();}
		//yield return new WaitForSeconds(soundInstru1.getLifeTime());
		specialCast = false;
	}
	
	/* ---- PEBBLE ---- */
//	private void setPebbleBarPos() {
//		pebbleBar.transform.position = new Vector3(thisTransform.position.x, thisTransform.position.y,pebbleBar.transform.position.z);
//	}

	/* ---- OTHER ---- */
	private void offsetCircles () {
		soundEmitt1.setCharacterMoveOffset(vectorFixed.x);
		soundEmitt2.setCharacterMoveOffset(vectorFixed.x);
	}



	private void checkInput()
	{
		// these are false unless one of keys is pressed
		isLeft = false;
		isRight = false;
		isJump = false;
		isGoDown = false;
		isCrounch = false;
		
		movingDir = moving.None;

	}

	void OnTriggerEnter(Collider other) 
	{
		if(other.gameObject.CompareTag("Player")) 
		{
			GameEventManager.TriggerGameOver(gameObject);
			AttackSound.playSound(true);
			chasingPlayer = false;
		}
	}

	private void GameStart () {
		if(FindObjectOfType(typeof(Walker)) && this != null) {
			setIniState();
			enabled = true;
		}
	}
	
	private void GameOver () {
		enabled = false;
		isLeft = false;
		isRight = false;
		isJump = false;
		movingDir = moving.None;
	}
	private void GamePause()
	{
		enabled = false;
		isLeft = false;
		isRight = false;
		isJump = false;
		paused = true;
		movingDir = moving.None;	
	}
	private void GameUnpause()
	{
		enabled = true;	
		paused = false;
	}
}
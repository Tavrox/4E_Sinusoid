using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Walker : Enemy {
	
	[HideInInspector] public Vector3 position;
	[HideInInspector] public Transform trans;

	/***** ENNEMI BEGIN *****/

	private Vector3 direction;
	
	public float targetDetectionArea = 3f, timeSearchingPlayer = 2f;
	private float blockDetectionArea = 2f;
	
	private RaycastHit hitInfo; //infos de collision
	private Ray detectTargetLeft, detectTargetRight; //point de départ, direction
	
	private bool goLeft = true;
	private int waypointId = 0;
	//public Transform[] waypoints;
	public List<Transform> waypoints= new List<Transform>(), endChaseArea= new List<Transform>();
	public float[] timePauseWP;
	private float timeToWait=0;

	/***** ENNEMI END *****/
//	public Pebble instPebble;
//	public WaveCreator instFootWave,instInstruWave;
	private GameObject GOinstFootWave, GOinstInstruWave;
	//public GameObject instPebbleBar;
	public float footStepDelay = 0.6f;

	private WaveCreator soundEmitt1, soundEmitt2, soundInstru1/*, soundInstru2,soundEmitt3*/;
	private int cptWave=1, pebbleDirection = 1;
	private bool blockCoroutine, first, toSprint, toWalk, specialCast, playerDirLeft, waypointReached, loosingPlayer;
	private Pebble pebble1;
	private float powerPebble;
//	private GameObject pebbleBar;
	private float waypointDetectionWidth;
	public LayerMask projectorMask;
	
	[HideInInspector] public bool paused = false;
	
	// Use this for initialization
	public override void Start () 
	{
		base.Start();
		
		GameEventManager.GameStart += GameStart;
		GameEventManager.GameOver += GameOver;
		GameEventManager.GamePause += GamePause;
		GameEventManager.GameUnpause += GameUnpause;

		
		GOinstFootWave = Instantiate(Resources.Load("Prefabs/04Gameplay/SoundWavesEmitter")) as GameObject;
		soundEmitt1 = GOinstFootWave.GetComponent<WaveCreator>();soundEmitt1.gameObject.name = "_footWaveWalker1";//footsteps wave 1
		GOinstFootWave = Instantiate(Resources.Load("Prefabs/04Gameplay/SoundWavesEmitter")) as GameObject;
		soundEmitt2 = GOinstFootWave.GetComponent<WaveCreator>();soundEmitt2.gameObject.name = "_footWaveWalker2";//footsteps wave 2
		GOinstInstruWave = Instantiate(Resources.Load("Prefabs/04Gameplay/SoundWavesInstru")) as GameObject;
		soundInstru1 = GOinstInstruWave.GetComponent<WaveCreator>();soundInstru1.gameObject.name = "_instruWaveWalker1"; //intru wave 1

		soundEmitt1.gameObject.transform.parent = soundEmitt2.gameObject.transform.parent = soundInstru1.gameObject.transform.parent = GameObject.Find("Level/Waves/").transform;

//		soundEmitt1 = Instantiate(instFootWave) as WaveCreator;
//		soundEmitt2 = Instantiate(instFootWave) as WaveCreator;
//		soundInstru1 = Instantiate(instInstruWave) as WaveCreator;
		soundEmitt1.createCircle(thisTransform);soundEmitt1.setParent(thisTransform);
		soundEmitt2.createCircle(thisTransform);soundEmitt2.setParent(thisTransform);
		soundInstru1.createCircle(thisTransform);soundInstru1.specialCircle();soundInstru1.setParent(thisTransform);
		//pebble1.setCallerObject(thisTransform);
		//enabled = false;
		runSpeed = 0.5f;

		setTarget(transform); //target
		patroling = true;
		waypointDetectionWidth = thisTransform.gameObject.GetComponentInChildren<Transform>().GetComponentInChildren<OTSprite>().transform.localScale.x/2;//transform.localScale.x;
		StartCoroutine("goToWaypoint",waypointId);
	}
	private void setIniState() {
		thisTransform.position = spawnPos;
		StartCoroutine("goToWaypoint",waypointId);
		cptWave = 1;
		goLeft = patroling = true;
		waypointId = 0;
		timeToWait=0;
		pebbleDirection = 1;
		StopCoroutine("waitB4FootStep");StopCoroutine("footStep");
		waypointReached = chasingPlayer = endChasingPlayer = blockCoroutine = isLeft = isRight = isJump = isGoDown = isPass = isCrounch = false;
		movingDir = moving.None;
		StopCoroutine("goToWaypoint");StopCoroutine("waitAtWP");
		StartCoroutine("goToWaypoint",waypointId);
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
		
		offsetCircles ();
		detectPlayer();
		detectEndChaseArea();

		if(!endChasingPlayer) {
			if(chasingPlayer) {/*if(target==null) stopChasing();else*/ ChasePlayer();}
			else if(patroling) {Patrol();}
		}
		else {
			if (!Physics.Raycast(detectTargetLeft, out hitInfo, targetDetectionArea, projectorMask) && !Physics.Raycast(detectTargetRight, out hitInfo, targetDetectionArea, projectorMask) && !loosingPlayer) {
				//if(hitInfo.collider.name != "Player") {
				//goBackToPatrol();
				StartCoroutine("goBackToPatrol");
				//}
			}//AJOUTER D'AUTRES CONDITIONS
			//else goBackToPatrol();
			//if(!chasingPlayer) goBackToPatrol();
		}
//		checkInput();
//		UpdateMovement();
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
			if(hitInfo.collider.name == "Player") {
				setTarget(GameObject.FindWithTag("Player").transform);
				activeChasing();
			}
		}
	}
	private void detectEndChaseArea() {
		foreach(Transform chaseAreaLimit in endChaseArea) {
			if(Vector3.Distance(transform.position, chaseAreaLimit.position) < waypointDetectionWidth) {
				endPFReached = true;
				StopCoroutine("footStep");
				stopChasing();
			}
		}
	}

	/************************
	 *						*
	 *    IA MANAGEMENT		*
	 *						*
	 ***********************/
	private IEnumerator goBackToPatrol () {
		loosingPlayer = true;
		yield return new WaitForSeconds(timeSearchingPlayer);
		isLeft = false;
		isRight = false;
		patroling = true;
		setTarget(transform); //target
		waypointId=0;
		float distTemp=Vector2.Distance(new Vector2(transform.position.x,0f), new Vector2(waypoints[waypointId].position.x,0f));
		int cpt=1;
		foreach(Transform point in waypoints) {
			if(Vector2.Distance(new Vector2(transform.position.x,0f), new Vector2(waypoints[waypointId].position.x,0f)) < distTemp) {
				distTemp = Vector2.Distance(new Vector2(transform.position.x,0f), new Vector2(waypoints[waypointId].position.x,0f));
				waypointId=cpt;
			}
			cpt++;
		}

//		if(facingDir == facing.Right) {
//			waypointId = waypoints.Count-1;
//		}
//		if(facingDir == facing.Left) {
//			waypointId = 0;
		//		}
		blockCoroutine = false;
		StartCoroutine("waitB4FootStep");
		StartCoroutine("goToWaypoint",waypointId);
		loosingPlayer = endPFReached = endChasingPlayer = false;/*********************/

	}
	private IEnumerator goToWaypoint (int waypointIDToReach) {
		if(waypoints[waypointIDToReach].position.x > transform.position.x) {
			isRight = true;
			isLeft = false;
			facingDir = facing.Right;
		}
		if(waypoints[waypointIDToReach].position.x < transform.position.x) {
			isLeft = true;
			isRight = false;
			facingDir = facing.Left;
		}
		UpdateMovement();
		yield return new WaitForSeconds(0.01f);
		if(Mathf.Abs(transform.position.x-waypoints[waypointIDToReach].position.x) < waypointDetectionWidth) {
			//endChasingPlayer = false;
			//StopCoroutine("goToWaypoint");
			waypointReached = true;
		}
		else StartCoroutine("goToWaypoint",waypointIDToReach);
	}
	private void Patrol () {
		if(waypoints.Count<=0) print("No Waypoints linked");

		if(waypointReached) {
			waypointReached = false;
			timeToWait = timePauseWP[waypointId];
			isRight = isLeft = false;
			StopCoroutine("goToWaypoint");
			if(goLeft) {
				if(waypointId-1 < 0) {
					goLeft = false;
					waypointId++;
				}
				else waypointId--;
			}
			else {
				if(waypointId >= waypoints.Count-1) {
					goLeft = true;
					waypointId--;
				}
				else waypointId++;
			}
			StartCoroutine("waitAtWP",timeToWait);
		}
	}
	private IEnumerator waitAtWP(float timePause) {
		yield return new WaitForSeconds(timePause);
		StartCoroutine("goToWaypoint",waypointId);
	}

	private void ChasePlayer () {
		StopCoroutine("goToWaypoint");
		StopCoroutine("waitAtWP");
		if (target.position.x < thisTransform.position.x+waypointDetectionWidth) {
			//direction = Vector3.left;
			isLeft = true;
			isRight = false;
			facingDir = facing.Left;
			UpdateMovement();
		}
		else if (target.position.x > thisTransform.position.x-waypointDetectionWidth /*&& isLeft == false*/) {
			//direction = Vector3.right;
			isRight = true; 
			isLeft = false;
			facingDir = facing.Right;
			UpdateMovement();
		}
		else {
			isLeft = isRight = false;
			targetReached();
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

	/* ---- SOUND DETECTION ---- */


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
		isPass = false;
		isCrounch = false;
		
		movingDir = moving.None;

	}

	void OnTriggerEnter(Collider other) 
	{
		if(other.gameObject.CompareTag("Player")) 
		{
			GameEventManager.TriggerGameOver();
			chasingPlayer = false;
		}
	}

	private void GameStart () {
		if(FindObjectOfType(typeof(Walker)) && this != null) {
			//transform.localPosition = spawnPos;
			setIniState();
			enabled = true;
		}
	}
	
	private void GameOver () {
		enabled = false;
		isLeft = false;
		isRight = false;
		isJump = false;
		isPass = false;
		movingDir = moving.None;
	}
	private void GamePause()
	{
		enabled = false;
		isLeft = false;
		isRight = false;
		isJump = false;
		isPass = false;
		paused = true;
		movingDir = moving.None;	
	}
	private void GameUnpause()
	{
		enabled = true;	
		paused = false;
	}
}
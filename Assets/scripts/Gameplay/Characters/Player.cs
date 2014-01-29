using UnityEngine;
using System.Collections;

public class Player : Character {
	
	[HideInInspector] public Vector3 position;
	[HideInInspector] public Transform trans;

	private GameObject GOpebble, GOinstFootWave, GOinstInstruWave;

	private WaveCreator soundEmitt1, soundEmitt2, soundEmitt3, soundInstru1, soundInstru2; //waves footsteps 1, 2, 3 | intru 1, 2 so that the active wave is not destroyed when calling a another one 
	private int cptWave=1, pebbleDirection = 1, pebbleMaxStrengh = 10;//cptWave = ID of the current displayed wave (from 1 to 3)| pebbleDirection = 1 or -1 -> right or left
	private bool 	blockCoroutine, first, 		//block the footsteps coroutine|first instru wave or not
					playerDirLeft, checkingGrabPosition, //player goes left or not (used for offsetting footwaves center point)
					firstFalling,firstGrounded; //Check when player hits or leave the floor once
	private Pebble pebble; //Throwable pebble
	private float powerPebble; //Throwing force added to the pebble after cast
	private GameObject pebbleBar; //UI Bar to tell the player the power of his shoot
	public float footStepDelay = 0.8f, footStepDelayFall = 0.4f, footStepDelaySprint = 0.4f;
	public int pebbleCount = 1;
//	private bool isSprint = false;

	[HideInInspector] public bool isSprint,toSprint,toWalk, specialCast, takingInstr, launchPebble, preparePebble;//if true(left shift pressed) footwaves' speed velocity increase | if true(left shift not pressed) footwaves' speed velocity decrease | true when playing instru (locks player and footsteps)
	[HideInInspector] public bool hasFallen;

	private float crounchTime = 0.3f, footStepDelayINI;

	public FESound WalkSound;
	public FESound RunSound;
	public FESound PrepareRockThrowSound;
	public FESound LaunchRockThrowSound;
	public FESound InstruSound;
	public FESound FallSound;
	public FESound JumpSound;
	public FESound StandUpSound;
	public FESound HideSound;

	[HideInInspector] public bool paused = false;
	
	// Use this for initialization
	public override void Start () {
		base.Start();

		
		GameEventManager.GameStart += GameStart;
		GameEventManager.GameOver += GameOver;
		GameEventManager.GamePause += GamePause;
		GameEventManager.GameUnpause += GameUnpause;

		spawnPos = thisTransform.position;

		if (WalkSound != null)
		{
			InvokeRepeating("playFootstepLeft",0f,WalkSound.RepeatRate);
			InvokeRepeating("playFootstepRight",WalkSound.Delay,WalkSound.RepeatRate);
			InvokeRepeating("playRunstepLeft",0f,RunSound.RepeatRate);
			InvokeRepeating("playRunstepRight",WalkSound.Delay,RunSound.RepeatRate);
		}

		//Creating waves game objects
		GOinstFootWave = Instantiate(Resources.Load("Prefabs/04Gameplay/SoundWavesEmitter")) as GameObject; //footsteps wave 1
		soundEmitt1 = GOinstFootWave.GetComponent<WaveCreator>();soundEmitt1.gameObject.name = "_footWavePlayer1";
		GOinstFootWave = Instantiate(Resources.Load("Prefabs/04Gameplay/SoundWavesEmitter")) as GameObject; //footsteps wave 2
		soundEmitt2 = GOinstFootWave.GetComponent<WaveCreator>();soundEmitt2.gameObject.name = "_footWavePlayer2";
		GOinstFootWave = Instantiate(Resources.Load("Prefabs/04Gameplay/SoundWavesEmitter")) as GameObject; //footsteps wave 3
		soundEmitt3 = GOinstFootWave.GetComponent<WaveCreator>();soundEmitt3.gameObject.name = "_footWavePlayer3";
		GOinstInstruWave = Instantiate(Resources.Load("Prefabs/04Gameplay/SoundWavesInstru")) as GameObject; //intru wave 1
		soundInstru1 = GOinstInstruWave.GetComponent<WaveCreator>();soundInstru1.gameObject.name = "_instruWavePlayer1";
		GOinstInstruWave = Instantiate(Resources.Load("Prefabs/04Gameplay/SoundWavesInstru")) as GameObject; //instru wave 2
		soundInstru2 = GOinstInstruWave.GetComponent<WaveCreator>();soundInstru2.gameObject.name = "_instruWavePlayer2";
		
		soundEmitt1.gameObject.transform.parent = soundEmitt2.gameObject.transform.parent = soundEmitt3.gameObject.transform.parent = 
			soundInstru1.gameObject.transform.parent = soundInstru2.gameObject.transform.parent = GameObject.Find("Level/Waves/").transform;

		soundEmitt1.createCircle(thisTransform);soundEmitt1.setParent(thisTransform); //creating wave elements of FOOT wave 1
		soundEmitt2.createCircle(thisTransform);soundEmitt2.setParent(thisTransform); //creating wave elements of FOOT wave 2
		soundEmitt3.createCircle(thisTransform);soundEmitt3.setParent(thisTransform); //creating wave elements of FOOT wave 3
		soundInstru1.createCircle(thisTransform);soundInstru1.specialCircle();soundInstru1.setParent(thisTransform); //creating wave elements of INSTRU wave 1 & setting waves params to INSTRU
		soundInstru2.createCircle(thisTransform);soundInstru2.specialCircle();soundInstru2.setParent(thisTransform); //creating wave elements of INSTRU wave 2 & setting waves params to INSTRU
	
		pebbleBar = Instantiate(Resources.Load("Prefabs/04Gameplay/PebbleBar")) as GameObject; //Create UI power bar
		footStepDelayINI = footStepDelay;
	}
	private void setIniState() {
		moveVel = moveVelINI;
	}

	// Update is called once per frame
	public void Update () {
		checkInput();
		UpdateMovement();
		offsetCircles (); //Replace waves at the center of the player (+/- offests)
	}

	private void checkInput() {
		// these are false unless one of keys is pressed
		isLeft = false;
		isRight = false;
		isJump = false;
		isGoDown = false;

		movingDir = moving.None;

		#region Pebble (F)
		if (Input.GetKeyDown(KeyCode.F)) { //OnPress -> set pebble power to 0 and create powerBar
			if(!pebble) { //Only if not already existing (can't have 2 pebbles at the same time)
				powerPebble = 0f; //
				setPebbleBarPos();
				pebbleBar.transform.localScale = new Vector3(0f,1f,1f);
				preparePebble = specialCast = true;
			}
		}
		if (Input.GetKey(KeyCode.F)) { //Hold F to add power
			if(powerPebble <= pebbleMaxStrengh && !pebble) { //Pebble max strenght
				preparePebble = specialCast = true;
				powerPebble += 0.2f;
				pebbleBar.transform.localScale = new Vector3(powerPebble,0.3f,1f); //Resize powerBar
				setPebbleBarPos();
			}
		}
		if (Input.GetKeyUp(KeyCode.F)) { //RELEASE THE PEBBLE !!
			if(!pebble) { //If no pebble already existing
				preparePebble = false;
				launchPebble = true;
				GOpebble = Instantiate(Resources.Load("Prefabs/04Gameplay/Pebble")) as GameObject;
				pebble = GOpebble.GetComponent<Pebble>(); //Create Pebble
				pebble.setPosition((transform.position.x-transform.localScale.x/2),(transform.position.y+transform.localScale.y/2), -6f); //Pebble ini position
				pebble.setCallerObject(thisTransform);
				pebbleDirection = (facingDir == facing.Right) ? 1 : -1;	//Direction of the pebble
				pebble.throwPebble(powerPebble, pebbleDirection); //Throw pebble function
				powerPebble = 0f; //reset power
				pebbleBar.transform.localScale = new Vector3(powerPebble,0.3f,1f); //reset power bar
				pebbleBar.transform.position = new Vector3((powerPebble/2)+thisTransform.position.x,thisTransform.position.y+2f,-30f);
			}
		}
		if (Input.GetKeyDown(KeyCode.KeypadPlus))
		{
			pebbleCount += 1;
		}
		if (Input.GetKeyDown(KeyCode.KeypadMinus))
		{
			pebbleCount -= 1;
		}
		#endregion
		#region TEMPORARY MUST NOT BE IN FINAL VERSION (Y) & (T) -> Destroy current circles
		if (Input.GetKeyDown(KeyCode.Y))
		{
			soundInstru2.destroyCircle();
		}
		if (Input.GetKeyDown(KeyCode.T))
		{
			soundInstru1.destroyCircle();
		}
		#endregion
		#region Instru Skill (R)
		if (Input.GetKeyDown(KeyCode.R)  && grounded && !specialCast) {
			playSoundInstru();
			StartCoroutine("specialCircleCast");
		}
		#endregion
		#region Sprint management (LeftShift)
		if(Input.GetKeyDown("left shift")) {//OnPress
			moveVel = moveVelSprint; //Increase Player Speed
			//footStepDelay = footStepDelaySprint; //Decrease FootStep Delay
			isSprint = true;
		}
		if(Input.GetKeyUp("left shift")) {//OnRelease
			moveVel = moveVelINI; //Decrease Player Speed
			//footStepDelay = footStepDelayINI; //Increase FootStep Delay
			isSprint = false;
		}
		if(Input.GetKeyDown("left shift")) {//LeftShift input
			toSprint=true;
		}
		else if(Input.GetKeyUp("left shift")) {//NO LeftShift input
			toWalk=true;
		}
		/*if(!blockCoroutine) {*/
		if(toSprint && grounded) {
			StopCoroutine("queueWaveState");
			StartCoroutine(queueWaveState("ToSprint",soundEmitt1));
			StartCoroutine(queueWaveState("ToSprint",soundEmitt2));
			StartCoroutine(queueWaveState("ToSprint",soundEmitt3));
//				/*if(soundEmitt1.getAlpha() <= 0f)*/ soundEmitt1.circleWalkToSprint();
//			/*if(soundEmitt2.getAlpha() <= 0f)*/ soundEmitt2.circleWalkToSprint();
//			/*if(soundEmitt3.getAlpha() <= 0f)*/ soundEmitt3.circleWalkToSprint();
				toSprint=false;
			footStepDelay = footStepDelaySprint;
			}
		else if (toWalk && grounded) {
			StopCoroutine("queueWaveState");
			StartCoroutine(queueWaveState("ToWalk",soundEmitt1));
			StartCoroutine(queueWaveState("ToWalk",soundEmitt2));
			StartCoroutine(queueWaveState("ToWalk",soundEmitt3));
//			/*if(soundEmitt1.getAlpha() <= 0f)*/ soundEmitt1.circleSprintToWalk();
//			/*if(soundEmitt2.getAlpha() <= 0f)*/ soundEmitt2.circleSprintToWalk();
//			/*if(soundEmitt3.getAlpha() <= 0f)*/ soundEmitt3.circleSprintToWalk();
			toWalk=false;
			footStepDelay = footStepDelayINI;
		}
		/*}*/
		#endregion
		#region Movement (Left), (Right), (Up), (Down), (Space)
		if((Input.GetKey("left") || Input.GetKey(KeyCode.Q)) && !specialCast) { //If input left & not casting instru
			isLeft = true;
			shootLeft = true;
			facingDir = facing.Left;
			if(!blockCoroutine && grounded) StartCoroutine("waitB4FootStep");
		}
		if(((Input.GetKeyUp("left") || Input.GetKeyUp(KeyCode.Q)) && !specialCast) || ((Input.GetKeyUp("right") || Input.GetKeyUp(KeyCode.D)) && !isLeft && !specialCast)) { 
			StopCoroutine("footStep");
			StopCoroutine("waitB4FootStep");
			blockCoroutine = false;
		}
		if(((Input.GetKeyDown("left") || Input.GetKeyDown(KeyCode.Q)) && !specialCast) || ((Input.GetKeyDown("right") || Input.GetKeyDown(KeyCode.D)) && !isLeft && !specialCast)) { 
			StopCoroutine("waitB4FootStep");
			StopCoroutine("footStep");
			blockCoroutine = false;
		}
		if ((Input.GetKey("right") || Input.GetKey(KeyCode.D)) && !isLeft && !specialCast) { //If input right & not casting instru
			isRight = true; 
			facingDir = facing.Right;
			shootLeft = false;
			if(!blockCoroutine && grounded) StartCoroutine("waitB4FootStep");
		}
		if ((Input.GetKeyDown("down") || Input.GetKeyDown(KeyCode.S)) && !specialCast && grounded) {

			if(isGrab) {checkingGrabPosition = false;StopCoroutine("checkGrabberPosition");isGrab = false;}
			else {
				if ( onEnvironment != null && onEnvironment.typeList == Environment.types.wood)
				{
					isCrounch = true;
					facingDir = facing.Down;
					StartCoroutine("CrounchMode");
				}
			}
		}
		if ((Input.GetKeyDown("up") || Input.GetKeyDown(KeyCode.Z)) && !specialCast && grounded) {
			if(isGrab) {checkingGrabPosition = false;StopCoroutine("checkGrabberPosition");isGrab = false;}
			isJump = true; 
			grounded = false;
		}
		#endregion
		#region Alpha (1), (2), (3)
		if (Input.GetKeyDown(KeyCode.Alpha1)) {
			//skill_axe.useSkill(Skills.SkillList.Axe);
		}
		if (Input.GetKeyDown(KeyCode.Alpha2)) {

		}
		if (Input.GetKeyDown(KeyCode.Alpha3)) {

		}
		#endregion
		#region (Escape)
		if (Input.GetKeyDown(KeyCode.Escape)) {
			if (GameEventManager.state != GameEventManager.GameState.Pause) GameEventManager.TriggerGamePause();
			else if (GameEventManager.state == GameEventManager.GameState.Pause) GameEventManager.TriggerGameUnpause();
		}
		#endregion
		if(grounded && checkingGrabPosition) {checkingGrabPosition = false;StopCoroutine("checkGrabberPosition");}

		if(grounded) {
			if(!firstGrounded) {
				//print("BEGIN BEING GROUNDED");
				firstGrounded = true;
				firstFalling = false;
				//StopCoroutine("footStep");
				playSoundFall();
				//cptWave = 1;
				if(fallFast) {
					fallFast = false;
					//soundEmitt3.circleWalkToSprint();
					soundEmitt3.resetCircle(transform.localScale.x/1.5f,playerDirLeft, true);
				}
//				soundEmitt1.circleFallToGrounded();
//				soundEmitt2.circleFallToGrounded();
				//				soundEmitt3.circleFallToGrounded();
				StopCoroutine("footStep");
				StopCoroutine("waitB4FootStep");
				blockCoroutine = false;
				
				StopCoroutine("queueWaveState");
				if(isSprint) { 
					footStepDelay = footStepDelaySprint;
					StartCoroutine(queueWaveState("ToSprint",soundEmitt1));
					StartCoroutine(queueWaveState("ToSprint",soundEmitt2));
					StartCoroutine(queueWaveState("ToSprint",soundEmitt3));
				}
				else {
					footStepDelay=footStepDelayINI;
					StartCoroutine(queueWaveState("ToGround",soundEmitt1));
					StartCoroutine(queueWaveState("ToGround",soundEmitt2));
					StartCoroutine(queueWaveState("ToGround",soundEmitt3));
				}
			}
		}
		else {
			if(!firstFalling) {
				//print("BEGIN FALLING");
				StopCoroutine("footStep");
				StopCoroutine("waitB4FootStep");
				blockCoroutine = false;
				footStepDelay=footStepDelayFall;
				firstFalling = true;
				firstGrounded = false;
//				soundEmitt1.circleGroundedToFall();
//				soundEmitt2.circleGroundedToFall();
//				soundEmitt3.circleGroundedToFall();
				StopCoroutine("queueWaveState");
				StartCoroutine(queueWaveState("ToFall",soundEmitt1));
				StartCoroutine(queueWaveState("ToFall",soundEmitt2));
				StartCoroutine(queueWaveState("ToFall",soundEmitt3));
			}
			StartCoroutine("waitB4FallWave");
		}
//		print (vectorMove.y);
//		if(!grounded && !blockCoroutine) {/*footStepDelay=footStepDelayFall;StartCoroutine("waitB4FallWave");*/}
//		else if (grounded && !blockCoroutine) {
//			/**/
//		}
	}
	IEnumerator queueWaveState (string state, WaveCreator soundEmitt) {
		yield return new WaitForSeconds(0.01f);
		soundEmitt.setAlpha();
		//print(state+" "+soundEmitt.getAlpha()+" "+soundEmitt.name);
		if(soundEmitt.getAlpha() <= 0f) {
			switch (state) {
			case "ToWalk":
				soundEmitt.circleSprintToWalk();
				break;
			case "ToSprint":
				soundEmitt.circleWalkToSprint();
				break;
			case "ToFall":
				soundEmitt.circleGroundedToFall();
				break;
			case "ToGround":
				soundEmitt.circleFallToGrounded();
				break;
			}
		}
		else {
			StartCoroutine(queueWaveState(state,soundEmitt));
		}
	}
	IEnumerator waitB4FallWave() { //Short Delay before the sprite actually touches the ground (edit when anim is finished)
		yield return new WaitForSeconds(0.7f);
		if(!blockCoroutine && !grounded && !isGrab) StartCoroutine("footStep");
	}
	void OnTriggerEnter(Collider col) {
		if(col.gameObject.CompareTag("platformGrabber") && !grounded) 
		{
			StartCoroutine("checkGrabberPosition", col);
		}
	}
	private IEnumerator checkGrabberPosition(Collider col) {
		checkingGrabPosition = true;
		yield return new WaitForSeconds(0.01f);
		//print(col.transform.position.y-(thisTransform.position.y+halfMyY));
		if(col.transform.position.y-(thisTransform.position.y+halfMyY) > -0.5f) {
			isGrab = true;
			checkingGrabPosition = false;
		}
		else {
			StartCoroutine("checkGrabberPosition",col);
		}
	}

	private void offsetCircles () { //Set circles position
		soundEmitt1.setCharacterMoveOffset(vectorFixed.x);
		soundEmitt2.setCharacterMoveOffset(vectorFixed.x);
		soundEmitt3.setCharacterMoveOffset(vectorFixed.x);
	}
	private void setPebbleBarPos() { //Set pebbleBar position
		pebbleBar.transform.position = new Vector3((powerPebble/2)+thisTransform.position.x,thisTransform.position.y+2f,-30f); //Replace powerBar as resize is made from center expanding to each side
	}
	IEnumerator waitB4FootStep() { //Short Delay before the sprite actually touches the ground (edit when anim is finished)
		yield return new WaitForSeconds(0.5f);
		if(!blockCoroutine && grounded) StartCoroutine("footStep");
	}
	IEnumerator footStep() { //Footsteps management
		blockCoroutine =true;
		//print (footStepDelay);
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
		if(cptWave == 1) {cptWave++;soundEmitt1.resetCircle(transform.localScale.x/1.5f,playerDirLeft, true);} //First wave launch
		else if (cptWave == 2) {cptWave++;soundEmitt2.resetCircle(transform.localScale.x/1.5f,playerDirLeft, true);} //Second wave launch
		else if (cptWave == 3) {cptWave=1;soundEmitt3.resetCircle(transform.localScale.x/1.5f,playerDirLeft, true);} //Third wave launch
		yield return new WaitForSeconds(footStepDelay); //Wait before launching next wave....wait...wait...wait...

		blockCoroutine = false; //UNLOCK THE COROUTIIIINE !!
	}
	IEnumerator specialCircleCast() { //Instru management
		specialCast = true; //Say "I AM PLAYING AN INTRUMENT" to the rest of the game (lock player)
		takingInstr = true;
		yield return new WaitForSeconds(InstruSound.Delay); //Cast time before playing the sound (the character has to take his intrument out of his ass)
		takingInstr = false;
		 //Cast time before playing the sound (the character has to take his intrument out of his ass)
		if(first) {first=!first;StartCoroutine("specialCirclePlay",soundInstru1);/*soundInstru1.resetCircle();*/} //If it's the first time playing
		else {first=!first;StartCoroutine("specialCirclePlay",soundInstru1);/*soundInstru2.resetCircle();*/} //If it's the second time playing (2 waves so that player can display both on screen if spamming music)
		//yield return new WaitForSeconds(soundInstru1.getLifeTime());
		yield return new WaitForSeconds(2.75f);
		StopCoroutine("specialCirclePlay");
		specialCast = false; //Not playing anymore, can move again
	}
	IEnumerator specialCirclePlay(WaveCreator soundInstru) {
		soundInstru.resetCircle();
		yield return new WaitForSeconds(0.5f);
		StartCoroutine("specialCirclePlay",soundInstru);
	}
	IEnumerator CrounchMode()
	{
		yield return new WaitForSeconds(crounchTime);
		isCrounch = false;
	}

	public override void BlockedUp()
	{
		aboveEnvironment = hitInfo.collider.GetComponent<Environment>();
		if(vectorMove.y > 0)
		{
			if (aboveEnvironment.typeList !=null && Environment.types.wood !=null && aboveEnvironment.typeList != Environment.types.wood)
   			{
				vectorMove.y = 0f;
				blockedUp = true;
			}
		}
	}
	
	#region PlaySounds
	private void playFootstepLeft()
	{
		if ((isLeft == true || isRight == true) && grounded && isSprint == false )
		{
			WalkSound.playLeftSound(onEnvironment);
		}
	}
	private void playFootstepRight()
	{
		if ((isLeft == true || isRight == true) && grounded && isSprint == false  )
		{
			WalkSound.playRightSound(onEnvironment);
		}
	}
	private void playRunstepRight()
	{
		if ((isLeft == true || isRight == true) && grounded && isSprint )
		{
			RunSound.playRightSound(onEnvironment);
		}
	}
	private void playRunstepLeft()
	{
		if ((isLeft == true || isRight == true) && grounded && isSprint )
		{
			RunSound.playLeftSound(onEnvironment);
		}
	}
	private void playSoundPrepareRockThrow()
	{

	}
	private void playSoundLaunchRockThrow()
	{
		
	}
	private void playSoundGrip()
	{
		
	}
	private void playSoundHide()
	{
		HideSound.playSound(onEnvironment);
	}
	private void playSoundInstru()
	{
		InstruSound.playSound();
	}
	private void playSoundStandUp()
	{
		StandUpSound.playSound(onEnvironment);
	}
	private void playSoundJump()
	{
		JumpSound.playSound(onEnvironment);
	}
	private void playSoundFall()
	{
		//FallSound.playSound(onEnvironment);
	}
	#endregion


	#region Game State Management - Events detection
	private void GameStart () {
		if(FindObjectOfType(typeof(Player)) && this != null) {
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
		movingDir = moving.None;
	}
	private void GamePause() {
		enabled = false;
		isLeft = false;
		isRight = false;
		isJump = false;
		paused = true;
		movingDir = moving.None;
		
	}
	private void GameUnpause() {
		paused = false;
		enabled = true;	
	}
	#endregion
}

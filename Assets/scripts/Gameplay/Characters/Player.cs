using UnityEngine;
using System.Collections;

public class Player : Character {
	
	[HideInInspector] public Vector3 position;
	[HideInInspector] public Transform trans;

	private GameObject GOpebble, GOinstFootWave, GOinstInstruWave;
	
	[SerializeField] private Rect hp_display;
	private WaveCreator soundEmitt1, soundEmitt2, soundEmitt3, soundInstru1, soundInstru2; //waves footsteps 1, 2, 3 | intru 1, 2 so that the active wave is not destroyed when calling a another one 
	private int cptWave=1, pebbleDirection = 1, pebbleMaxStrengh = 10;//cptWave = ID of the current displayed wave (from 1 to 3)| pebbleDirection = 1 or -1 -> right or left
	private bool 	blockCoroutine, first, 		//block the footsteps coroutine|first instru wave or not
					specialCast, playerDirLeft; //true when playing instru (locks player and footsteps) | player goes left or not (used for offsetting footwaves center point)
	private Pebble pebble; //Throwable pebble
	private float powerPebble; //Throwing force added to the pebble after cast
	private GameObject pebbleBar; //UI Bar to tell the player the power of his shoot
	public float footStepDelay;

	[HideInInspector] public bool isSprint,toSprint,toWalk;//if true(left shift pressed) footwaves' speed velocity increase | if true(left shift not pressed) footwaves' speed velocity decrease

	public FESound WalkSound;
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
		{ InvokeRepeating("playFootstep",5f,WalkSound.RepeatRate);}

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
		isPass = false;
		isCrounch = false;

		movingDir = moving.None;

		#region Pebble (F)
		if (Input.GetKeyDown(KeyCode.F)) { //OnPress -> set pebble power to 0 and create powerBar
			if(!pebble) { //Only if not already existing (can't have 2 pebbles at the same time)
				powerPebble = 0f; //
				setPebbleBarPos();
				pebbleBar.transform.localScale = new Vector3(0f,1f,1f);
			}
		}
		if (Input.GetKey(KeyCode.F)) { //Hold F to add power
			if(powerPebble <= pebbleMaxStrengh && !pebble) { //Pebble max strenght
				powerPebble += 0.2f;
				pebbleBar.transform.localScale = new Vector3(powerPebble,0.3f,1f); //Resize powerBar
				setPebbleBarPos();
			}
		}
		if (Input.GetKeyUp(KeyCode.F)) { //RELEASE THE PEBBLE !!
			if(!pebble) { //If no pebble already existing

				GOpebble = Instantiate(Resources.Load("Prefabs/04Gameplay/Pebble")) as GameObject;
				pebble = GOpebble.GetComponent<Pebble>(); //Create Pebble
				pebble.setPosition((transform.position.x-transform.localScale.x/2),transform.position.y, -6f); //Pebble ini position
				pebble.setCallerObject(thisTransform);
				pebbleDirection = (facingDir == facing.Right) ? 1 : -1;	//Direction of the pebble
				pebble.throwPebble(powerPebble, pebbleDirection); //Throw pebble function
				powerPebble = 0f; //reset power
				pebbleBar.transform.localScale = new Vector3(powerPebble,0.3f,1f); //reset power bar
				pebbleBar.transform.position = new Vector3((powerPebble/2)+thisTransform.position.x,thisTransform.position.y+2f,-30f);
			}
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
			StartCoroutine("specialCircleCast");
		}
		#endregion
		#region Sprint management (LeftShift)
		if(Input.GetKeyDown("left shift")) {//OnPress
			moveVel = 1.75f * moveVel; //Increase Player Speed
			footStepDelay = footStepDelay / 2f; //Decrease FootStep Delay
			isSprint = true;
		}
		if(Input.GetKeyUp("left shift")) {//OnRelease
			moveVel = moveVel / 1.75f; //Decrease Player Speed
			footStepDelay = footStepDelay * 2f; //Increase FootStep Delay
			isSprint = false;
		}
		if(Input.GetKey("left shift")) {//LeftShift input
			toSprint=true;
		}
		else if(!Input.GetKey("left shift")) {//NO LeftShift input
			toWalk=true;
		}
		/*if(!blockCoroutine) {*/
			if(toSprint) {
				if(soundEmitt1.getAlpha() <= 0f) soundEmitt1.circleWalkToSprint();
				if(soundEmitt2.getAlpha() <= 0f) soundEmitt2.circleWalkToSprint();
				if(soundEmitt3.getAlpha() <= 0f) soundEmitt3.circleWalkToSprint();
				toSprint=false;
			}
			else if (toWalk) {
				if(soundEmitt1.getAlpha() <= 0f) soundEmitt1.circleSprintToWalk();
				if(soundEmitt2.getAlpha() <= 0f) soundEmitt2.circleSprintToWalk();
				if(soundEmitt3.getAlpha() <= 0f) soundEmitt3.circleSprintToWalk();
				toWalk=false;
			}
		/*}*/
		#endregion
		#region Movement (Left), (Right), (Up), (Down), (Space)
		if(Input.GetKey("left") && !specialCast) { //If input left & not casting instru
			isLeft = true;
			shootLeft = true;
			facingDir = facing.Left;
			if(!blockCoroutine && grounded) StartCoroutine("waitB4FootStep");
		}
		if((Input.GetKeyUp("left") && !specialCast) || (Input.GetKeyUp("right") && !isLeft && !specialCast)) { 
			StopCoroutine("footStep");
			blockCoroutine = false;
		}
		if (Input.GetKey("right") && !isLeft && !specialCast) { //If input right & not casting instru
			isRight = true; 
			facingDir = facing.Right;
			shootLeft = false;
			if(!blockCoroutine && grounded) StartCoroutine("waitB4FootStep");
		}
		if (Input.GetKey(KeyCode.DownArrow)) {
			isCrounch = true;
			facingDir = facing.Down;
		}
		if (Input.GetKeyDown("up")) {
			isJump = true; 
		}
		if(Input.GetKeyDown("space")) {
			isPass = true;
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
		yield return new WaitForSeconds(0.1f);
		if(!blockCoroutine && grounded) StartCoroutine("footStep");
	}
	IEnumerator footStep() { //Footsteps management
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
		if(cptWave == 1) {cptWave++;soundEmitt1.resetCircle(transform.localScale.x/1.5f,playerDirLeft, true);} //First wave launch
		else if (cptWave == 2) {cptWave++;soundEmitt2.resetCircle(transform.localScale.x/1.5f,playerDirLeft, true);} //Second wave launch
		else if (cptWave == 3) {cptWave=1;soundEmitt3.resetCircle(transform.localScale.x/1.5f,playerDirLeft, true);} //Third wave launch
		yield return new WaitForSeconds(footStepDelay); //Wait before launching next wave....wait...wait...wait...

		blockCoroutine = false; //UNLOCK THE COROUTIIIINE !!
	}
	IEnumerator specialCircleCast() { //Instru management
		specialCast = true; //Say "I AM PLAYING AN INTRUMENT" to the rest of the game (lock player)
		yield return new WaitForSeconds(1f); //Cast time before playing the sound (the character has to take his intrument out of his ass)
		
		if(first) {first=!first;soundInstru1.resetCircle();} //If it's the first time playing
		else {first=!first;soundInstru2.resetCircle();} //If it's the second time playing (2 waves so that player can display both on screen if spamming music)
		//yield return new WaitForSeconds(soundInstru1.getLifeTime());
		specialCast = false; //Not playing anymore, can move again
	}
	
	#region PlaySounds
	private void playFootstep()
	{
		if ((isLeft == true || isRight == true) && grounded )
		{
			WalkSound.playSound();
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
		HideSound.playSound();
	}
	private void playSoundInstru()
	{
		InstruSound.playSound();
	}
	private void playSoundStandUp()
	{
		StandUpSound.playSound();
	}
	private void playSoundJump()
	{
		JumpSound.playSound();
	}
	#endregion


	#region Game State Management - Events detection
	private void GameStart () {
		if(FindObjectOfType(typeof(Player)) && this != null) {
			transform.localPosition = spawnPos;
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
	private void GamePause() {
		enabled = false;
		isLeft = false;
		isRight = false;
		isJump = false;
		isPass = false;
		paused = true;
		movingDir = moving.None;
		
	}
	private void GameUnpause() {
		paused = false;
		enabled = true;	
	}
	#endregion
}

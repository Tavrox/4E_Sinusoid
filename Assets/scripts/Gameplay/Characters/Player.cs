using UnityEngine;
using System.Collections;

public class Player : Character {
	
	[HideInInspector] public Vector3 position;
	[HideInInspector] public Transform trans;
	
	public Skills skill_knife;
	public Skills skill_axe;
	public Skills skill_shield;
	public Pebble instPebble;
	public WaveCreator instFootWave,instInstruWave;
	public GameObject instPebbleBar;
	public OTSprite menu;
	public float footStepDelay = 0.6f;
	
	[SerializeField] private Rect hp_display;
	[SerializeField] private SoundSprite soundMan;
	[SerializeField] private ModulatedSound mdSound;
	private WaveCreator soundEmitt1, soundEmitt2, soundInstru1, soundInstru2,soundEmitt3;
	private int cptWave=1, pebbleDirection = 1;
	private bool blockCoroutine, first, toSprint, toWalk, specialCast;
	private Pebble pebble1;
	private float powerPebble;
	private GameObject pebbleBar;

	[HideInInspector] public bool paused = false;
	
	// Use this for initialization
	public override void Start () 
	{
		base.Start();

		
		GameEventManager.GameStart += GameStart;
		GameEventManager.GameOver += GameOver;
		GameEventManager.GamePause += GamePause;
		GameEventManager.GameUnpause += GameUnpause;

		spawnPos = thisTransform.position;

		soundEmitt1 = Instantiate(instFootWave) as WaveCreator;
		soundEmitt2 = Instantiate(instFootWave) as WaveCreator;
		soundEmitt3 = Instantiate(instFootWave) as WaveCreator;
		soundInstru1 = Instantiate(instInstruWave) as WaveCreator;
		soundInstru2 = Instantiate(instInstruWave) as WaveCreator;
		soundEmitt1.createCircle();
		soundEmitt2.createCircle();
		soundEmitt3.createCircle();
		soundInstru1.createCircle();soundInstru1.specialCircle();
		soundInstru2.createCircle();soundInstru2.specialCircle();

		pebbleBar = Instantiate(instPebbleBar) as GameObject;
	}
	
	// Update is called once per frame
	public void Update () 
	{
		checkInput();
		UpdateMovement();
	}
	
	private void GameStart () 
	{
		if(FindObjectOfType(typeof(Player)) && this != null) {
			transform.localPosition = spawnPos;
			enabled = true;
		}
	}
	
	private void GameOver () 
	{
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
		paused = false;
		enabled = true;	
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

		// keyboard input
		if (Input.GetKeyDown(KeyCode.F))
		{
			if(!pebble1) {
			powerPebble = 0f;
			pebbleBar.transform.localScale = new Vector3(0f,1f,1f);
			}
		}
		if (Input.GetKey(KeyCode.F))
		{
			if(powerPebble <=10 && !pebble1) {
				powerPebble += 0.2f;
				pebbleBar.transform.localScale = new Vector3(powerPebble,1f,1f);
				pebbleBar.transform.position = new Vector3((powerPebble-12f)/2,5.5f,-30f);
			}
		}
		if (Input.GetKeyUp(KeyCode.F))
		{
			if(!pebble1) {
				pebble1 = Instantiate(instPebble) as Pebble;
				pebble1.setPosition((transform.position.x-transform.localScale.x/2),transform.position.y, -6f);
				pebbleDirection = (facingDir == facing.Right) ? 1 : -1;
				pebble1.throwPebble(powerPebble, pebbleDirection);
				powerPebble = 0f;
				pebbleBar.transform.localScale = new Vector3(powerPebble,1f,1f);
				pebbleBar.transform.position = new Vector3((powerPebble-12f)/2,5.5f,-30f);
			}
		}
		if (Input.GetKeyDown(KeyCode.Y))
		{
			soundInstru2.destroyCircle();
		}
		if (Input.GetKeyDown(KeyCode.T))
		{
			soundInstru1.destroyCircle();
		}
		if (Input.GetKeyDown(KeyCode.R)  && grounded && !specialCast)
		{
			StartCoroutine("specialCircleCast");
		}
		if(Input.GetKeyDown("left shift")) {
			moveVel = 1.75f * moveVel;
			footStepDelay = footStepDelay / 2f;
		}
		if(Input.GetKeyUp("left shift")) {
			moveVel = moveVel / 1.75f;
			footStepDelay = footStepDelay * 2f;
		}
		if(Input.GetKey("left shift")) {
			toSprint=true;
		}
		else if(!Input.GetKey("left shift")) {
			toWalk=true;
		}
		/*if(!blockCoroutine) {*/
			if(toSprint) 		{
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
		if(Input.GetKey("left") && !specialCast) 
		{ 
			isLeft = true;
			shootLeft = true;
			facingDir = facing.Left;
			if(!blockCoroutine && grounded) StartCoroutine("waitB4FootStep");
		}
		if((Input.GetKeyUp("left") && !specialCast) || (Input.GetKeyUp("right") && !isLeft && !specialCast)) {
			StopCoroutine("footStep");
			blockCoroutine = false;
		}
		if (Input.GetKey("right") && !isLeft && !specialCast) 
		{ 
			isRight = true; 
			facingDir = facing.Right;
			shootLeft = false;
			if(!blockCoroutine && grounded) StartCoroutine("waitB4FootStep");
		}
		if (Input.GetKey(KeyCode.DownArrow))
		{
			isCrounch = true;
			facingDir = facing.Down;
		}
		if (Input.GetKeyDown("up")) 
		{ 
			isJump = true; 
		}
		if(Input.GetKeyDown("space"))
		{
			isPass = true;
		}
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			//skill_axe.useSkill(Skills.SkillList.Axe);
		}
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{

		}
		if (Input.GetKeyDown(KeyCode.Alpha3))
		{

		}
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (GameEventManager.gamePaused == false)
			{
				GameEventManager.TriggerGamePause();
			}
			else if (GameEventManager.gamePaused == true)
			{
				GameEventManager.TriggerGameUnpause();
			}
		}
	}

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

		if(cptWave == 1) {cptWave++;soundEmitt1.resetCircle();}
		else if (cptWave == 2) {cptWave++;soundEmitt2.resetCircle();}
		else if (cptWave == 3) {cptWave=1;soundEmitt3.resetCircle();}
		yield return new WaitForSeconds(footStepDelay);

		blockCoroutine = false;
	}
	IEnumerator specialCircleCast()
	{
		specialCast = true;
		yield return new WaitForSeconds(1f);
		
		if(first) {first=!first;soundInstru1.resetCircle();}
		else {first=!first;soundInstru2.resetCircle();}
		//yield return new WaitForSeconds(soundInstru1.getLifeTime());
		specialCast = false;
	}
}

using UnityEngine;
using System.Collections;

public class Player : Character {
	
	[HideInInspector] public Vector3 position;
	[HideInInspector] public Transform trans;
	
	public Skills skill_knife;
	public Skills skill_axe;
	public Skills skill_shield;
	public OTSprite menu;
	public float footStepDelay = 0.6f;
	
	[SerializeField] private Rect hp_display;
	[SerializeField] private SoundSprite soundMan;
	[SerializeField] private ModulatedSound mdSound;
	private WavesCreator soundEmitt1, soundEmitt2, soundInstru1, soundInstru2;
	private bool blockCoroutine, first, toSprint, toWalk, specialCast;

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

		soundEmitt1 = GameObject.Find("SoundWavesEmitter1").GetComponent<WavesCreator>();
		soundEmitt2 = GameObject.Find("SoundWavesEmitter2").GetComponent<WavesCreator>();
		soundInstru1 = GameObject.Find("SoundWavesInstru1").GetComponent<WavesCreator>();
		soundInstru2 = GameObject.Find("SoundWavesInstru2").GetComponent<WavesCreator>();
		soundEmitt1.createCircle();
		soundEmitt2.createCircle();
		soundInstru1.createCircle();soundInstru1.specialCircle();
		soundInstru2.createCircle();soundInstru2.specialCircle();
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
		if (Input.GetKeyDown(KeyCode.Y))
		{
			soundInstru2.destroyCircle();
		}
		if (Input.GetKeyDown(KeyCode.T))
		{
			soundInstru1.destroyCircle();
		}
		if (Input.GetKeyDown(KeyCode.R)  && !isJump && !specialCast)
		{
			StartCoroutine("specialCircleCast");
		}
		if(Input.GetKeyDown("left shift")) {
			moveVel = 2 * moveVel;
			footStepDelay = footStepDelay / 2;
			toSprint=true;
		}
		if(Input.GetKeyUp("left shift")) {
			moveVel = moveVel / 2;
			footStepDelay = footStepDelay * 2;
			toWalk=true;
		}
		if(!blockCoroutine) {
			if(toSprint) {soundEmitt1.circleWalkToSprint();soundEmitt2.circleWalkToSprint();toSprint=false;}
			else if (toWalk) {soundEmitt1.circleSprintToWalk();soundEmitt2.circleSprintToWalk();toWalk=false;}
		}
		if(Input.GetKey("left") && !specialCast) 
		{ 
			isLeft = true;
			shootLeft = true;
			facingDir = facing.Left;
			if(!blockCoroutine && !isJump) StartCoroutine("footStep");
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
			if(!blockCoroutine && !isJump) StartCoroutine("footStep");
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
		if(first) {first=!first;soundEmitt1.resetCircle();}
		else {first=!first;soundEmitt2.resetCircle();}
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

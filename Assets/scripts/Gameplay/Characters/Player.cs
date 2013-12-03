using UnityEngine;
using System.Collections;

public class Player : Character {
	
	[HideInInspector] public Vector3 position;
	[HideInInspector] public Transform trans;
	
	public Skills skill_knife;
	public Skills skill_axe;
	public Skills skill_shield;
	public OTSprite menu;
	
	[SerializeField] private Rect hp_display;
	[SerializeField] private SoundSprite soundMan;
	[SerializeField] private ModulatedSound mdSound;
	private BlockThrower soundEmitt;

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

		soundEmitt = GameObject.Find("SoundWavesEmitter").GetComponent<BlockThrower>();
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
		
		if(Input.GetKeyDown(KeyCode.R))
		{
			soundEmitt.createCircle();
		}
		// keyboard input
		if(Input.GetKey("left")) 
		{ 
			isLeft = true;
			shootLeft = true;
			facingDir = facing.Left;
		}
		if (Input.GetKey("right") && isLeft == false) 
		{ 
			isRight = true; 
			facingDir = facing.Right;
			shootLeft = false;
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
}

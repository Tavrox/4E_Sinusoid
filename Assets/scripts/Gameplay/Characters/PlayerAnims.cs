using UnityEngine;
using System.Collections;

public class PlayerAnims : MonoBehaviour 
{
	public enum animDef
	{
		None,
		WalkLeft, WalkRight,
		RunLeft, RunRight,
		RopeLeft, RopeRight,
		Climb, ClimbStop,
		StandLeft, StandRight,
		HangLeft, HangRight,
		FallLeft, FallRight ,
		ShootLeft, ShootRight,
		CrounchLeft, CrounchRight,
		AttackLeft, AttackRight,
		TakeInstru, PlayInstru,
		PreparePebble, LaunchPebble
	}
	
	public Transform spriteParent;
	public OTAnimatingSprite animSprite;
	public OTAnimation anim;
	public int normalFPS=14, sprintFPS=20;
	private animDef currentAnim;
	private Character _character;
	private Player _player;
	
	private bool animPlaying = false;
	
	// Use this for initialization
	void Start () 
	{
		_character 	= GetComponent<Character>();
		_player 	= GetComponent<Player>();
		anim = animSprite.animation;//GameObject.Find("Player/spriteParent/playerSprite-1").GetComponent<OTAnimatingSprite
	}
	void Update() 
	{
		animSprite.looping = true;
		// Order of action matters, they need to have priorities. //
		Run();
		Walk();
		Stand();
		Crounch();
		Jump();
		Attack();
		Hurt();
		Fall();
		Paused();
		Pebble();
		PlayInstru();
	}
	private void Run()
	{
		if(_character.isRight && _character.grounded && currentAnim!=animDef.RunRight && _player.isSprint)
		{
			currentAnim = animDef.RunRight;
			anim.fps = sprintFPS;
			animSprite.Play("run");
			NormalScaleSprite();
		}
		if(_character.isLeft && _character.grounded && currentAnim!=animDef.RunLeft && _player.isSprint)
		{
			currentAnim = animDef.RunLeft;
			anim.fps = sprintFPS;
			animSprite.Play("run");
			InvertSprite();
		}
	}
	private void Walk()
	{
		if(_character.isRight && _character.grounded && currentAnim!=animDef.WalkRight && !_player.isSprint)
		{
			currentAnim = animDef.WalkRight;
			animSprite.Play("walk");
			anim.fps = normalFPS;
			NormalScaleSprite();
		}
		if(_character.isLeft && _character.grounded && currentAnim!=animDef.WalkLeft && !_player.isSprint)
		{
			currentAnim = animDef.WalkLeft;
			animSprite.Play("walk");
			anim.fps = normalFPS;
			InvertSprite();
		}
	}
	private void Stand()
	{	
		print ("CHECK RIGHT" + _character.isRight);
		print ("CHECK GROUND" + _character.grounded);
		print ("CHECK ANIM " + currentAnim);
		print ("CHECK FACING" + _character.facingDir);
		print ("CHECK PLAYING" + animPlaying);
		if(!_character.isLeft && _character.grounded == true && currentAnim != animDef.StandLeft && _character.facingDir == Character.facing.Left && animPlaying == false)
		{
			currentAnim = animDef.StandLeft;
			animSprite.Play("stand"); // stand left
			anim.fps = normalFPS;
			InvertSprite();
		}
		if(!_character.isRight && _character.grounded && currentAnim != animDef.StandRight && _character.facingDir == Character.facing.Right && animPlaying == false)
		{
			currentAnim = animDef.StandRight;
			animSprite.Play("stand"); // stand left
			anim.fps = normalFPS;
			NormalScaleSprite();
		}
	}
	private void Crounch()
	{
		if (_character.isCrounch == true)
		{
			currentAnim = animDef.CrounchLeft;
			animSprite.Play("crounch");
			anim.fps = normalFPS;
		}
	}
	private void Jump()
	{
		if(_character.grounded == false && currentAnim != animDef.FallLeft && _character.facingDir == Character.facing.Left)
		{
			currentAnim = animDef.FallLeft;
			animSprite.Play("jump"); // fall left
			anim.fps = normalFPS;
			InvertSprite();
		}
		if(_character.grounded == false && currentAnim != animDef.FallRight && _character.facingDir == Character.facing.Right)
		{
			currentAnim = animDef.FallRight;
			animSprite.Play("jump"); // fall right
			anim.fps = normalFPS;
			NormalScaleSprite();
		}
	}
	private void Attack()
	{
		/*
		if (_player.shootingKnife == true  && _character.facingDir == Character.facing.Left)
		{
			animPlaying = true;
			currentAnim = animDef.ShootRight;
			animSprite.Play("throw_knife");
			anim.fps = normalFPS;
			InvertSprite();
			StartCoroutine( WaitAndCallback( anim.GetDuration(anim.framesets[3]) ) );
		}
		if (_player.shootingKnife == true && _character.facingDir == Character.facing.Right)
		{
			animPlaying = true;
			currentAnim = animDef.ShootRight;
			animSprite.Play("throw_knife");
			anim.fps = normalFPS;
			NormalScaleSprite();
			StartCoroutine( WaitAndCallback( anim.GetDuration(anim.framesets[3]) ) );
		}
		*/
	}
	private void Hurt()
	{
		//ENEMIES SPECIFIC ANIMS
		if (_character.isShot == true && _character.facingDir == Character.facing.Left)
		{
			animPlaying = true;
			animSprite.Play("hurt");
			anim.fps = normalFPS;
			StartCoroutine( WaitAndCallback( anim.GetDuration(anim.framesets[2]) ) );
			InvertSprite();
		}
		if (_character.isShot == true && _character.facingDir == Character.facing.Right)
		{
			animPlaying = true;
			animSprite.Play("hurt");
			anim.fps = normalFPS;
			StartCoroutine( WaitAndCallback( anim.GetDuration(anim.framesets[2]) ) );
			NormalScaleSprite();
		}
	}
	private void Fall()
	{
		
	}
	private void Paused()
	{
		if (_player.paused == true)
		{
			currentAnim = animDef.None;
			animSprite.looping = false;
		}
	}

	private void Pebble()
	{
		if(_character.facingDir == Character.facing.Right && _character.grounded && currentAnim!=animDef.PreparePebble && _player.specialCast && _player.preparePebble)
		{
			animPlaying = true;
			currentAnim = animDef.PreparePebble;
			animSprite.Play("preparePebble");
			anim.fps = normalFPS;
			NormalScaleSprite();
		}
		if(_character.facingDir == Character.facing.Left && _character.grounded && currentAnim!=animDef.PreparePebble && _player.specialCast && _player.preparePebble)
		{
			animPlaying = true;
			currentAnim = animDef.PreparePebble;
			animSprite.Play("preparePebble");
			anim.fps = normalFPS;
			InvertSprite();
		}
		if(_character.facingDir == Character.facing.Right && _character.grounded && currentAnim!=animDef.LaunchPebble && _player.specialCast && _player.launchPebble)
		{
			animPlaying = true;
			currentAnim = animDef.LaunchPebble;
			animSprite.Play("launchPebble");
			anim.fps = normalFPS;
			NormalScaleSprite();
			StartCoroutine("WaitAfterPebble");
		}
		if(_character.facingDir == Character.facing.Left && _character.grounded && currentAnim!=animDef.LaunchPebble && _player.specialCast && _player.launchPebble)
		{
			animPlaying = true;
			currentAnim = animDef.LaunchPebble;
			animSprite.Play("launchPebble");
			anim.fps = normalFPS;
			InvertSprite();
			StartCoroutine("WaitAfterPebble");
		}
	}
	IEnumerator WaitAfterPebble() {
		yield return new WaitForSeconds(0.1f);
		animPlaying = _player.launchPebble = _player.specialCast = false;
	}
	private void PlayInstru()
	{
		if(_character.facingDir == Character.facing.Right && _character.grounded && currentAnim!=animDef.TakeInstru && _player.specialCast && _player.takingInstr)
		{
			currentAnim = animDef.TakeInstru;
			animSprite.Play("takeInstru");
			anim.fps = normalFPS;
			NormalScaleSprite();
		}
		if(_character.facingDir == Character.facing.Left && _character.grounded && currentAnim!=animDef.TakeInstru && _player.specialCast && _player.takingInstr)
		{
			currentAnim = animDef.TakeInstru;
			animSprite.Play("takeInstru");
			anim.fps = normalFPS;
			InvertSprite();
		}
		if(_character.facingDir == Character.facing.Right && _character.grounded && currentAnim!=animDef.PlayInstru && _player.specialCast && !_player.takingInstr && !_player.preparePebble && !_player.launchPebble)
		{
			currentAnim = animDef.PlayInstru;
			animSprite.Play("instru");
			anim.fps = normalFPS;
			NormalScaleSprite();
		}
		if(_character.facingDir == Character.facing.Left && _character.grounded && currentAnim!=animDef.PlayInstru && _player.specialCast && !_player.takingInstr && !_player.preparePebble && !_player.launchPebble)
		{
			currentAnim = animDef.PlayInstru;
			animSprite.Play("instru");
			anim.fps = normalFPS;
			InvertSprite();
		}
	}

	private void AnimationFinished()
	{
		animPlaying = false;
	}
	private void InvertSprite()
	{
		spriteParent.localScale = new Vector3(-1,1,1);
	}
	private void NormalScaleSprite()
	{
		spriteParent.localScale = new Vector3(1,1,1);
	}
	IEnumerator WaitAndCallback(float waitTime)
	{
		yield return new WaitForSeconds(waitTime);
		AnimationFinished();
	}
}

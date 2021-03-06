﻿using UnityEngine;
using System.Collections;

public class PlayerAnims : MonoBehaviour 
{
	public enum animDef
	{
		None,
		WalkLeft, WalkRight,
		RunLeft, RunRight,
		RopeLeft, RopeRight,
		JumpLeft, JumpRight,
		Climb, ClimbStop,
		StandLeft, StandRight,
		HangLeft, HangRight,
		FallLeft, FallRight ,
		ShootLeft, ShootRight,
		CrounchLeft, CrounchRight,
		AttackLeft, AttackRight,
		TakeInstru, PlayInstru,
		PreparePebble, LaunchPebble,
		GrabLeft, GrabRight
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
		Fall();
		Grab ();
		Run();
		Walk();
		Stand();
		Crounch();
		Jump();
		Attack();
		Hurt();
		Paused();
		Pebble();
		PlayInstru();
	}
	private void Grab() {
		if(_player.isGrab && currentAnim!=animDef.GrabRight && _character.facingDir == Character.facing.Right /*&& !_player.isFall*/)
		{
			currentAnim = animDef.GrabRight;
			anim.fps = normalFPS;
			animSprite.Play("grab");
			NormalScaleSprite();
		}
		if(_player.isGrab && currentAnim!=animDef.GrabLeft && _character.facingDir == Character.facing.Left /*&& !_player.isFall*/)
		{
			currentAnim = animDef.GrabLeft;
			anim.fps = normalFPS;
			animSprite.Play("grab");
			InvertSprite();
		}
	}
	private void Run()
	{
		if(_character.isRight && _character.grounded && currentAnim!=animDef.RunRight && _player.isSprint && currentAnim!=animDef.GrabRight)
		{
			currentAnim = animDef.RunRight;
			anim.fps = sprintFPS;
			animSprite.Play("run");
			NormalScaleSprite();
		}
		if(_character.isLeft && _character.grounded && currentAnim!=animDef.RunLeft && _player.isSprint && currentAnim!=animDef.GrabLeft)
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
		if(_character.grounded == false && currentAnim != animDef.JumpLeft && _character.facingDir == Character.facing.Left && !_player.isGrab && !_player.isFall)
		{
			currentAnim = animDef.JumpLeft;
			animSprite.Play("jump"); // fall left
			anim.fps = normalFPS;
			InvertSprite();
		}
		if(_character.grounded == false && currentAnim != animDef.JumpRight && _character.facingDir == Character.facing.Right && !_player.isGrab && !_player.isFall)
		{
			currentAnim = animDef.JumpRight;
			animSprite.Play("jump"); // fall right
			anim.fps = normalFPS;
			NormalScaleSprite();
		}
	}
	private void Fall()
	{
		if(_player.isFall && currentAnim!=animDef.FallRight && _character.facingDir == Character.facing.Right)
		{
			currentAnim = animDef.FallRight;
			anim.fps = normalFPS;
			animSprite.Play("fall");
			NormalScaleSprite();
		}
		if(_player.isFall && currentAnim!=animDef.FallLeft && _character.facingDir == Character.facing.Left)
		{
			currentAnim = animDef.FallLeft;
			anim.fps = normalFPS;
			animSprite.Play("fall");
			InvertSprite();
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

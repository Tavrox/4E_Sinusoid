using UnityEngine;
using System.Collections;

public class WalkerAnims : MonoBehaviour 
{
	public enum animDef
	{
		None,
		WalkLeft, WalkRight,
		RopeLeft, RopeRight,
		Climb, ClimbStop,
		StandLeft, StandRight,
		HangLeft, HangRight,
		FallLeft, FallRight ,
		ShootLeft, ShootRight,
		CrounchLeft, CrounchRight,
		AttackLeft, AttackRight,
	}

	
	private OTAnimatingSprite animBody, animMecha, animTail;
	private OTAnimatingSprite[] spritesTab;

	private Transform spriteParentBody, spriteParentMecha, spriteParentTail;
	
	private animDef currentAnim;
	private Character _character;
	private Player _player;
	private Walker _walker;
	
	private bool animPlaying = false;
	
	// Use this for initialization
	void Start () 
	{
		_character 	= GetComponent<Character>();
		_walker = GetComponent<Walker>();
		_player 	= GameObject.FindObjectOfType<Player>();
		spritesTab = gameObject.GetComponentsInChildren<OTAnimatingSprite>();
		foreach (OTAnimatingSprite sprite in spritesTab) {
			if(sprite.name=="walkerBodySprite") animBody = sprite;
			if(sprite.name=="walkerMechaSprite") animMecha = sprite;
			if(sprite.name=="walkerTailSprite") animTail = sprite;
		}
		spriteParentBody = animBody.transform.parent.transform;
		spriteParentMecha = animMecha.transform.parent.transform;
		spriteParentTail = animTail.transform.parent.transform;
		animBody.Play ("standBody");
		animMecha.Play ("standMecha");
		animTail.Play ("standTail");
	}
	void Update() 
	{
		animBody.looping = true;
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
	}
	private void Run()
	{
		if(_character.isRight && _character.grounded && currentAnim!=animDef.WalkRight && !_walker.getEndPFReached())
		{
			currentAnim = animDef.WalkRight;
			animBody.Play ("runBody");
			animMecha.Play ("runMecha");
			animTail.Play ("runTail");
			NormalScaleSprite();;
		}
		if(_character.isLeft && _character.grounded && currentAnim!=animDef.WalkLeft && !_walker.getEndPFReached())
		{
			currentAnim = animDef.WalkLeft;
			animBody.Play ("runBody");
			animMecha.Play ("runMecha");
			animTail.Play ("runTail");
			InvertSprite();
		}
	}
	private void Walk()
	{
		
	}
	private void Stand()
	{	
		if(_walker.getEndPFReached()) {
			currentAnim = animDef.StandLeft;
			animBody.Play ("standBody");
			animMecha.Play ("standMecha");
			animTail.Play ("standTail");
		}
		if(!_character.isLeft && _character.grounded == true && currentAnim != animDef.StandLeft && _character.facingDir == Character.facing.Left && animPlaying == false)
		{
			currentAnim = animDef.StandLeft;
			animMecha.Play ("standMecha");
			if(!_character.isShot) {animBody.Play ("standBody");animTail.Play ("standTail");}
			InvertSprite();
		}
		if(!_character.isRight && _character.grounded && currentAnim != animDef.StandRight && _character.facingDir == Character.facing.Right && animPlaying == false)
		{
			currentAnim = animDef.StandRight;
			animMecha.Play ("standMecha");
			if(!_character.isShot) {animBody.Play ("standBody");animTail.Play ("standTail");}
			NormalScaleSprite();
		}
	}
	private void Crounch()
	{
		if (_character.isCrounch == true)
		{
			currentAnim = animDef.CrounchLeft;
			animBody.Play ("crouchBody");
			animMecha.Play ("crouchMecha");
			animTail.Play ("crouchTail");
		}
	}
	private void Jump()
	{
		if(_character.grounded == false && currentAnim != animDef.FallLeft && _character.facingDir == Character.facing.Left)
		{
			currentAnim = animDef.FallLeft;
			animMecha.Play ("jumpMecha");
			if(!_character.isShot) {animBody.Play ("jumpBody");animTail.Play ("jumpTail");}
			InvertSprite();
		}
		if(_character.grounded == false && currentAnim != animDef.FallRight && _character.facingDir == Character.facing.Right)
		{
			currentAnim = animDef.FallRight;
			animMecha.Play ("jumpMecha");
			if(!_character.isShot) {animBody.Play ("jumpBody");animTail.Play ("jumpTail");}
			NormalScaleSprite();
		}
	}
	private void Attack()
	{
		if(_character.isShot && _character.facingDir == Character.facing.Left) {
			animPlaying = true;
			currentAnim = animDef.ShootRight;
			animBody.Play ("attackBody");
			animTail.Play ("attacktTail");
			InvertSprite();
			StartCoroutine( WaitAndCallback( 1f ) );
		}
		if(_character.isShot && _character.facingDir == Character.facing.Right) {
			
			animPlaying = true;
			currentAnim = animDef.ShootRight;
			animBody.Play ("attackBody");
			animTail.Play ("attacktTail");
			NormalScaleSprite();
			StartCoroutine( WaitAndCallback( 1f ) );
		}
	}
	private void Hurt()
	{
		//ENEMIES SPECIFIC ANIMS
		if (_character.isShot == true && _character.facingDir == Character.facing.Left)
		{
			animPlaying = true;
			animMecha.Play ("hurtMecha");
			if(!_character.isShot) {animBody.Play ("hurtBody");animTail.Play ("hurtTail");}
			StartCoroutine( WaitAndCallback( 1f ) );
			InvertSprite();
		}
		if (_character.isShot == true && _character.facingDir == Character.facing.Right)
		{
			animPlaying = true;
			animMecha.Play ("hurtMecha");
			if(!_character.isShot) {animBody.Play ("hurtBody");animTail.Play ("hurtTail");}
			StartCoroutine( WaitAndCallback( 1f ) );
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
			animBody.looping = false;
			animMecha.looping = false;
			animTail.looping = false;
		}
	}
	
	private void AnimationFinished()
	{
		animPlaying = false;
	}
	private void InvertSprite()
	{
		spriteParentBody.localScale = new Vector3(-1,1,1);
		spriteParentMecha.localScale = new Vector3(-1,1,1);
		spriteParentTail.localScale = new Vector3(-1,1,1);
	}
	private void NormalScaleSprite()
	{
		spriteParentBody.localScale = new Vector3(1,1,1);
		spriteParentMecha.localScale = new Vector3(1,1,1);
		spriteParentTail.localScale = new Vector3(1,1,1);
	}
	IEnumerator WaitAndCallback(float waitTime)
	{
		yield return new WaitForSeconds(waitTime);
		AnimationFinished();
	}
}

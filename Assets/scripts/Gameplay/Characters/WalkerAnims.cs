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
	
	//public Transform spriteParent;
	//public OTAnimatingSprite animSprite;
	//public OTAnimation anim;
	
	private animDef currentAnim;
	private Character _character;
	private Player _player;
	private Walker _walker;
	
	private bool animPlaying = false;

	private OTAnimatingSprite animBody, animTail, animMecha;
	private Transform[] spriteParentsTab;
	
	// Use this for initialization
	void Start () 
	{
		_character 	= GetComponent<Character>();
		_walker = GetComponent<Walker>();
		_player 	= GameObject.FindObjectOfType<Player>();
		//animSprite.Play("stand");

		spriteParentsTab = gameObject.GetComponentsInChildren<Transform>();
		animBody = animMecha = animTail = spriteParentsTab[0].GetComponentInChildren<OTAnimatingSprite>();
		
		foreach (Transform spriteParent in spriteParentsTab) {
			if(spriteParent.name=="spriteParentBody") animBody = spriteParent.GetComponentInChildren<OTAnimatingSprite>();
			if(spriteParent.name=="spriteParentMecha") animMecha = spriteParent.GetComponentInChildren<OTAnimatingSprite>();
			if(spriteParent.name=="spriteParentTail") animTail = spriteParent.GetComponentInChildren<OTAnimatingSprite>();
		}
		animBody.looping = animMecha.looping = animTail.looping = true;
		
		animBody.Play("standBody");
		animMecha.Play("standMecha");
		animTail.Play("standTail");		
	}
	void Update() 
	{
		//animSprite.looping = true;
		// Order of action matters, they need to have priorities. //
		Run();
		//Walk();
		Stand();
		Crounch();
		//Jump();
		Attack();
		//Hurt();
		//Fall();
		Paused();
//		animBody.size.Set(5.599997f,3.599998f);
//		animMecha.size.Set(5.599997f,3.599998f);
//		animTail.size.Set(5.599997f,3.599998f);
//		animBody.gameObject.transform.localScale.Set(5.599997f,3.599998f,0f);
//		animMecha.gameObject.transform.localScale.Set(5.599997f,3.599998f,0f);
//		animTail.gameObject.transform.localScale.Set(5.599997f,3.599998f,0f);
		if(animTail.frameIndex==37) {
			//_walker.collider.enabled = false;
			//_walker.collider.bounds.center.Set(0,0,0);
			//_walker.collider.bounds.size.Set(0,0,0);
			_walker.setBxTailPosition(1.8f, -0.1f);
		}
		else if(animTail.frameIndex==38) {
			_walker.setBxTailPosition(2.3f, -0.15f);
		}
		else if(animTail.frameIndex==45) {
			_walker.setBxTailPosition(1.65f, 0.3f);
		}
		else if(animTail.frameIndex==47) {
			_walker.setBxTailPosition(1.5f, 1.1f);
		}
		else if(animTail.frameIndex==50) {
			_walker.setBxTailPosition(0f, 0f);
		}
		else if(animTail.frameIndex==54) {
			_walker.setAttacking(false);
		}
	}
	private void Run()
	{
		if(_character.isRight && _character.grounded && currentAnim!=animDef.WalkRight && !_walker.getEndPFReached() && !_walker.getAttacking())
		{
			currentAnim = animDef.WalkRight;
			animBody.Play("runBody");
			animMecha.Play("runMecha");
			animTail.Play("runTail");
			NormalScaleSprite();
		}
		if(_character.isLeft && _character.grounded && currentAnim!=animDef.WalkLeft && !_walker.getEndPFReached() && !_walker.getAttacking())
		{
			currentAnim = animDef.WalkLeft;
			animBody.Play("runBody");
			animMecha.Play("runMecha");
			animTail.Play("runTail");
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
			animBody.Play("standBody");
			animMecha.Play("standMecha");
			animTail.Play("standTail");
		}
		if(!_character.isLeft && _character.grounded == true && currentAnim != animDef.StandLeft && _character.facingDir == Character.facing.Left && animPlaying == false)
		{
			currentAnim = animDef.StandLeft;
			animBody.Play("standBody");
			animMecha.Play("standMecha");
			animTail.Play("standTail");
			InvertSprite();
		}
		if(!_character.isRight && _character.grounded && currentAnim != animDef.StandRight && _character.facingDir == Character.facing.Right && animPlaying == false)
		{
			currentAnim = animDef.StandRight;
			animBody.Play("standBody");
			animMecha.Play("standMecha");
			animTail.Play("standTail");
			NormalScaleSprite();
		}
	}
	private void Crounch()
	{
		if (_character.isCrounch == true)
		{
			currentAnim = animDef.CrounchLeft;
			//animSprite.Play("crounch");
		}
	}
	private void Jump()
	{
		if(_character.grounded == false && currentAnim != animDef.FallLeft && _character.facingDir == Character.facing.Left && !_walker.getAttacking())
		{
			currentAnim = animDef.FallLeft;
			animBody.Play("standBody");
			animMecha.Play("standMecha");
			animTail.Play("standTail");
			InvertSprite();
		}
		if(_character.grounded == false && currentAnim != animDef.FallRight && _character.facingDir == Character.facing.Right && !_walker.getAttacking())
		{
			currentAnim = animDef.FallRight;
			animBody.Play("standBody");
			animMecha.Play("standMecha");
			animTail.Play("standTail");
			NormalScaleSprite();
		}
	}
	private void Attack()
	{
		if (_walker.getAttacking() == true && currentAnim != animDef.ShootLeft && _character.facingDir == Character.facing.Left)
		{
			animPlaying = true;
			currentAnim = animDef.ShootLeft;
			animBody.Play("attackBody");
			animMecha.Play("runMecha");
			animTail.Play("attackTail");
			InvertSprite();
			//StartCoroutine( WaitAndCallback( anim.GetDuration(anim.framesets[3]) ) );
		}
		if (_walker.getAttacking() == true && currentAnim != animDef.ShootRight && _character.facingDir == Character.facing.Right)
		{
			animPlaying = true;
			currentAnim = animDef.ShootRight;
			animBody.Play("attackBody");
			animMecha.Play("runMecha");
			animTail.Play("attackTail");
			NormalScaleSprite();
			//StartCoroutine( WaitAndCallback( anim.GetDuration(anim.framesets[3]) ) );
		}
	}
	private void Hurt()
	{
		//ENEMIES SPECIFIC ANIMS
		/*if (_character.isShot == true && _character.facingDir == Character.facing.Left)
		{
			animPlaying = true;
			animSprite.Play("hurt");
			StartCoroutine( WaitAndCallback( anim.GetDuration(anim.framesets[2]) ) );
			InvertSprite();
		}
		if (_character.isShot == true && _character.facingDir == Character.facing.Right)
		{
			animPlaying = true;
			animSprite.Play("hurt");
			StartCoroutine( WaitAndCallback( anim.GetDuration(anim.framesets[2]) ) );
			NormalScaleSprite();
		}*/
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
		//spriteParent.localScale = new Vector3(-1,1,1);
		foreach (Transform spriteParent in spriteParentsTab) {
			if(spriteParent.name=="spriteParentBody" || spriteParent.name=="spriteParentMecha" || spriteParent.name=="spriteParentTail") spriteParent.localScale = new Vector3(-1,1,1);
		}
	}
	private void NormalScaleSprite()
	{
		//spriteParent.localScale = new Vector3(1,1,1);
		foreach (Transform spriteParent in spriteParentsTab) {
			if(spriteParent.name=="spriteParentBody" || spriteParent.name=="spriteParentMecha" || spriteParent.name=="spriteParentTail") spriteParent.localScale = new Vector3(1,1,1);
		}
	}
	IEnumerator WaitAndCallback(float waitTime)
	{
		yield return new WaitForSeconds(waitTime);
		AnimationFinished();
	}
}

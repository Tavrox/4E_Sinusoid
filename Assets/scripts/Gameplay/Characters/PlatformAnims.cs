using UnityEngine;
using System.Collections;

public class PlatformAnims : MonoBehaviour 
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
		Destroy,
	}
	
//	public Transform spriteParent;
	public OTAnimatingSprite animSprite;
//	public OTAnimation anim;
	
	private animDef currentAnim;
//	private Character _character;
//	private Player _player;
	
	private bool animPlaying = false;
	private bool defaultState =true;
	private BoxCollider[] colliderChild;

	// Use this for initialization
//	void Start () 
//	{
//		_character 	= GetComponent<Character>();
//		_player 	= GetComponent<Player>();
//	}
//	void Update() 
//	{
//		//animSprite.looping = true;
//		// Order of action matters, they need to have priorities. //
//	}
	public void destroyAnim() {
		animSprite.Play("destroy");
		currentAnim = animDef.Destroy;
		collider.enabled = false;
		colliderChild = gameObject.GetComponentsInChildren<BoxCollider>();
		foreach(BoxCollider coll in colliderChild) {
			coll.collider.enabled = false;
		}
		defaultState =false;
	}
	
//	private void AnimationFinished()
//	{
//	    animPlaying = false;
//	}
//	private void InvertSprite()
//	{
//		spriteParent.localScale = new Vector3(-1,1,1);
//	}
//	private void NormalScaleSprite()
//	{
//		spriteParent.localScale = new Vector3(1,1,1);
//	}
//	IEnumerator WaitAndCallback(float waitTime)
//	{
//	    yield return new WaitForSeconds(waitTime);
//	    AnimationFinished();
//	}
	void OnTriggerEnter(Collider other) {
		if(other.name=="Player" || other.name=="Pebble(Clone)") {
			if(defaultState) destroyAnim();
		}
//		print ("TOUCHE");
//		print (other.name);
	}
}

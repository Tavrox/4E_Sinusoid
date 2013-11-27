using UnityEngine;
using System.Collections;

public class PlayerAnims : MonoBehaviour {

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
	private animDef currentAnim;

	private Transform spriteParent;
	private OTAnimatingSprite animSprite;
	private OTAnimation anim;
	private Player _player;

	// Use this for initialization
	void Start () {
	
		setup();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void setup()
	{
		_player = this.GetComponent<Player>();
//		spriteParent = _player.GetComponentInChildren<Transform>
	}
}

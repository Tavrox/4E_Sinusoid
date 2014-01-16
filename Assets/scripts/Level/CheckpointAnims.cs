using UnityEngine;
using System.Collections;


public class CheckpointAnims : MonoBehaviour {
	
	public enum animDef
	{
		None,
		Waterfall,
		Water,
		WaterLeft,
		WaterRight,
		Ground,
		GroundLeft,
		GroundRight
	}

	public Transform spriteParent;
	public OTAnimatingSprite animSprite;
	public OTAnimation anim;
	public animDef currentAnim;

	// Use this for initialization
	void Start () {
		playAnimDef();
	}
	
	// Update is called once per frame
	void Update () {
		animSprite.looping = true;
	}

	void playAnimDef () {
		switch(currentAnim.ToString()) 
		{
		case "Waterfall" :
			//if(currentAnim != animDef.Waterfall) {
				currentAnim = animDef.Waterfall;
				animSprite.Play("waterfall");
			//}
			break;
		case "Water" :
			//if(currentAnim != animDef.Water) {
				currentAnim = animDef.Water;
				animSprite.Play("water");
			//}
			break;
		case "WaterLeft" :
			//if(currentAnim != animDef.WaterLeft) {
				currentAnim = animDef.WaterLeft;
				animSprite.Play("waterL");
			//}
			break;
		case "WaterRight" :
			//if(currentAnim != animDef.WaterRight) {
				currentAnim = animDef.WaterRight;
				animSprite.Play("waterR");
			//}
			break;
		case "Ground" :
			//if(currentAnim != animDef.Ground) {
				currentAnim = animDef.Ground;
				animSprite.Play("ground");
			//}
			break;
		case "GroundLeft" :
			//if(currentAnim != animDef.GroundLeft) {
				currentAnim = animDef.GroundLeft;
				animSprite.Play("groundL");
			//}
			break;
		case "GroundRight" :
			//if(currentAnim != animDef.GroundRight) {
				currentAnim = animDef.GroundRight;
				animSprite.Play("groundR");
			//}
			break;
		}
	}
}

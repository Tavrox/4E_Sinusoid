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
	
	}
	
	// Update is called once per frame
	void Update () {
		animSprite.looping = true;
		playAnimDef();
	}

	void playAnimDef () {
		switch(currentAnim.ToString()) 
		{
		case "Waterfall" :
			animSprite.Play("waterfall");
			break;
		case "Water" :
			animSprite.Play("water");
			break;
		case "WaterLeft" :
			animSprite.Play("waterL");
			break;
		case "WaterRight" :
			animSprite.Play("waterR");
			break;
		case "Ground" :
			animSprite.Play("waterground");
			break;
		case "GroundLeft" :
			animSprite.Play("watergroundL");
			break;
		case "GroundRight" :
			animSprite.Play("watergroundR");
			break;
		}
	}
}

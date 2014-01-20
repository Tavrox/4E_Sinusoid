using UnityEngine;
using System.Collections;


public class CheckpointAnims : MonoBehaviour {
		
	private OTAnimatingSprite animWaterfall, animWater, animGround;
	private OTAnimatingSprite[] spritesTab;

	// Use this for initialization
	void Start () {
		//playAnimDef();
		spritesTab = gameObject.GetComponentsInChildren<OTAnimatingSprite>();
		//animWaterfall = animWater = animGround = spritesTab[0];

		foreach (OTAnimatingSprite sprite in spritesTab) {
			if(sprite.name=="spriteCPWaterFall") animWaterfall = sprite;
			if(sprite.name=="spriteCPWater") animWater = sprite;
			if(sprite.name=="spriteCPGround") animGround = sprite;
		}
		animWaterfall.Play("waterfall");
		animWater.Play("water");
		animGround.Play("ground");
		animWaterfall.looping = animWater.looping = animGround.looping = true;
	}
}

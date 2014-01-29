using UnityEngine;
using System.Collections;


public class CheckpointAnims : MonoBehaviour {
		
	private OTAnimatingSprite animWaterfall, animWater, animGround;
	private Transform[] spriteParentsTab;

	// Use this for initialization
	void Start () {
		//playAnimDef();
		spriteParentsTab = gameObject.GetComponentsInChildren<Transform>();
		animWaterfall = animWater = animGround = spriteParentsTab[0].GetComponentInChildren<OTAnimatingSprite>();

		foreach (Transform spriteParent in spriteParentsTab) {
			if(spriteParent.name=="spriteParentCPWaterFall") animWaterfall = spriteParent.GetComponentInChildren<OTAnimatingSprite>();
			if(spriteParent.name=="spriteParentCPWater") animWater = spriteParent.GetComponentInChildren<OTAnimatingSprite>();
			if(spriteParent.name=="spriteParentCPGround") animGround = spriteParent.GetComponentInChildren<OTAnimatingSprite>();
		}
		animWaterfall.Play("waterfall");
		animWater.Play("water");
		animGround.Play("ground");
		animWaterfall.looping = animWater.looping = animGround.looping = true;
	}
}

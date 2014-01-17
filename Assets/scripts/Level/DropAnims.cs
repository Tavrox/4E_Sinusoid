using UnityEngine;
using System.Collections;


public class DropAnims : MonoBehaviour {
	
	public enum animDefDrop
	{
		None,
		DropBase,
		Drop
	}

	public OTAnimatingSprite animSprite;
	public animDefDrop currentAnimDrop;


	// Use this for initialization
	void Start () {
		//playAnim();
		//if(currentAnimDrop.ToString()=="DropBase") {animSprite.Play("dropbase");/*animSprite.looping = true;*/}
		//else {animSprite.frameIndex = 62;animSprite.Stop();}
	}
	// Update is called once per frame
	void Update () {

	}
}

using UnityEngine;
using System.Collections;

public class AnimatedBtn : MonoBehaviour {

	private string btnAnim;

	// Use this for initialization
	void Start () {

		btnAnim = "over";
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnMouseOver()
	{
		OTAnimatingSprite anim = GetComponent<OTAnimatingSprite>();
		anim.Play(btnAnim);
	}
}

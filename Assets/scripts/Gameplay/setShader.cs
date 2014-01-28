using UnityEngine;
using System.Collections;

public class setShader : MonoBehaviour {
	public Shader shader1;
	public bool isAura;
	public bool isBackground;
	// Use this for initialization
	void Start () {
		shader1 = Shader.Find("Mobile/Particles/Alpha Blended");
		if(isBackground) {
			shader1 = Shader.Find("custom/MyBlendedAlphaBG");
		}
		gameObject.renderer.material.shader =shader1;

		if (isAura)
		{
			//gameObject.renderer.material.color = new Color(gameObject.renderer.material.color.r, gameObject.renderer.material.color.g, gameObject.renderer.material.color.b, 0f) ;
			print (gameObject.renderer.material.color);
		}
	}
}

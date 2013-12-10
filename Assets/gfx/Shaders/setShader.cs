using UnityEngine;
using System.Collections;

public class setShader : MonoBehaviour {
	public Shader shader1;
	// Use this for initialization
	void Start () {
		shader1 = Shader.Find("Mobile/Particles/Alpha Blended");
		gameObject.renderer.material.shader =shader1;
	}
}

using UnityEngine;
using System.Collections;

public class DropEmitter : MonoBehaviour {


	public FESound DestroySound;

	void OnTriggerEnter (Collider other) {

		if (other.CompareTag("soundStopper") == true)
		{
//			if (other.getComponent<Environment>().typeList == Environment.types.Stone)
//			{
			playDestroy(DestroySound);
//			}
		}
	
	}


	private void playDestroy(FESound _sound)
	{
		DestroySound.playSound();
	}
}

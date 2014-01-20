using UnityEngine;
using System.Collections;

public class FESound : MonoBehaviour {

	public MasterAudioGroup SoundGroup;
	[Range (0,10f)] public float Delay = 0f;
	[Range (0,1f)] public float Volume = 1f;
	[Range (0,1f)] public float Pitch = 1f;
	public float RepeatRate = 0.6f;

	public void playSound()
	{
		MasterAudio.PlaySound(SoundGroup.name, Volume, Pitch, Delay);
	}
	public void playSound(Environment _enviro)
	{
		MasterAudio.PlaySound(SoundGroup.name + "_" + _enviro.typeList.ToString(), Volume, Pitch, Delay);
	}
	public void playVariationSounds(Environment _enviro)
	{
		MasterAudio.PlaySound(SoundGroup.name, Volume, Pitch, Delay, SoundGroup.name + "_" + _enviro.typeList.ToString() );
	}
	public void playModulatedSound(float _var1, float _var2)
	{
		float percent = (_var1 / _var2);	
		Volume = percent;
		MasterAudio.PlaySound(SoundGroup.name, Volume, Pitch, Delay);
	}
	public void playLeftSound(Environment _enviro)
	{
		MasterAudio.PlaySound(SoundGroup.name + "_" + _enviro.typeList.ToString() + "L", Volume, Pitch, Delay);
<<<<<<< HEAD
		//Debug.Log(SoundGroup.name + "_" + _enviro.typeList.ToString() + "L" );
=======
//		Debug.Log(SoundGroup.name + "_" + _enviro.typeList.ToString() + "L" );
>>>>>>> 502ae1953e671b342a59b9f4ff8e38743981b712
	}
	public void playRightSound(Environment _enviro)
	{
		MasterAudio.PlaySound(SoundGroup.name + "_" + _enviro.typeList.ToString() + "R" , Volume, Pitch, Delay);
<<<<<<< HEAD
		//Debug.Log(SoundGroup.name + "_" + _enviro.typeList.ToString() + "R" );
=======
//		Debug.Log(SoundGroup.name + "_" + _enviro.typeList.ToString() + "R" );
>>>>>>> 502ae1953e671b342a59b9f4ff8e38743981b712
	}
	public void playDistancedSound(Transform _obj1, Transform _obj2)
	{
		Vector2 pos1 = new Vector2(_obj1.position.x, _obj1.position.y);
		Vector2 pos2 = new Vector2(_obj2.position.x, _obj2.position.y);
		float res = Vector2.Distance(pos1, pos2);
		//Debug.Log ("Distance Sound" + res);
//		MasterAudio.PlaySound(SoundGroup.name, Volume, Pitch, Delay);
	}
}
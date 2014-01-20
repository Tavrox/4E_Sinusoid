using UnityEngine;
using System.Collections;

public class FESound : MonoBehaviour {

	public MasterAudioGroup SoundGroup;
	[Range (0,10f)] public float Delay;
	[Range (0,1f)] public float Volume;
	[Range (0,1f)] public float Pitch;
	public enum SoundType {Oneshot, Continuous};
	public SoundType type;
	public float RepeatRate = 0.6f;

	public void playSound()
	{
		MasterAudio.PlaySound(SoundGroup.name, Volume, Pitch, Delay);
	}
	public void playSound(Environment _enviro)
	{
		MasterAudio.PlaySound(SoundGroup.name, Volume, Pitch, Delay, SoundGroup.name + "_" + _enviro.typeList.ToString() );
	}
	public void playModulatedSound(float _var1, float _var2)
	{
		float percent = (_var1 / _var2);	
		Volume = percent;
		MasterAudio.PlaySound(SoundGroup.name, Volume, Pitch, Delay);
	}
	public void playDistancedSound(Transform _obj1, Transform _obj2)
	{
		Vector2 pos1 = new Vector2(_obj1.position.x, _obj1.position.y);
		Vector2 pos2 = new Vector2(_obj2.position.x, _obj2.position.y);
		float res = Vector2.Distance(pos1, pos2);
		Debug.Log ("Distance Sound" + res);
//		MasterAudio.PlaySound(SoundGroup.name, Volume, Pitch, Delay);
	}
}
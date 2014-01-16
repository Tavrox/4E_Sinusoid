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
	public void playSound(Environment.types _enviro)
	{
		MasterAudio.PlaySound(SoundGroup.name, Volume, Pitch, Delay, SoundGroup.name + "_" + _enviro.ToString() );
	}
	public void playModulatedSound(float _var1, float _var2)
	{
		float percent = (_var1 / _var2);	
		Volume = percent;
		MasterAudio.PlaySound(SoundGroup.name, Volume, Pitch, Delay);
	}

}
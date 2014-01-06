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

}
using UnityEngine;
using System.Collections;

public class FESound : MonoBehaviour {

	public MasterAudioGroup SoundGroup;
	[Range (0,10f)] public float Delay = 0f;
	[Range (0,1f)] public float Volume = 1f;
	[Range (0,1f)] public float Pitch = 1f;
	public float RepeatRate = 0.6f;
	private float distanceToPlayer = 0f;
	public float distanceForFadeOut = 5f;
	private Transform referralDistance;
	private Transform distToTrack;

	void Start()
	{
		if (SoundGroup == null)
		{
			Debug.LogWarning("the sound group " + gameObject.transform.parent.transform.parent.gameObject.name + "/" +  gameObject.transform.parent.gameObject.name + "/" + gameObject.name + " hasn't been attributed");
		}
	}

	public void playSound()
	{
		if (SoundGroup != null)
		{
			MasterAudio.PlaySound(SoundGroup.name, Volume, Pitch, Delay);
		}
	}
	public void playSound(Environment _enviro)
	{
		if (SoundGroup != null)
		{
			MasterAudio.PlaySound(SoundGroup.name + "_" + _enviro.typeList.ToString(), Volume, Pitch, Delay);
		}
	}
	public void playVariationSound(Environment _enviro)
	{
		if (SoundGroup != null)
		{
			MasterAudio.PlaySound(SoundGroup.name, Volume, Pitch, Delay, SoundGroup.name + "_" + _enviro.typeList.ToString() );
		}
	}
	public void playVariationSound(string _variation)
	{
		if (SoundGroup != null)
		{
			MasterAudio.PlaySound(SoundGroup.name, Volume, Pitch, Delay, SoundGroup.name + "_" + _variation);
		}
	}
	public void playModulatedSound(float _var1, float _var2)
	{
		if (SoundGroup != null)
		{
			float percent = (_var1 / _var2);	
			Volume = percent;
			MasterAudio.PlaySound(SoundGroup.name, Volume, Pitch, Delay);
		}
	}
	public void playLeftSound(Environment _enviro)
	{
		if (_enviro != null && SoundGroup != null)
		{
			PlaySoundResult _psr = MasterAudio.PlaySound(SoundGroup.name + "_" + _enviro.typeList.ToString() + "L", Volume, Pitch, Delay);
		}
//		Debug.Log(_psr.ActingVariation);
	}
	public void playRightSound(Environment _enviro)
	{
		if (_enviro != null && SoundGroup != null)
		{
			PlaySoundResult _psr = MasterAudio.PlaySound(SoundGroup.name + "_" + _enviro.typeList.ToString() + "R" , Volume, Pitch, Delay);
		}
//		Debug.Log(_psr.ActingVariation);
	}
	public void playDistancedSound(string _var = null)
	{
		referralDistance = this.gameObject.transform;
		distToTrack = GameObject.FindGameObjectWithTag("Player").transform;
		if (_var == null)
		{
			MasterAudio.PlaySound(SoundGroup.name, Volume, Pitch, Delay);
		}
		else
		{
			MasterAudio.PlaySound(SoundGroup.name, Volume, Pitch, Delay, SoundGroup.name + "_" + _var);
		}
		InvokeRepeating("checkDistance", 0f, 0.1f); 
	}

	public void stopSound()
	{
		MasterAudio.StopAllOfSound(SoundGroup.name);
	}

	private void checkDistance()
	{
//		Debug.DrawLine(gameObject.transform.position, distToTrack.position, Color.blue);
//		Debug.DrawLine(gameObject.transform.position, referralDistance.position, Color.black);
		// WORK IN PROGRESS
		Vector2 thisObjPos = new Vector2 (gameObject.transform.position.x, gameObject.transform.position.y);
		Vector2 referralPos = new Vector2 (referralDistance.transform.position.x, referralDistance.transform.position.y);
		Vector2 posToTrack = new Vector2 (distToTrack.position.x, distToTrack.position.y);
//		distanceToPlayer = (Vector2.Distance(thisObjPos, referralPos)) / (Vector2.Distance(thisObjPos, posToTrack )) ;
//		distanceToPlayer = (Vector2.Distance(thisObjPos, posToTrack )) / (Vector2.Distance(thisObjPos, referralPos)) ;
		distanceToPlayer = Vector2.Distance(thisObjPos, posToTrack );
		if (distanceToPlayer < 15f)
		{
			MasterAudio.FadeSoundGroupToVolume(SoundGroup.name, 1f, distanceForFadeOut);
		}
		else
		{
			MasterAudio.FadeSoundGroupToVolume(SoundGroup.name, 0f, distanceForFadeOut);
		}
//		Debug.Log(gameObject.name + thisObjPos);
//		Debug.Log(referralDistance.name + referralPos);
//		Debug.Log("PosToTrack" + posToTrack);
//		Debug.Log("ObjPos>>ReferralPos" + Vector2.Distance(thisObjPos, referralPos));
//		Debug.Log("ObjPos>>Trackpos" + Vector2.Distance(thisObjPos, posToTrack));
//		Debug.Log("Ratio" + distanceToPlayer);
	}












}
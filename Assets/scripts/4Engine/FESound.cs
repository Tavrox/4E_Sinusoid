﻿using UnityEngine;
using System.Collections;

public class FESound : MonoBehaviour {

	public MasterAudioGroup SoundGroup;
	[Range (0,10f)] public float Delay = 0f;
	[Range (0,1f)] public float Volume = 1f;
	[Range (0,1f)] public float Pitch = 1f;
	public float RepeatRate = 0.6f;
	private float distanceToPlayer = 0f;
	private Transform referralDistance;
	private Transform distToTrack;

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
		PlaySoundResult _psr = MasterAudio.PlaySound(SoundGroup.name + "_" + _enviro.typeList.ToString() + "L", Volume, Pitch, Delay);
//		Debug.Log(_psr.ActingVariation);
	}
	public void playRightSound(Environment _enviro)
	{
		PlaySoundResult _psr = MasterAudio.PlaySound(SoundGroup.name + "_" + _enviro.typeList.ToString() + "R" , Volume, Pitch, Delay);
//		Debug.Log(_psr.ActingVariation);
	}
	public void playDistancedSound()
	{
		referralDistance = GetComponentInChildren<Transform>();
		distToTrack = GameObject.FindGameObjectWithTag("Player").transform;
		InvokeRepeating("checkDistance", 0f, 0.5f); 
//		MasterAudio.PlaySound(SoundGroup.name, Volume, Pitch, Delay);
	}

	private void checkDistance()
	{
		Debug.DrawLine(gameObject.transform.position, distToTrack.position);
		Debug.DrawLine(gameObject.transform.position, referralDistance.position);
		// WORK IN PROGRESS
		Vector2 thisObjPos = new Vector2 (gameObject.transform.position.x, gameObject.transform.position.y);
		Vector2 referralPos = new Vector2 (referralDistance.position.x, referralDistance.position.y);
		Vector2 posToTrack = new Vector2 (distToTrack.position.x, distToTrack.position.y);
		distanceToPlayer = Vector2.Distance(thisObjPos, posToTrack);
	}
}
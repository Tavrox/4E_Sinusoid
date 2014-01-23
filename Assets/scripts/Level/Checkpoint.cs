using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour {

	private Player _player;
	private LevelManager _LevMan;
	public bool isActivated;

	public FESound RespawnSound;
	public FESound ReloadSound;
	public FESound IdleSound;

	// Use this for initialization
	void Start () {
		_player = GameObject.FindWithTag("Player").GetComponent<Player>();
		_LevMan = GameObject.FindWithTag("LevelManager").GetComponent<LevelManager>();
		if (IdleSound != null)
		{
			IdleSound.playDistancedSound();
		}

	}

	void OnTriggerEnter(Collider other) 
	{
		if(other.gameObject.CompareTag("Player") && isActivated != true) 
		{
			_player.spawnPos =  new Vector3(transform.position.x,transform.position.y,_player.transform.position.z);
			isActivated = true;
			_LevMan.setActivatedCheckpoint(this);
		}
	}

	public void playSoundRespawn()
	{
		RespawnSound.playSound();
	}
	public void playSoundReload()
	{
		ReloadSound.playSound();
	}
	public void playSoundIdle()
	{
		IdleSound.playDistancedSound();
	}



}

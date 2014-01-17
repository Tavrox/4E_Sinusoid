using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour {

	private Player _player;
	private LevelManager _LevMan;
	public bool isActivated;

	public FESound RespawnSound;
	public FESound ReloadSound;

	// Use this for initialization
	void Start () {
		_player = GameObject.FindWithTag("Player").GetComponent<Player>();
		_LevMan = GameObject.FindWithTag("LevelManager").GetComponent<LevelManager>();

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
		ReloadSound.playSound();
	}
	public void playSoundReload()
	{
		ReloadSound.playSound();
	}



}

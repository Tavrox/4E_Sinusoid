using UnityEngine;
using System.Collections;

public class Drop : MonoBehaviour {
	
	private Vector2 vectorMove, vectorFixed;
	public GameObject DropBase, DropEmitter;
	public float fallSpeed,delayRestart,delayB4Start;
	private Vector3 spawnPos;

	private GameObject GOinstFootWave, DropMask;
	private WaveCreator soundEmitt1,soundEmitt2;
	private int cptWave=1;
	private bool blockCoroutine, first;

	public FESound _DropSound;

	// Use this for initialization
	void Start () {
		GOinstFootWave = Instantiate(Resources.Load("Prefabs/04Gameplay/SoundWavesDrop")) as GameObject;
		soundEmitt1 = GOinstFootWave.GetComponent<WaveCreator>();soundEmitt1.gameObject.name = "_dropWave1";//footsteps wave 1
		soundEmitt1.gameObject.transform.parent = GameObject.Find("Level/Waves/").transform;
		soundEmitt1.createCircle(transform);soundEmitt1.setParent(transform);
		GOinstFootWave = Instantiate(Resources.Load("Prefabs/04Gameplay/SoundWavesDrop")) as GameObject;
		soundEmitt2 = GOinstFootWave.GetComponent<WaveCreator>();soundEmitt2.gameObject.name = "_dropWave2";//footsteps wave 2
		soundEmitt2.gameObject.transform.parent = GameObject.Find("Level/Waves/").transform;
		soundEmitt2.createCircle(transform);soundEmitt2.setParent(transform);

		DropMask = gameObject.transform.FindChild("SoundWaveEltMask").gameObject as GameObject;

		spawnPos = transform.position;
		vectorMove = new Vector2(0f, -fallSpeed);
		DropEmitter.renderer.enabled = DropMask.renderer.enabled = false;
		StartCoroutine("waitB4Start");
	}
	
	// Update is called once per frame
	void Update () {
		if(DropBase.GetComponent<DropAnims>().animSprite.frameIndex==42) {
			DropEmitter.renderer.enabled = DropMask.renderer.enabled = true;
			StartCoroutine("dropUpdate");
		}
		if(DropEmitter.GetComponent<DropAnims>().animSprite.frameIndex==53) 
			StartCoroutine("delayRestartDrop");
	}
	IEnumerator waitB4Start() {
		yield return new WaitForSeconds(delayB4Start);
		DropEmitter.GetComponent<DropAnims>().animSprite.frameIndex = 62;
		DropBase.GetComponent<DropAnims>().animSprite.Play("dropbase");
	}
	IEnumerator delayRestartDrop () {
		yield return new WaitForSeconds(delayRestart);
		DropEmitter.renderer.enabled = DropMask.renderer.enabled = false;
		transform.position = spawnPos;
		DropEmitter.GetComponent<DropAnims>().animSprite.frameIndex=62;
		DropBase.GetComponent<DropAnims>().animSprite.Play("dropbase");
	}
	IEnumerator dropUpdate() {
		yield return new WaitForSeconds(0.02f);
		UpdateMovement();
		StartCoroutine("dropUpdate");
	}

	void UpdateMovement() {
		vectorFixed = vectorMove * Time.deltaTime;
		transform.position += new Vector3(0f,vectorFixed.y,0f);
	}
	void OnTriggerEnter(Collider other) {
		if(other.gameObject.CompareTag("soundStopper")) {
			StopCoroutine("dropUpdate");
			DropEmitter.GetComponent<DropAnims>().animSprite.PlayBackward("drop");
			wave();
//			if (other.gameObject.GetComponent<Environment>() == null)
//			{
//				Debug.Log("There's no environment comp on the object");
//			}
//			else
//			{
//				Environment _env = other.gameObject.GetComponent<Environment>();
			_DropSound.playDistancedSound();
//			}
		}
	}

	private void playSoundDrop () {
		_DropSound.playSound();
	}
	void wave()
	{
		if(cptWave == 1) {cptWave++;soundEmitt1.resetCircle(transform.localScale.x/1.5f,false, false);}
		else if (cptWave == 2) {cptWave=1;soundEmitt2.resetCircle(transform.localScale.x/1.5f,false, false);}
	}

}

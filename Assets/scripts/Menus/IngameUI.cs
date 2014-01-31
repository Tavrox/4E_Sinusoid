using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IngameUI : MonoBehaviour {
	
	public Dictionary<string,GameObject> subUIObjects = new Dictionary<string, GameObject>();

	void Start () 
	{
		GameEventManager.GameStart += GameStart;
		GameEventManager.GameOver += GameOver;
		GameEventManager.GamePause += GamePause;
		GameEventManager.GameUnpause += GameUnpause;
		GameEventManager.Respawn += Respawn;

		subUIObjects.Add("Death", FETool.findWithinChildren(this.gameObject, "Death"));
		subUIObjects.Add("Pause", FETool.findWithinChildren(this.gameObject, "Pause"));
		subUIObjects.Add("Ingame", FETool.findWithinChildren(this.gameObject, "Ingame"));
	}

	private void GameStart () 
	{
		// Désactiver le GameOver	
	}
	private void GameOver () 
	{
		subUIObjects["Death"].GetComponent<SubUI>().revealSub();
		subUIObjects["Ingame"].GetComponent<SubUI>().hideSub();
		// Afficher le gameover
	}
	private void GamePause()
	{
		subUIObjects["Pause"].GetComponent<SubUI>().revealSub();
		subUIObjects["Death"].GetComponent<SubUI>().hideSub();
		subUIObjects["Ingame"].GetComponent<SubUI>().hideSub();
	}
	private void GameUnpause()
	{
		subUIObjects["Pause"].GetComponent<SubUI>().hideSub();
		subUIObjects["Death"].GetComponent<SubUI>().hideSub();
		subUIObjects["Ingame"].GetComponent<SubUI>().revealSub();
	}
	private void Respawn()
	{
		subUIObjects["Death"].GetComponent<SubUI>().hideSub();
		subUIObjects["Ingame"].GetComponent<SubUI>().revealSub();
	}
}

using UnityEngine;
using System.Collections;

public class IngameUI : MonoBehaviour {


	void Start () 
	{
		GameEventManager.GameStart += GameStart;
		GameEventManager.GameOver += GameOver;
		GameEventManager.GamePause += GamePause;
		GameEventManager.GameUnpause += GameUnpause;
	}
	
	// Update is called once per frame
	void Update () 
	{

	}

	private void GameStart () 
	{
		// Désactiver le GameOver	
	}
	private void GameOver () 
	{
		// Afficher le gameover
	}
	private void GamePause()
	{
		// Afficher le menu
		
	}
	private void GameUnpause()
	{
		// Désactiver le menu
	
	}
}

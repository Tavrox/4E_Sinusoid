using UnityEngine;
using System.Collections;

public static class GameEventManager {

	public delegate void GameEvent();
	
	public static event GameEvent GameStart, GamePause, GameUnpause, GameOver, NextLevel, PreviousLevel, Respawn;
	public enum GameState
	{
		Live,
		GameOver,
		Pause
	};
	public static GameState state = GameState.Live;
	
	public static void TriggerGameStart()
	{
		if(GameStart != null)
		{
			Debug.LogWarning("GAMESTART");
			state = GameState.Live;
			GameStart();
		}
	}

	public static void TriggerGameOver(){
		if(GameOver != null && state != GameState.GameOver)
		{
			Debug.LogWarning("GAMEOVER");
			state = GameState.GameOver;
			GameOver();
		}
	}
	
	public static void TriggerNextLevel(){
		if(NextLevel != null){
			NextLevel();
		}
	}
	public static void TriggerPreviousLevel(){
		if(PreviousLevel != null){
			PreviousLevel();
		}
	}
	public static void TriggerGamePause()
	{
		if(GamePause != null && state != GameState.Pause)
		{
			Debug.LogWarning("PAUSE");
			state = GameState.Pause;
			GamePause();
		}
	}
	public static void TriggerGameUnpause()
	{
		if(GameUnpause != null)
		{
			Debug.LogWarning("UNPAUSE");
			state = GameState.Live;
			GameUnpause();
		}
	}
	public static void TriggerRespawn()
	{
		if(Respawn != null)
		{
			Debug.LogWarning("RESPAWN");
			state = GameState.Live;
			Respawn();
		}
	}
}

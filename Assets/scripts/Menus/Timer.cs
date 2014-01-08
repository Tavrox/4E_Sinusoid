using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour {

	public int posX;
	public int posY;
	public int secLeft = 60;
	public int microSecLeft = 99;
	public GUISkin _skinTimer;
	public Color _colSafe;
	public Color _colorWarning;
	public Color _colCritical;
	private bool pauseTimer = false;

	public bool triggeredEnd = false;


	// Use this for initialization
	void Start () {
		InvokeRepeating("updateTimer", 0, 0.01f);

		GameEventManager.GameStart += GameStart;
		GameEventManager.GameOver += GameOver;
		GameEventManager.GamePause += GamePause;
		GameEventManager.GameUnpause += GameUnpause;
	}
	
	private void updateTimer()
	{
		if (pauseTimer != true)
		{
			microSecLeft -= 1;

			if (secLeft < 60)
			{
				_skinTimer.label.normal.textColor = _colSafe;
			}
			if (secLeft < 30)
			{
				_skinTimer.label.normal.textColor = _colorWarning;
			}
			if (secLeft < 15)
			{
				_skinTimer.label.normal.textColor = _colCritical;
			}

			if (microSecLeft == 0)
			{
				secLeft -= 1 ;
				microSecLeft = 59;
			}

			if (secLeft <= 0 && triggeredEnd == false)
			{
				GameEventManager.TriggerGameOver();
				triggeredEnd = true;
				secLeft = 60;
			}
		}
	}

	public void resetTimer()
	{
			secLeft = 60;
			microSecLeft = 60;
	}

	private void GameStart()
	{
		pauseTimer = false;
		triggeredEnd = false;
		resetTimer();
	}
	private void GamePause()
	{
		pauseTimer = true;
	}
	private void GameUnpause()
	{
		pauseTimer = false;
	}
	
	private void GameOver()
	{
		pauseTimer = true;
	}

	private void OnGUI()
	{
		GUI.skin = _skinTimer;
		GUI.Label(new Rect(posX, posY, 400, 400), secLeft.ToString() + " " + microSecLeft.ToString());
	}
}

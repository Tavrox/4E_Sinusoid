using UnityEngine;
using System.Collections;
[ExecuteInEditMode]

public class Label : MonoBehaviour {
	public string text = "Label";
	public GUISkin skin;
	public int size = 10;
	public Color color;
	public enum type
	{
		Button,
		Label,
		Box
	};
	public type typeList;
	private float offsetX = 215f;
	private float offsetY = -580f;

	void OnGUI()
	{
		Vector3 point = Camera.main.WorldToScreenPoint(transform.position);
		GUI.skin = skin;
		switch (typeList)
		{
			case (type.Box) :
			{
				skin.box.normal.textColor = color;
				skin.box.fontSize = size;
				GUI.Box(new Rect(point.x - offsetX, Screen.currentResolution.height - point.y  + offsetY, 200, 200), text);
				break;
			}
			case (type.Button) :
			{
				skin.button.normal.textColor = color;
				skin.button.fontSize = size;
				GUI.Button(new Rect(point.x - offsetX, Screen.currentResolution.height - point.y  + offsetY, 200, 200), text);
				break;
			}
			case (type.Label) :
			{
				skin.label.normal.textColor = color;
				skin.label.fontSize = size;
				GUI.Label(new Rect(point.x - offsetX, Screen.currentResolution.height - point.y + offsetY , 200, 200), text);
				break;
			}
		}
	}
}

using UnityEngine;
using System.Collections;

public class Waypoint : MonoBehaviour {
	public int _id { get; set; }
	
	public string _name { get; set; }
	
	public int _timePause { get; set; }

	public Waypoint(int id, string name, int timePause) {
		_id = id;
		_name = _name;
		_timePause = timePause;
	}
}

using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	public float _speedX;
	public float _speedY;
	public float _rot;
	private Vector3 _initPosProj;
	private Quaternion _initRotProj;
	public GameObject _target;
	public bool moving;

	public float speedX
	{
		get { return _speedX; }
		set { _speedX = value; }
	}

	public float speedY
	{
		get { return _speedY; }
		set { _speedY = value; }
	}

	public float rot
	{
		get { return _rot; }
		set { _rot = value; }
	}

	public Vector3 initPosProj
	{
		get { return _initPosProj; }
		set { _initPosProj = value; }
	}

	public Quaternion initRotProj
	{
		get { return _initRotProj; }
		set { _initRotProj = value; }
	}

	void Start()
	{
		_initPosProj = gameObject.transform.position;
		_initRotProj = gameObject.transform.rotation;

		InvokeRepeating("projectileCheck", 0f, 1f);
	}

	void Update()
	{
		if (moving)
		{	moveProj(); }

	}

	private void projectileCheck()
	{
		if (_target != null)
		{
			if (gameObject.transform.position == _target.transform.position)
			{ resetProj(); }
		}
	}

	public void resetProj()
	{
		gameObject.transform.position = initPosProj;
		gameObject.transform.rotation = initRotProj;
	}
	public void moveProj()
	{
		gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, _target.transform.position, speedX);
	}
	public void setupMove(GameObject _tar, bool _state)
	{
		_target = _tar;
		moving = _state;
	}

	void OnTriggerEnter(Collider _other)
	{
		if (_other.CompareTag("Player"))
	    {
			GameEventManager.TriggerGameOver();
		}

	}
}

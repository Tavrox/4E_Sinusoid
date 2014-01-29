using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	public float _speedX;
	public float _speedY;
	private float _rot;
	private Vector3 _initPosProj;
	private Quaternion _initRotProj;
	private LevelBrick _owner;
	public Vector3 _target;
	[HideInInspector] public bool moving;
	private Vector3 _direction;
	public FESound CollisionSound;

	public Vector3 direction
	{
		get { return _direction; }
		set { _direction = value; }
	}

	public float speedX
	{
		get { return _speedX; }
		set { _speedX = value; }
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

	public LevelBrick owner
	{
		get { return _owner; }
		set { _owner = value; }
	}

	public void Setup(GameObject _newTar, float _shtRate)
	{
		_target = _newTar.transform.position;
		direction = (_target - initPosProj).normalized;
		_initPosProj = gameObject.transform.position;
		_initRotProj = gameObject.transform.rotation;
	}

	void Update()
	{
		if (moving)
		{moveProj();}

	}

	public void resetProj()
	{
		gameObject.transform.position = initPosProj;
		gameObject.transform.rotation = initRotProj;
		direction = (_target - initPosProj).normalized;
		moving = false;
	}
	public void moveProj()
	{
		Debug.DrawRay(initPosProj, direction);
//		direction = (_target - initPosProj).normalized;
		gameObject.transform.position += new Vector3 ( speedX * _direction.x, speedX * _direction.y, 0f);
	}
	public void setupMove(GameObject _tar, bool _state)
	{
		_target = _tar.gameObject.transform.position;
		moving = _state;
	}

	void OnTriggerEnter(Collider _other)
	{
		if (_other.CompareTag("Player"))
	    {
			GameEventManager.TriggerGameOver();
		}
		if (_other.CompareTag("soundStopper"))
 	    {
			resetProj();
		}

	}
}

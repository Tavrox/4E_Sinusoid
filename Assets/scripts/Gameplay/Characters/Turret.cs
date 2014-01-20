using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour {


	private GameObject _base;
	private GameObject _maca;
	private GameObject _rotating;
	private Projectile _projectile;
	private GameObject _target;
	private OTAnimatingSprite _baseAnim;
	private OTAnimatingSprite _macaAnim;
	private OTAnimatingSprite _rotatingAnim;
	private OTSprite _projectileSpr;

	public float shootRate = 3f;
	public float turnRate = 1f;
	
	public FESound ShootSound;
	public FESound ScanSound;

	// Use this for initialization
	void Start () {
		_base = FETool.findWithinChildren(this.gameObject, "Base");
		_maca = FETool.findWithinChildren(this.gameObject, "Maca");
		_rotating = FETool.findWithinChildren(this.gameObject, "Rotating");
		_target = FETool.findWithinChildren(this.gameObject, "Target");
		_projectile = FETool.findWithinChildren(this.gameObject, "Projectile").GetComponent<Projectile>();

		_baseAnim = _base.GetComponentInChildren<OTAnimatingSprite>();
		_macaAnim = _maca.GetComponentInChildren<OTAnimatingSprite>();
		_rotatingAnim = _rotating.GetComponentInChildren<OTAnimatingSprite>();
		_projectileSpr = _projectile.GetComponentInChildren<OTSprite>();

		InvokeRepeating("shootAtPoint", 0f, shootRate);
		InvokeRepeating("turn", 0f, turnRate);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	private void scanPlace()
	{

	}
	private void shootAtPoint()
	{
		_macaAnim.PlayOnce("macaghul");
		_projectile.setupMove(_target, true);

//		_projectiletransform.position = Vector3.MoveTowards(transform.position, target.position, _projectile.speedx;);

		Debug.Log("Turret.shoot");
	}
	private void shootAtTarget(GameObject _target)
	{

	}
	private void calculateTrajectory(GameObject _target)
	{
		float projPosX = _projectile.transform.position.x;
		float projPosY = _projectile.transform.position.y;

		float tarPosX = _target.transform.position.x;
		float tarPosY = _target.transform.position.y;
	}
	private void turn()
	{
		_rotating.transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan((_target.transform.position.x-_rotating.transform.position.x)/Mathf.Abs(_rotating.transform.position.y-_target.transform.position.y))*Mathf.Rad2Deg);
//		float rotPosX = _rotating.transform.position.x;
//		float rotPosY = _rotating.transform.position.y;
//		
//		float tarPosX = _target.transform.position.x;
//		float tarPosY = _target.transform.position.y;
//
//		Vector2 rotPos = new Vector2(rotPosX, rotPosY);
//		Vector2 tarPos = new Vector2(tarPosX, tarPosY);
//
//		Vector2 upVector = tarPos - rotPos;
//		_rotating.transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Tan((Mathf.Abs(rotPosX - tarPosX) / Mathf.Abs(rotPosY - tarPosY))));
	
	}

}

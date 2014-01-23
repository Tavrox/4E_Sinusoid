using UnityEngine;
using System.Collections;

public class Turret : LevelBrick {


	private GameObject _base;
	private GameObject _maca;
	private GameObject _rotating;
	private Projectile _projectile;
	private GameObject _target;
	private Collider _triggerArea;
	private OTAnimatingSprite _baseAnim;
	private OTAnimatingSprite _macaAnim;
	private OTAnimatingSprite _rotatingAnim;
	private OTSprite _projectileSpr;

	public float shootRate = 1f;
	public float turnRate = 0.1f;
	
	public FESound ShootSound;
	public FESound ScanSound;
	public FESound IdleSound;

	// Use this for initialization
	void Start () {
		brickType = brickEnum.Turret;
		_base = FETool.findWithinChildren(this.gameObject, "Base");
		_maca = FETool.findWithinChildren(this.gameObject, "Maca");
		_rotating = FETool.findWithinChildren(this.gameObject, "Rotating");
		_target = FETool.findWithinChildren(this.gameObject, "Target");
		_projectile = FETool.findWithinChildren(this.gameObject, "Projectile").GetComponent<Projectile>();
		_projectile.owner = this;
		_projectile.Setup(_target, shootRate);
		_triggerArea = FETool.findWithinChildren(this.gameObject, "TriggerArea").GetComponent<BoxCollider>();

		_baseAnim = _base.GetComponentInChildren<OTAnimatingSprite>();
		_macaAnim = _maca.GetComponentInChildren<OTAnimatingSprite>();
		_rotatingAnim = _rotating.GetComponentInChildren<OTAnimatingSprite>();
		_projectileSpr = _projectile.GetComponentInChildren<OTSprite>();

		InvokeRepeating("shootAtPoint", 0f, shootRate);
		InvokeRepeating("turn", 0f, turnRate);
		InvokeRepeating("playIdleSound", 0f, IdleSound.RepeatRate);
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
		ShootSound.playDistancedSound();
		_projectile.resetProj();
		_projectile.setupMove(_target, true);
	}
	private void shootAtTarget(GameObject _target)
	{

	}
	private void turn()
	{
		_rotating.transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan((_target.transform.position.x-_rotating.transform.position.x)/Mathf.Abs(_rotating.transform.position.y-_target.transform.position.y))*Mathf.Rad2Deg);
	
	}
	public void changeTarget(GameObject _newTarget)
	{
		_projectile._target = _newTarget.gameObject.transform.position;
		_target.gameObject.transform.position = _newTarget.transform.position;
	}
	public GameObject getTarget()
	{
		return _target;
	}

	private void playIdleSound()
	{
		IdleSound.playDistancedSound();
	}

}

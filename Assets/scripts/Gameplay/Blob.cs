using UnityEngine;
using System.Collections;

public class Blob : MonoBehaviour {

	private float _cos, _sin, _alpha;
	private Vector3 vectorDir;
	private Transform myTransform;
	private Player player;
	private Projector instanceProj;
	private GameObject toDestroy;
	private bool extendProj, lightened, rotated;
	private Material myProjMaterial;

	public Projector prefabProj;
	public int numberOfAlphaStates = 3;
	[Range(0.1f, 100)] public float lifeTime = 9f;
	[Range(1, 100)] public int speedSound = 1;
	public int projDiameterIni = 30;
	[Range(1, 10)] public int enlargeDiameterSpeed = 1;
	public float diameterMultiplier = 1.5f, fadeSpeed = 500f;
	[Range(10, 100)] public int enlargeRatioSpeed = 10;
	public float maximumRatio = 1f;

	private RaycastHit hitInfo; //infos de collision
	private Ray /*detectBlockTop, detectBlockBottom,*/ detectBlockLeft, detectBlockRight; //point de départ, direction
	// Use this for initialization
	void Start () {
		myTransform = transform;
		player = GameObject.FindWithTag("Player").GetComponent<Player>();
		StartCoroutine("reduceAlpha");
		setPosition(new Vector3(player.transform.position.x, (player.transform.position.y-player.transform.localScale.y/2), player.transform.position.z));
		instanceProj = Instantiate(prefabProj) as Projector;
		instanceProj.fieldOfView=projDiameterIni;
		myProjMaterial = new Material(instanceProj.material);
		instanceProj.material = myProjMaterial;
		//instanceProj.transform.Rotate(new Vector3(0f,0f,90f));
		//instanceProj.aspectRatio=1;
	}
	// Update is called once per frame
	void Update () {

		if(extendProj) StartCoroutine("lightsOff");
		else {
			if(this.GetComponentInChildren<OTSprite>().alpha <= 0) endLife();
			else updateWaveElt();
		}
		if(myTransform.position.x > (player.transform.position.x + 10f) || myTransform.position.x < (player.transform.position.x - 10f) ||
		   myTransform.position.y > (player.transform.position.y + 10f) || myTransform.position.y < (player.transform.position.y - 10f)) {
			endLife();
		}
		
		detectBlockLeft = new Ray(myTransform.position, Vector3.left);
		detectBlockRight = new Ray(myTransform.position, Vector3.right);
//		detectBlockTop = new Ray(myTransform.position, Vector3.up);
//		detectBlockBottom = new Ray(myTransform.position, Vector3.down);
		Debug.DrawRay(myTransform.position, Vector3.left*0.5f);
		Debug.DrawRay(myTransform.position, Vector3.right*0.5f);
		Debug.DrawRay(myTransform.position, Vector3.up*0.5f);
		Debug.DrawRay(myTransform.position, Vector3.down*0.5f);
		
		if (!rotated && ((Physics.Raycast(detectBlockLeft, out hitInfo, 0.5f)) || (Physics.Raycast(detectBlockRight, out hitInfo, 0.5f)))) {
			if(hitInfo.collider.tag == "soundStopper") {
			instanceProj.transform.Rotate(new Vector3(0f,0f,90f));
			rotated = true;
			}
		}
	}

	IEnumerator reduceAlpha()
	{
		yield return new WaitForSeconds(lifeTime/numberOfAlphaStates);
		this.GetComponentInChildren<OTSprite>().alpha -= 1/numberOfAlphaStates;
		StartCoroutine("reduceAlpha");
	}
	void endLife () {
		StopCoroutine("reduceAlpha");

		Destroy(instanceProj.gameObject);
		Destroy(gameObject);
	}
	void updateWaveElt() {
		vectorDir.x = getCosX() * Time.deltaTime * speedSound;
		vectorDir.y = getSinX() * Time.deltaTime * speedSound;
		myTransform.position += new Vector3(vectorDir.x,vectorDir.y,0f);
		instanceProj.transform.position = new Vector3(myTransform.position.x,myTransform.position.y,-15f);
		//print(this.GetComponentInChildren<OTSprite>().alpha);
	}
	void stopWaveElt() {
		//print ("ça stoppe ?!");
		_cos = 0;
		_sin = 0;
		extendProj = true;
	}
	IEnumerator lightsOff() {
		yield return new WaitForSeconds(0.1f);
		//print(instanceProj.aspectRatio);
		if(!lightened) {
			if(instanceProj.aspectRatio <= maximumRatio) instanceProj.aspectRatio+=1f/enlargeRatioSpeed;
			if(instanceProj.fieldOfView <= projDiameterIni*diameterMultiplier) instanceProj.fieldOfView+=enlargeDiameterSpeed;
			else if (instanceProj.fieldOfView > projDiameterIni*diameterMultiplier && instanceProj.aspectRatio > maximumRatio) lightened = true;
		}
		else {
			//instanceProj.aspectRatio-=0.05f;
			//if(instanceProj.aspectRatio < 0f) endLife();
			myProjMaterial.color = new Color (0f,0f,0f,myProjMaterial.color.a-(1/fadeSpeed));
			//instanceProj.material.color = new Color (0f,0f,0f,instanceProj.material.color.a-0.005f);
			if(myProjMaterial.color.a <= 0) {StopCoroutine("lightsOff");endLife();}
		}
		StartCoroutine("lightsOff");
	}
	public void setCos(float value) {
		_cos = value;
	}
	public void setSin(float value) {
		_sin = value;
	}
	public void setPosition(Vector3 vector) {
		myTransform.position = new Vector3(vector.x, vector.y, vector.z);
	}
	public float getCosX() {
		return _cos;
	}
	public float getSinX() {
		return _sin;
	}

	void OnTriggerEnter(Collider other) {
		if(other.gameObject.CompareTag("soundStopper"))//if(other.gameObject.name == "Tiles")
		{print("BWAAAAAA");
			stopWaveElt();
		}
	}
}

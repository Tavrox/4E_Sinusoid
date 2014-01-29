using UnityEngine;
using System.Collections;

public class WaveElt : MonoBehaviour {
	
	private float _cos, _cosIni, _sin, _sinIni, _alpha,_initialAlphaProj, _initialAlpha, waveXOffset = 0f, _myProjAspectRatioIni, lifeTimeIni, offset, nextOffset;
	//private Color _initialAlphaProj;
	private Vector3 vectorDir;
	private Transform myTransform;
	//private Player player;
	private Projector instanceProj;
	private GameObject toDestroy;
	private bool extendProj, lightened, rotated, reducingAlpha, ligthOffing, specialCircle;
	private Material myProjMaterial;
	private float speedSoundIni;
	private GameObject _mySprite;
	private Transform pebblePositionSave;
	
	//public Projector prefabProj;
	public float numberOfAlphaStates = 3;
	[Range(0.1f, 100)] public float lifeTime = 0.6f;
	[Range(0.01f, 100)] public float speedSound = 3f;
	public float projDiameterIni = 3f;
	[Range(0.01f, 10)] public float enlargeDiameterSpeed = 2;
	public float diameterMultiplier = 0f, fadeSpeed = 50f;
	[Range(0.01f, 100)] public float enlargeRatioSpeed = 0.07f;
	public float maximumRatio = 5f;
	private Transform callerObj;
	//public bool calledByCharacter;

	/** INSTRU-FALL VARIABLES **/
	public float sprintSpeedCoeff = 2f, instruLifeTime = 8f, instruSpeedSound = 8f, instruProjDiameterIni = 1f, 
	instruEnlargeDiameterSpeed = 1f, instruDiameterMultiplier = 5f, instruFadeSpeed = 0.1f,
	instruEnlargeRatioSpeed = 0.4f, instruMaximumRatio = 5f, fallSpeedCoeff = 4f;
	public int instruNbAphaState = 6;


//	private RaycastHit hitInfo; //infos de collision
//	private Ray /*detectBlockTop, detectBlockBottom,*/ detectBlockLeft, detectBlockRight; //point de départ, direction
	// Use this for initialization
	void Start () {
		myTransform = transform;
		//player = GameObject.FindWithTag("Player").GetComponent<Player>();
		_mySprite = gameObject.transform.parent.GetComponentInChildren<OTSprite>().gameObject as GameObject;
		//callerObj = GameObject.FindWithTag("Player");
	
		//setPosition(new Vector3(callerObj.transform.position.x, (callerObj.transform.position.y-callerObj.transform.localScale.y/2), callerObj.transform.position.z));
		//instanceProj = this.GetComponent<Projector>();//Instantiate(prefabProj) as Projector;
		myTransform.localScale=new Vector3(0,myTransform.localScale.y,0);
		//myProjMaterial = new Material(instanceProj.material);
		//instanceProj.material = myProjMaterial;
		_myProjAspectRatioIni = 1f;
		
		_initialAlpha = 0.6f;
		_initialAlphaProj = 1f;//new Color (0f,0f,0f,myProjMaterial.color.a);
		speedSoundIni = speedSound;
		lifeTimeIni = lifeTime;
		
		_alpha = 0f;//this.GetComponentInChildren<OTSprite>().alpha = 0f;
		//myProjMaterial.color = new Color (0f,0f,0f,0f);
		//instanceProj.transform.Rotate(new Vector3(0f,0f,90f));
		//instanceProj.aspectRatio=1;
		pebblePositionSave = GameObject.Find("PebblePosition").GetComponent<Transform>();
		StartCoroutine("reduceAlpha");
		StartCoroutine("myUpdate");
	}
	// Update is called once per frame
	IEnumerator myUpdate () {
		if(extendProj && !ligthOffing) StartCoroutine("lightsOff");
		else if (!extendProj) {
			if(_alpha <= 0) endLife();
			else updateWaveElt();
		}
//		if(myTransform.position.x > (callerObj.position.x + 10f) || myTransform.position.x < (callerObj.position.x - 10f) ||
//		   myTransform.position.y > (callerObj.position.y + 10f) || myTransform.position.y < (callerObj.position.y - 10f)) {
//			endLife();
//		}
//		detectBlockLeft = new Ray(myTransform.position, Vector3.left);
//		detectBlockRight = new Ray(myTransform.position, Vector3.right);
		//		detectBlockTop = new Ray(myTransform.position, Vector3.up);
		//		detectBlockBottom = new Ray(myTransform.position, Vector3.down);
//		Debug.DrawRay(myTransform.position, Vector3.left*0.5f, Color.cyan);
//		Debug.DrawRay(myTransform.position, Vector3.right*0.5f, Color.cyan);
//		Debug.DrawRay(myTransform.position, Vector3.up*0.5f, Color.cyan);
//		Debug.DrawRay(myTransform.position, Vector3.down*0.5f, Color.cyan);
	
		yield return new WaitForSeconds(0.02f);
		StartCoroutine("myUpdate");
	}
	
	IEnumerator reduceAlpha()
	{
		reducingAlpha =true;
		//		print ("----beginREDUCEALPHA");
		yield return new WaitForSeconds((float) lifeTime/numberOfAlphaStates);
		_alpha -= /*this.GetComponentInChildren<OTSprite>().alpha -=*/ (float) 1/numberOfAlphaStates;
		//myProjMaterial.color = new Color (0f,0f,0f,myProjMaterial.color.a-((float) 1f/numberOfAlphaStates));
		_mySprite.GetComponent<OTSprite>().alpha = _alpha;
		//		print ("endREDUCEALPHA-----");
		reducingAlpha = false;
		StartCoroutine("reduceAlpha");
	}
	public void endLife () {
		StopCoroutine("myUpdate");
		StopCoroutine("reduceAlpha");
		reducingAlpha = false;
		gameObject.collider.isTrigger = false;
		gameObject.collider.enabled=false;
		_mySprite.GetComponent<OTSprite>().alpha = 0f;
//		myTransform.position = new Vector3(-100f,-100f,0f);
//		instanceProj.transform.position = new Vector3(-100f,-100f,-15f);
		_alpha/*this.GetComponentInChildren<OTSprite>().alpha*/ = 0f;
		myTransform.localScale=new Vector3(0,4,0);
		_mySprite.renderer.enabled = false;
		//myProjMaterial.color = new Color (0f,0f,0f,0f);
		//enabled = false;
		//Destroy(instanceProj.gameObject);
		//Destroy(gameObject);
	}
	public void startLife () {
		StopCoroutine("reduceAlpha");
		_alpha/*this.GetComponentInChildren<OTSprite>().alpha*/ = 0f;
		enabled = true;
		lightened = false;
		reducingAlpha = false;
//		if(callerObj.isLeft) waveXOffset = -callerObj.localScale.x/1.5f;
//		else waveXOffset = callerObj.localScale.x/1.5f;
		//print(callerObj.name);
		gameObject.collider.isTrigger = true;
		myTransform.parent.transform.position = new Vector3((callerObj.position.x+waveXOffset),callerObj.position.y,-15f);
		_mySprite.renderer.enabled = true;
		if(rotated) {myTransform.parent.transform.transform.Rotate(new Vector3(0f,0f,-90f));rotated = false;}
	//	instanceProj.transform.position = new Vector3((callerObj.position.x+waveXOffset),(callerObj.position.y/*-callerObj.localScale.y/2.3f*/),-15f);
		_alpha/*this.GetComponentInChildren<OTSprite>().alpha*/ = _initialAlpha;
		_mySprite.GetComponent<OTSprite>().alpha = _alpha;
		//myProjMaterial.color = new Color (0f,0f,0f,0.001f);
	//	myProjMaterial.color = _initialAlphaProj;
	//	instanceProj.aspectRatio = _myProjAspectRatioIni;
		myTransform.localScale=new Vector3(projDiameterIni,4,projDiameterIni);
		gameObject.collider.enabled=true;
		_cos = _cosIni;
		_sin = _sinIni;//if(callerObj.name=="Pebble(Clone)") print("PEEEEEEBLE");
		extendProj = false;ligthOffing=false;StopCoroutine("lightsOff");
		if(!reducingAlpha) StartCoroutine("reduceAlpha");
		StopCoroutine("myUpdate");
		StartCoroutine("myUpdate");
	}
	public void setCharacterPositionOffset (float charScaleX, bool dirLeft) {
		if(dirLeft) waveXOffset = -charScaleX/1.5f;
		else waveXOffset = charScaleX/1.5f;
	}
	public void setCharacterMoveOffset (float offsetValue) {
		nextOffset = offsetValue;
	}
	void updateWaveElt() {
		//if(myProjMaterial.color.a < _initialAlphaProj.a) myProjMaterial.color = new Color (0f,0f,0f,myProjMaterial.color.a+0.15f);
		offset = 0f;
		//if(callerObj.name == "Pebble(Clone)") print(callerObj.name);
		if(!specialCircle) offset = nextOffset;
		vectorDir.x = (getCosX() * Time.deltaTime * speedSound)+offset/1.5f;
		vectorDir.y = getSinX() * Time.deltaTime * speedSound;
		myTransform.parent.transform.position += new Vector3(vectorDir.x,vectorDir.y,0f);
	//	instanceProj.transform.position = new Vector3(myTransform.position.x,myTransform.position.y,-15f);
		//print(this.GetComponentInChildren<OTSprite>().alpha);
	}
	void stopWaveElt() {
		//print ("ça stoppe ?!");
		_cos = 0;
		_sin = 0;
		_mySprite.renderer.enabled = false;
		//_alpha/*this.GetComponentInChildren<OTSprite>().alpha*/ = 0f;
		extendProj = true;
	}
	IEnumerator lightsOff() {
		StopCoroutine("myUpdate");
		ligthOffing = true;
				//print ("*****beginLIGHTOFF");
		yield return new WaitForSeconds(0.1f);
		//print(instanceProj.aspectRatio);
		if(!lightened) {
			if(myTransform.localScale.z <= maximumRatio) myTransform.localScale+=new Vector3(0,0,enlargeRatioSpeed);
			if(myTransform.localScale.x <= projDiameterIni*diameterMultiplier) myTransform.localScale+=new Vector3(enlargeDiameterSpeed,0,0);
			else if (myTransform.localScale.x > projDiameterIni*diameterMultiplier && myTransform.localScale.z > maximumRatio) lightened = true;
		}
		else {
			//instanceProj.aspectRatio-=0.05f;
			//if(instanceProj.aspectRatio < 0f) endLife();
			_alpha -= fadeSpeed;
			myTransform.localScale-=new Vector3(0f,0f,fadeSpeed);
			//instanceProj.material.color = new Color (0f,0f,0f,instanceProj.material.color.a-0.005f);
		}
		if(myTransform.localScale.z <= 0f) {extendProj = false;ligthOffing=false;StopCoroutine("lightsOff");endLife();}
		else StartCoroutine("lightsOff");
		ligthOffing = false;
		//		print ("endLIGHTOFF******");
	}
	public void setCos(float value) {
		_cos = _cosIni = value;
	}
	public void setSin(float value) {
		_sin = _sinIni = value;
	}
	public void setAlpha(float value) {
		_alpha = value;
	}
	public void setPosition(Vector3 vector) {
		myTransform.parent.transform.position = new Vector3(vector.x, vector.y, vector.z);
	}
	public float getCosX() {
		return _cos;
	}
	public float getSinX() {
		return _sin;
	}
	public float getAlpha() {
		return _alpha;//myProjMaterial.color.a;
	}
	public void setWaveXOffset (float value) {
		waveXOffset = value;
	}
	public void setOffset (float value) {
		offset = value;
	}	
	public void setCallerObject (Transform obj) {
		callerObj = obj;
		//print (callerObj.name);
		//gameObject.transform.parent = GameObject.Find("Level/TilesLayout").transform;
	}
	public Transform getCallerObject () {
		return callerObj;
	}
	void OnCollisionEnter(Collision col) {
		if(col.gameObject.CompareTag("soundStopper") && !rotated) {
			Vector3 hit = col.contacts[0].normal;
			//Debug.Log(hit);
			float angle = Vector3.Angle(hit, Vector3.forward);

			if (Vector3.Dot(hit,Vector3.forward) > 0) { // top
				//print("PF_En_DESSOUS");
			}else if(Vector3.Dot(hit,Vector3.forward) < 0){ // Back
				//print("PF_Au_DESSUS");
			}else if(Vector3.Dot(hit,Vector3.forward) == 0){
				// Sides
				Vector3 cross = Vector3.Cross(Vector3.forward, hit);
				if (cross.y < 0) { // right
					//print("PF_A_DROITE");
					myTransform.parent.transform.transform.Rotate(new Vector3(0f,0f,90f));
					rotated = true;
				}
				else { // left
					//print("PF_A_GAUCHE");
					myTransform.parent.transform.transform.Rotate(new Vector3(0f,0f,90f));
					rotated = true;
				}
			}
			gameObject.collider.enabled=false;
		}
	}
	void OnTriggerEnter(Collider other) {
		if(other.gameObject.CompareTag("soundStopper"))//if(other.gameObject.name == "Tiles")
		{
			gameObject.collider.isTrigger = false;
			//print("BWAAAAAA");

//			if (!rotated && Physics.Raycast(detectBlockLeft, out hitInfo, 0.5f)) {
//				if(hitInfo.collider.tag == "soundStopper") {
//					//print("ROTATIOOOOOON LEEEFT");
//					rotated = true;
//				}
//			}
//			if (!rotated && Physics.Raycast(detectBlockRight, out hitInfo, 0.5f)) {
//				if(hitInfo.collider.tag == "soundStopper") {
//					//print("ROTATIOOOOOON RIIIGHT");
//					myTransform.parent.transform.transform.Rotate(new Vector3(0f,0f,90f));
//					rotated = true;
//				}
//			}
			stopWaveElt();
		}
		if(other.gameObject.CompareTag("soundKiller"))//if(other.gameObject.name == "Tiles")
		{
			_cos = 0;
			_sin = 0;
			_alpha = 0f;
			endLife();
		}
		if(other.gameObject.CompareTag("Enemy") && !callerObj.CompareTag("Enemy") && callerObj.name!="spriteParentDrop") {
			if(callerObj.CompareTag("Pebble") && callerObj.GetComponent<Pebble>().getCallerObject().CompareTag("Player")) {
				//print ("I HEAR A PLAYER'S PEBBLE");
				pebblePositionSave.transform.position = new Vector3(callerObj.gameObject.transform.position.x, callerObj.gameObject.transform.position.y, callerObj.gameObject.transform.position.z);
				if(!other.GetComponent<Enemy>().getChasingPlayer() && !other.GetComponent<Enemy>().getTarget().CompareTag("Player")) { //Ne réagit pas au caillou si déjà en chasse du joueur
					//print("QUI A VOLé L'ORANGE");
					other.GetComponent<Enemy>().setTarget(pebblePositionSave);
					other.GetComponent<Enemy>().activeChasing();
				}
			}
			else {
				//print ("I HEAR A FOOTSTEP");
				other.GetComponent<Enemy>().setTarget(callerObj);
				other.GetComponent<Enemy>().activeChasing();
			}
		}
	}
	public void setWalkState () {
		lifeTime = lifeTimeIni;
		speedSound = speedSoundIni;
		//print(transform.parent.transform.parent.transform.name);
	}
	public void setSprintState () {
		lifeTime = lifeTimeIni;
		speedSound = speedSoundIni * sprintSpeedCoeff;
	}
	public void setSpecial () {
		numberOfAlphaStates = instruNbAphaState;
		lifeTime = lifeTimeIni = instruLifeTime;
		speedSound = speedSoundIni = instruSpeedSound;
		projDiameterIni = instruProjDiameterIni;
		enlargeDiameterSpeed = instruEnlargeDiameterSpeed;
		diameterMultiplier = instruDiameterMultiplier;
		fadeSpeed = instruFadeSpeed;
		enlargeRatioSpeed = instruEnlargeRatioSpeed;
		maximumRatio = instruMaximumRatio;
		specialCircle = true;
	}
	public void setFallState () {
		speedSound = speedSoundIni * fallSpeedCoeff;
	}
	public void setGroundedState () {
		speedSound = speedSoundIni;
	}
}
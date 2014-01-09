using UnityEngine;
using System.Collections;

public class WaveElt : MonoBehaviour {
	
	private float _cos, _cosIni, _sin, _sinIni, _alpha,_initialAlphaProj, _initialAlpha, waveXOffset, _myProjAspectRatioIni, lifeTimeIni, offset, nextOffset;
	//private Color _initialAlphaProj;
	private Vector3 vectorDir;
	private Transform myTransform;
	//private Player player;
	private Projector instanceProj;
	private GameObject toDestroy;
	private bool extendProj, lightened, rotated, reducingAlpha, ligthOffing, specialCircle;
	private Material myProjMaterial;
	private float speedSoundIni;
	
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

	private RaycastHit hitInfo; //infos de collision
	private Ray /*detectBlockTop, detectBlockBottom,*/ detectBlockLeft, detectBlockRight; //point de départ, direction
	// Use this for initialization
	void Start () {
		myTransform = transform;
		//player = GameObject.FindWithTag("Player").GetComponent<Player>();
		
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

		StartCoroutine("reduceAlpha");
		StartCoroutine("myUpdate");
	}
	// Update is called once per frame
	IEnumerator myUpdate () {
		if(extendProj && !ligthOffing) StartCoroutine("lightsOff");
		else {
			if(_alpha <= 0) endLife();
			else updateWaveElt();
		}
//		if(myTransform.position.x > (callerObj.position.x + 10f) || myTransform.position.x < (callerObj.position.x - 10f) ||
//		   myTransform.position.y > (callerObj.position.y + 10f) || myTransform.position.y < (callerObj.position.y - 10f)) {
//			endLife();
//		}
		detectBlockLeft = new Ray(myTransform.position, Vector3.left);
		detectBlockRight = new Ray(myTransform.position, Vector3.right);
		//		detectBlockTop = new Ray(myTransform.position, Vector3.up);
		//		detectBlockBottom = new Ray(myTransform.position, Vector3.down);
		Debug.DrawRay(myTransform.position, Vector3.left*0.5f);
		Debug.DrawRay(myTransform.position, Vector3.right*0.5f);
		Debug.DrawRay(myTransform.position, Vector3.up*0.5f);
		Debug.DrawRay(myTransform.position, Vector3.down*0.5f);
	
		yield return new WaitForSeconds(0.015f);
		StartCoroutine("myUpdate");
	}
	
	IEnumerator reduceAlpha()
	{
		reducingAlpha =true;
		//		print ("----beginREDUCEALPHA");
		yield return new WaitForSeconds((float) lifeTime/numberOfAlphaStates);
		_alpha -= /*this.GetComponentInChildren<OTSprite>().alpha -=*/ (float) 1/numberOfAlphaStates;
		//myProjMaterial.color = new Color (0f,0f,0f,myProjMaterial.color.a-((float) 1f/numberOfAlphaStates));
		//		print ("endREDUCEALPHA-----");
		reducingAlpha = false;
		StartCoroutine("reduceAlpha");
	}
	public void endLife () {
		StopCoroutine("reduceAlpha");
		reducingAlpha = false;
//		myTransform.position = new Vector3(-100f,-100f,0f);
//		instanceProj.transform.position = new Vector3(-100f,-100f,-15f);
		_alpha/*this.GetComponentInChildren<OTSprite>().alpha*/ = 0f;
		myTransform.localScale=new Vector3(0,4,0);
		//myProjMaterial.color = new Color (0f,0f,0f,0f);
		//enabled = false;
		//Destroy(instanceProj.gameObject);
		//Destroy(gameObject);
	}
	public void startLife () {
		enabled = true;
		lightened = false;
//		if(callerObj.isLeft) waveXOffset = -callerObj.localScale.x/1.5f;
//		else waveXOffset = callerObj.localScale.x/1.5f;
		//print(callerObj.name);
		myTransform.parent.transform.position = new Vector3((callerObj.position.x),callerObj.position.y,-15f);
	//	instanceProj.transform.position = new Vector3((callerObj.position.x+waveXOffset),(callerObj.position.y/*-callerObj.localScale.y/2.3f*/),-15f);
		_alpha/*this.GetComponentInChildren<OTSprite>().alpha*/ = _initialAlpha;
		//myProjMaterial.color = new Color (0f,0f,0f,0.001f);
	//	myProjMaterial.color = _initialAlphaProj;
	//	instanceProj.aspectRatio = _myProjAspectRatioIni;
		myTransform.localScale=new Vector3(projDiameterIni,4,projDiameterIni);
		_cos = _cosIni;
		_sin = _sinIni;//if(callerObj.name=="Pebble(Clone)") print("PEEEEEEBLE");
		extendProj = false;ligthOffing=false;StopCoroutine("lightsOff");
		if(!reducingAlpha) StartCoroutine("reduceAlpha");
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
		//_alpha/*this.GetComponentInChildren<OTSprite>().alpha*/ = 0f;
		extendProj = true;
	}
	IEnumerator lightsOff() {
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
			//instanceProj.material.color = new Color (0f,0f,0f,instanceProj.material.color.a-0.005f);
		}
		if(_alpha <= 0) {extendProj = false;ligthOffing=false;StopCoroutine("lightsOff");endLife();}
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

	void OnTriggerEnter(Collider other) {
		if(other.gameObject.CompareTag("soundStopper"))//if(other.gameObject.name == "Tiles")
		{
			//print("BWAAAAAA");
			
//			if (!rotated && Physics.Raycast(detectBlockLeft, out hitInfo, 0.5f)) {
//				if(hitInfo.collider.tag == "soundStopper") {
//					print("ROTATIOOOOOON LEEEFT");
//					instanceProj.transform.Rotate(new Vector3(0f,0f,90f));
//					rotated = true;
//				}
//			}
//			if (!rotated && Physics.Raycast(detectBlockRight, out hitInfo, 0.5f)) {
//				if(hitInfo.collider.tag == "soundStopper") {
//					print("ROTATIOOOOOON RIIIGHT");
//					instanceProj.transform.Rotate(new Vector3(0f,0f,45f));
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
	}
	public void setWalkState () {
		lifeTime = lifeTimeIni;
		speedSound = speedSoundIni;
	}
	public void setSprintState () {
		lifeTime = lifeTimeIni;
		speedSound = speedSoundIni * 2;
	}
	public void setSpecial () {
		numberOfAlphaStates = 6;
		lifeTime = lifeTimeIni = 8f;
		speedSound = speedSoundIni = 8f;
		projDiameterIni = 2f;
		enlargeDiameterSpeed = 1f;
		diameterMultiplier = 2f;
		fadeSpeed = 500f;
		enlargeRatioSpeed = 0.4f;
		maximumRatio = 2f;
		specialCircle = true;
	}
}


//using UnityEngine;
//using System.Collections;
//
//public class WaveElt : MonoBehaviour {
//	
//	private float _cos, _cosIni, _sin, _sinIni, _alpha, _initialAlpha, waveXOffset, _myProjAspectRatioIni, lifeTimeIni, offset, nextOffset;
//	private Color _initialAlphaProj;
//	private Vector3 vectorDir;
//	private Transform myTransform;
//	//private Player player;
//	private Projector instanceProj;
//	private GameObject toDestroy;
//	private bool extendProj, lightened, rotated, reducingAlpha, ligthOffing, specialCircle;
//	private Material myProjMaterial;
//	private float speedSoundIni;
//	
//	//public Projector prefabProj;
//	public float numberOfAlphaStates = 3;
//	[Range(0.1f, 100)] public float lifeTime = 0.6f;
//	[Range(0.01f, 100)] public float speedSound = 3f;
//	public float projDiameterIni = 3f;
//	[Range(0.01f, 10)] public float enlargeDiameterSpeed = 2;
//	public float diameterMultiplier = 0f, fadeSpeed = 50f;
//	[Range(0.01f, 100)] public float enlargeRatioSpeed = 0.07f;
//	public float maximumRatio = 5f;
//	private Transform callerObj;
//	//public bool calledByCharacter;
//	
//	private RaycastHit hitInfo; //infos de collision
//	private Ray /*detectBlockTop, detectBlockBottom,*/ detectBlockLeft, detectBlockRight; //point de départ, direction
//	// Use this for initialization
//	void Start () {
//		myTransform = transform;
//		//player = GameObject.FindWithTag("Player").GetComponent<Player>();
//		
//		//callerObj = GameObject.FindWithTag("Player");
//		
//		
//		//setPosition(new Vector3(callerObj.transform.position.x, (callerObj.transform.position.y-callerObj.transform.localScale.y/2), callerObj.transform.position.z));
//		instanceProj = this.GetComponent<Projector>();//Instantiate(prefabProj) as Projector;
//		instanceProj.fieldOfView=projDiameterIni;
//		myProjMaterial = new Material(instanceProj.material);
//		instanceProj.material = myProjMaterial;
//		_myProjAspectRatioIni = instanceProj.aspectRatio;
//		
//		_initialAlpha = 0.6f;
//		_initialAlphaProj = new Color (0f,0f,0f,myProjMaterial.color.a);
//		speedSoundIni = speedSound;
//		lifeTimeIni = lifeTime;
//		
//		this.GetComponentInChildren<OTSprite>().alpha = 0f;
//		myProjMaterial.color = new Color (0f,0f,0f,0f);
//		//instanceProj.transform.Rotate(new Vector3(0f,0f,90f));
//		//instanceProj.aspectRatio=1;
//		
//		StartCoroutine("reduceAlpha");
//		StartCoroutine("myUpdate");
//	}
//	// Update is called once per frame
//	IEnumerator myUpdate () {
//		if(extendProj && !ligthOffing) StartCoroutine("lightsOff");
//		else {
//			if(myProjMaterial.color.a <= 0) endLife();
//			else updateWaveElt();
//		}
//		if(myTransform.position.x > (callerObj.position.x + 10f) || myTransform.position.x < (callerObj.position.x - 10f) ||
//		   myTransform.position.y > (callerObj.position.y + 10f) || myTransform.position.y < (callerObj.position.y - 10f)) {
//			endLife();
//		}
//		detectBlockLeft = new Ray(myTransform.position, Vector3.left);
//		detectBlockRight = new Ray(myTransform.position, Vector3.right);
//		//		detectBlockTop = new Ray(myTransform.position, Vector3.up);
//		//		detectBlockBottom = new Ray(myTransform.position, Vector3.down);
//		Debug.DrawRay(myTransform.position, Vector3.left*0.5f);
//		Debug.DrawRay(myTransform.position, Vector3.right*0.5f);
//		Debug.DrawRay(myTransform.position, Vector3.up*0.5f);
//		Debug.DrawRay(myTransform.position, Vector3.down*0.5f);
//		
//		yield return new WaitForSeconds(0.015f);
//		StartCoroutine("myUpdate");
//	}
//	
//	IEnumerator reduceAlpha()
//	{
//		reducingAlpha =true;
//		//		print ("----beginREDUCEALPHA");
//		yield return new WaitForSeconds((float) lifeTime/numberOfAlphaStates);
//		this.GetComponentInChildren<OTSprite>().alpha -= (float) 1/numberOfAlphaStates;
//		myProjMaterial.color = new Color (0f,0f,0f,myProjMaterial.color.a-((float) 1f/numberOfAlphaStates));
//		//		print ("endREDUCEALPHA-----");
//		StartCoroutine("reduceAlpha");
//		reducingAlpha = false;
//	}
//	public void endLife () {
//		StopCoroutine("reduceAlpha");
//		reducingAlpha = false;
//		//		myTransform.position = new Vector3(-100f,-100f,0f);
//		//		instanceProj.transform.position = new Vector3(-100f,-100f,-15f);
//		this.GetComponentInChildren<OTSprite>().alpha = 0f;
//		myProjMaterial.color = new Color (0f,0f,0f,0f);
//		//enabled = false;
//		//Destroy(instanceProj.gameObject);
//		//Destroy(gameObject);
//	}
//	public void startLife () {
//		enabled = true;
//		lightened = false;
//		//		if(callerObj.isLeft) waveXOffset = -callerObj.localScale.x/1.5f;
//		//		else waveXOffset = callerObj.localScale.x/1.5f;
//		//print(callerObj.name);
//		myTransform.position = new Vector3((callerObj.position.x),callerObj.position.y,0f);
//		instanceProj.transform.position = new Vector3((callerObj.position.x+waveXOffset),(callerObj.position.y/*-callerObj.localScale.y/2.3f*/),-15f);
//		this.GetComponentInChildren<OTSprite>().alpha = _initialAlpha;
//		//myProjMaterial.color = new Color (0f,0f,0f,0.001f);
//		myProjMaterial.color = _initialAlphaProj;
//		instanceProj.aspectRatio = _myProjAspectRatioIni;
//		instanceProj.fieldOfView = projDiameterIni;
//		_cos = _cosIni;
//		_sin = _sinIni;//if(callerObj.name=="Pebble(Clone)") print("PEEEEEEBLE");
//		extendProj = false;ligthOffing=false;StopCoroutine("lightsOff");
//		if(!reducingAlpha) StartCoroutine("reduceAlpha");
//	}
//	public void setCharacterPositionOffset (float charScaleX, bool dirLeft) {
//		if(dirLeft) waveXOffset = -charScaleX/1.5f;
//		else waveXOffset = charScaleX/1.5f;
//	}
//	public void setCharacterMoveOffset (float offsetValue) {
//		nextOffset = offsetValue;
//	}
//	void updateWaveElt() {
//		//if(myProjMaterial.color.a < _initialAlphaProj.a) myProjMaterial.color = new Color (0f,0f,0f,myProjMaterial.color.a+0.15f);
//		offset = 0f;
//		//if(callerObj.name == "Pebble(Clone)") print(callerObj.name);
//		if(!specialCircle) offset = nextOffset;
//		vectorDir.x = (getCosX() * Time.deltaTime * speedSound)+offset/1.5f;
//		vectorDir.y = getSinX() * Time.deltaTime * speedSound;
//		myTransform.position += new Vector3(vectorDir.x,vectorDir.y,0f);
//		instanceProj.transform.position = new Vector3(myTransform.position.x,myTransform.position.y,-15f);
//		//print(this.GetComponentInChildren<OTSprite>().alpha);
//	}
//	void stopWaveElt() {
//		//print ("ça stoppe ?!");
//		_cos = 0;
//		_sin = 0;
//		this.GetComponentInChildren<OTSprite>().alpha = 0f;
//		extendProj = true;
//	}
//	IEnumerator lightsOff() {
//		ligthOffing = true;
//		//		print ("*****beginLIGHTOFF");
//		yield return new WaitForSeconds(0.1f);
//		//print(instanceProj.aspectRatio);
//		if(!lightened) {
//			if(instanceProj.aspectRatio <= maximumRatio) instanceProj.aspectRatio+=enlargeRatioSpeed;
//			if(instanceProj.fieldOfView <= projDiameterIni*diameterMultiplier) instanceProj.fieldOfView+=enlargeDiameterSpeed;
//			else if (instanceProj.fieldOfView > projDiameterIni*diameterMultiplier && instanceProj.aspectRatio > maximumRatio) lightened = true;
//		}
//		else {
//			//instanceProj.aspectRatio-=0.05f;
//			//if(instanceProj.aspectRatio < 0f) endLife();
//			myProjMaterial.color = new Color (0f,0f,0f,myProjMaterial.color.a-(1f/fadeSpeed));
//			//instanceProj.material.color = new Color (0f,0f,0f,instanceProj.material.color.a-0.005f);
//		}
//		if(myProjMaterial.color.a <= 0) {extendProj = false;ligthOffing=false;StopCoroutine("lightsOff");endLife();}
//		else StartCoroutine("lightsOff");
//		ligthOffing = false;
//		//		print ("endLIGHTOFF******");
//	}
//	public void setCos(float value) {
//		_cos = _cosIni = value;
//	}
//	public void setSin(float value) {
//		_sin = _sinIni = value;
//	}
//	public void setPosition(Vector3 vector) {
//		myTransform.position = new Vector3(vector.x, vector.y, vector.z);
//	}
//	public float getCosX() {
//		return _cos;
//	}
//	public float getSinX() {
//		return _sin;
//	}
//	public float getAlpha() {
//		return myProjMaterial.color.a;
//	}
//	public void setWaveXOffset (float value) {
//		waveXOffset = value;
//	}
//	public void setOffset (float value) {
//		offset = value;
//	}	
//	public void setCallerObject (Transform obj) {
//		callerObj = obj;
//		//print (callerObj.name);
//		//gameObject.transform.parent = GameObject.Find("Level/TilesLayout").transform;
//	}
//	
//	void OnTriggerEnter(Collider other) {
//		if(other.gameObject.CompareTag("soundStopper"))//if(other.gameObject.name == "Tiles")
//		{
//			//print("BWAAAAAA");
//			
//			//			if (!rotated && Physics.Raycast(detectBlockLeft, out hitInfo, 0.5f)) {
//			//				if(hitInfo.collider.tag == "soundStopper") {
//			//					print("ROTATIOOOOOON LEEEFT");
//			//					instanceProj.transform.Rotate(new Vector3(0f,0f,90f));
//			//					rotated = true;
//			//				}
//			//			}
//			//			if (!rotated && Physics.Raycast(detectBlockRight, out hitInfo, 0.5f)) {
//			//				if(hitInfo.collider.tag == "soundStopper") {
//			//					print("ROTATIOOOOOON RIIIGHT");
//			//					instanceProj.transform.Rotate(new Vector3(0f,0f,45f));
//			//					rotated = true;
//			//				}
//			//			}
//			stopWaveElt();
//		}
//		if(other.gameObject.CompareTag("soundKiller"))//if(other.gameObject.name == "Tiles")
//		{
//			_cos = 0;
//			_sin = 0;
//			this.GetComponentInChildren<OTSprite>().alpha = 0f;
//			endLife();
//		}
//	}
//	public void setWalkState () {
//		lifeTime = lifeTimeIni;
//		speedSound = speedSoundIni;
//	}
//	public void setSprintState () {
//		lifeTime = lifeTimeIni;
//		speedSound = speedSoundIni * 2;
//	}
//	public void setSpecial () {
//		numberOfAlphaStates = 6;
//		lifeTime = lifeTimeIni = 8f;
//		speedSound = speedSoundIni = 8f;
//		projDiameterIni = 14f;
//		enlargeDiameterSpeed = 1f;
//		diameterMultiplier = 2f;
//		fadeSpeed = 500f;
//		enlargeRatioSpeed = 0.4f;
//		maximumRatio = 2f;
//		specialCircle = true;
//	}
//}
//
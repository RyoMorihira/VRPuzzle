using UnityEngine;
using System.Collections;

public class LaserMachineController : MonoBehaviour {

	public GameObject energyBallPrefab;

	private GameObject upside;
	private GameObject downside;
	private GameObject enemy;
	private GameObject energyBall;

	private float energy;
	private float suppliedEnergy;
	private ParticleSystem energyParticle;
	private Vector3 upsideRotation;
	private Vector3 downsideRotation;
	private bool faceEnemy = false;
	private float faceBeginTime = 0.0f;
	private Quaternion destinationRotation;
	private Quaternion iniRotation;

	private string energyBallShotSE;
	private string energyChargingSE;
	private SoundPlayer energyChargingSoundPlayer;

	void Awake () {
		upside = transform.FindChild ("Upside").gameObject;
		downside = transform.FindChild ("Downside").gameObject;
	}

	// Use this for initialization
	void Start () {
		energy = 0.0f;
		upsideRotation = upside.transform.localEulerAngles;
		downsideRotation = downside.transform.localEulerAngles;

		energyBallShotSE = "SE/Laser/UL_Short_burst_106";
		energyChargingSE = "SE/Laser/UL_FS_Designed_Growl_Var4";
	}

	// Update is called once per frame
	void Update () {
		if (energyBall != null && energy != 0.0f && Mathf.Log(energy/13.0f) * 25.0f > energyParticle.startSize) {
			energyParticle.startSize += Time.deltaTime * suppliedEnergy / 5.0f;
			if(!energyBall.transform.localPosition.Equals(new Vector3(0.0f, 0.0f, 20.0f))){
				energyBall.transform.localPosition = new Vector3(0.0f, 0.0f, 20.0f);
			}
		}
		if (energy != 0.0f) {
			upsideRotation.z += Mathf.Log (energy / 13.0f) * 250.0f * Time.deltaTime;
			upside.transform.localEulerAngles = upsideRotation;
			downsideRotation.z -= Mathf.Log (energy / 13.0f) * 200.0f * Time.deltaTime;
			downside.transform.localEulerAngles = downsideRotation;
			if (energyChargingSoundPlayer != null) {
				energyChargingSoundPlayer.SetVolume (Mathf.Log (energy / 13.0f) / Mathf.Log (1000.0f / 13.0f));
			}
		}

		if (faceEnemy) {
			transform.rotation = Quaternion.Slerp (iniRotation, destinationRotation, 0.5f - Mathf.Cos (Mathf.PI * (Time.time - faceBeginTime)) * 0.5f);
			if (faceBeginTime + 1.0f < Time.time) {
				transform.rotation = destinationRotation;
				faceEnemy = false;
			}
		}
	}

	public void SetEnergyBall(){
		energyBall = (GameObject)Instantiate (energyBallPrefab);
		energyBall.transform.parent = gameObject.transform;
		energyBall.transform.localPosition = new Vector3(0.0f, 0.0f, 20.0f);
		energyParticle = energyBall.GetComponent<ParticleSystem> ();
	}

	public void SetEnemy(GameObject enemy){
		this.enemy = enemy;
		enemy.GetComponent<EnemyController> ().SetEnergyBall (energyBall);
	}

	public float SupplyEnergy(float amount, float duration){
		if (energy == 0.0f) {
			//Singleton<SoundPlayer>.instance.playSE ("EnergyCharging");
			energyChargingSoundPlayer = SoundPlayer.SoundPlay(transform, energyChargingSE, 1.0f, true);
		}
		energy += amount;
		suppliedEnergy = amount;
		Debug.Log (energy);
		return amount;
	}

	public float GetEnergy(){
		Debug.Log ("Current Energy : " + energy);
		return energy;
	}

	public void FaceEnemy(){
		faceEnemy = true;
		faceBeginTime = Time.time;
		iniRotation = transform.rotation;
		GameObject obj = new GameObject ();
		obj.transform.LookAt (enemy.transform.position);
		destinationRotation = obj.transform.rotation;
		Destroy (obj);
	}

	public void FireEnergyBall(){
		SoundPlayer.SoundPlay(transform, energyBallShotSE, Mathf.Log(energy / 13.0f) / Mathf.Log(1000.0f / 13.0f));
		energy = 0.0f;
		energyBall.GetComponent<EnergyBallController> ().FireTo (enemy, 500.0f);
		energyChargingSoundPlayer.SoundStop ();
	}

	public Vector3 GetDownsidePosition(){
		return downside.transform.position;
	}

	public bool IsEnergyBallFired(){
		if (energyBall != null && energyBall.GetComponent<EnergyBallController> ().IsFired()) {
			return true;
		} else {
			return false;
		}
	}

	public void SetEnergyBallCollider(bool b){
		energyBall.GetComponent<EnergyBallController> ().SetCollider (false);
	}
}

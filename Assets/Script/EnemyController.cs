using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

	public GameObject burstPrefab;

	private Vector3 initialPosition;
	private float initialTime;
	private float duration;
	private GameObject energyball;
	private bool explode = false;
	private float explodeTime = 0.0f;
	private GameObject burst;
	private float size = 0.0f;

	private string explosionSE;
	private string enemySE;

	// Use this for initialization
	void Awake () {
		initialPosition = transform.position;
		initialTime = Time.time;
		duration = 100.0f;
	}

	void Start(){
		explosionSE = "SE/Enemy/explosion_bazooka";
		enemySE = "SE/Enemy/Ambience_AlienPlanet_00";
		SoundPlayer.SoundPlay (transform, enemySE, 1.0f, true, 1.0f);
	}
	
	// Update is called once per frame
	void Update () {
		if (!explode) {
			transform.position = initialPosition * (1.0f - (Time.time - initialTime) / duration);
		} else {
			if (Time.time > explodeTime + 0.2f) {
				Destroy (gameObject);
			}
		}
	}

	public void SetDuration(float duration){
		this.duration = duration;
	}

	public void SetEnergyBall(GameObject ball){
		energyball = ball;
	}

	void OnTriggerEnter(Collider other){
		Destroy (other.gameObject);
		explode = true;
		explodeTime = Time.time;
		burst = (GameObject)Instantiate (burstPrefab, transform.position, transform.rotation);
		burst.GetComponent<BurstController> ().SetParticleSize (size);
		//Singleton<SoundPlayer>.instance.playSEOneShot ("EnemyExplosion");
		SoundPlayer.SoundPlay(transform.position, explosionSE, 1.0f, false, 0.5f);
		Debug.Log (transform.position);
	}

	public void SetSize(float size){
		this.size = size;
	}

	public float CollisionPeriod(){
		if (energyball == null) {
			return 100000.0f;
		} else {
			return (transform.position.magnitude - energyball.transform.localPosition.z - size / 2.0f) / initialPosition.magnitude * duration;
		}
	}
}

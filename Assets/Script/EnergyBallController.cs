using UnityEngine;
using System.Collections;

public class EnergyBallController : MonoBehaviour {

	private bool fire = false;
	private float speed = 0.0f;
	private GameObject destination;
	private Vector3 iniPosition;
	private float fireTime = 0.0f;
	private Collider collider;

	// Use this for initialization
	void Start () {
		iniPosition = transform.position;
		collider = GetComponent<Collider> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (fire) {
			transform.position = iniPosition + (destination.transform.position - iniPosition) / Vector3.Magnitude(destination.transform.position - iniPosition) * speed * (Time.time - fireTime);
		}
		if (!GetComponent<ParticleSystem> ().IsAlive ()) {
			Destroy (gameObject);
		}
	}

	public void FireTo(GameObject destination, float speed){
		fire = true;
		iniPosition = transform.position;
		this.destination = destination;
		this.speed = speed;
		fireTime = Time.time;
	}

	public bool IsFired(){
		return fire;
	}

	public void SetCollider(bool b){
		collider.enabled = b;
	}
}

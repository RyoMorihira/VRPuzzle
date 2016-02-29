using UnityEngine;
using System.Collections;

public class BurstController : MonoBehaviour {

	private ParticleSystem[] particles;

	void Awake(){
		particles = new ParticleSystem[7];
		particles[0] = transform.FindChild ("par1").gameObject.GetComponent<ParticleSystem> ();
		particles[1] = particles[0].transform.FindChild ("par1").gameObject.GetComponent<ParticleSystem> ();
		particles[2] = particles[0].transform.FindChild ("fire1").gameObject.GetComponent<ParticleSystem> ();
		particles[3] = particles[0].transform.FindChild ("glitter1").gameObject.GetComponent<ParticleSystem> ();
		particles[4] = particles[0].transform.FindChild ("stone1").gameObject.GetComponent<ParticleSystem> ();
		particles[5] = particles[0].transform.FindChild ("ring1").gameObject.GetComponent<ParticleSystem> ();
		particles[6] = particles[0].transform.FindChild ("smoke1").gameObject.GetComponent<ParticleSystem> ();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetParticleSize(float size){
		for (int i = 0; i < particles.Length; i++) {
			particles [i].startSize *= size / 4.0f * 2.0f;
		}
	}
}

using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class VertexController : MonoBehaviour {

	public GameObject aura;

	private GameObject instanceAura;

	private string auraSE;

	void Awake(){
		//aura = transform.FindChild ("Aura_1").gameObject;
	}

	// Use this for initialization
	void Start () {
		auraSE = "SE/Vertex/LowSwoosh1";
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void ChangeColor(Color c){
		Material vm = GetComponent<Renderer> ().material;
		vm.color = new Color (c.r, c.g, c.b, 120.0f);
		vm.SetColor ("_EmissionColor", vm.color);
		GetComponent<Renderer> ().material = vm;
	}

	public void SetEventTrigger(EventTrigger.Entry entry){
		EventTrigger trigger = transform.FindChild ("EventTrigger").GetComponent<EventTrigger> ();
		trigger.triggers.Add (entry);
	}

	public void ParticlePlay(){
		//aura.GetComponent<ParticleSystem> ().Play ();
		if (instanceAura != null) {
			Destroy (instanceAura);
		}
		instanceAura = Instantiate(aura, transform.position, transform.rotation) as GameObject;
		instanceAura.transform.parent = transform;
		instanceAura.GetComponent<ParticleSystem> ().startColor = GetComponent<Renderer> ().material.color;
		SoundPlayer.SoundPlay (transform, auraSE, 0.1f);
	}

	public void ParticleStop(){
		//aura.GetComponent<ParticleSystem> ().Stop ();
		instanceAura.GetComponent<ParticleSystem>().Stop();
	}
}

using UnityEngine;
using System.Collections;

public class ExplanationController : MonoBehaviour {

	public Material triangleMaterial;
	public GameObject energySupply;

	private GameObject[] vertices;
	private VertexController[] vc;
	private GameObject energyBall;
	private ParticleSystem energyBallParticle;
	private Color color;
	private float baseTime;
	private GameObject triangle;
	private Vector3 g;
	private GameObject supply;
	private int progress = 0;

	// Use this for initialization
	void Start () {
		vertices = new GameObject[3];
		vc = new VertexController[3];
		color = RandomColor ();
		for (int i = 0; i < 3; i++) {
			vertices[i] = transform.FindChild ("Vertex" + (i + 1)).gameObject;
			vc[i] = vertices [i].GetComponent<VertexController> ();
			vc [i].ChangeColor (color);
		}
		g = (vertices [0].transform.position + vertices [1].transform.position + vertices [2].transform.position) / 3.0f;
		energyBall = transform.FindChild ("EnergyBall").gameObject;
		energyBallParticle = energyBall.GetComponent<ParticleSystem> ();
		baseTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if (progress == 0 && Time.time - baseTime > 2.0f) {
			// first aura @ 2s
			vc[0].ParticlePlay();
			progress++;
		} else if (progress == 1 && Time.time - baseTime > 3.0f) {
			// second aura @ 3s
			vc[1].ParticlePlay();
			progress++;
		} else if (progress == 2 && Time.time - baseTime > 4.0f) {
			// third aura @ 4s
			vc[2].ParticlePlay();
			progress++;
		} else if (progress == 3 && Time.time - baseTime > 4.5f) {
			// create triangle @ 4.5s
			CreateTriangle();
			progress++;
		} else if (progress == 4 && Time.time - baseTime > 5.0f) {
			// supply energy & energy ball @ 5s
			SetEnergySupply(g, energyBall.transform.position, color);
			progress++;
		} else if (progress == 5 && Time.time - baseTime > 8.0f) {
			// finish @ 8s
			baseTime = Time.time;
			color = RandomColor ();
			Destroy (triangle);
			Destroy (supply);
			for (int i = 0; i < 3; i++) {
				vc [i].ParticleStop ();
				vc [i].ChangeColor (color);
			}
			energyBallParticle.startSize = 0.0f;
			progress = 0;
		}
		if (supply != null) {
			// energy charge
			energyBallParticle.startSize += Time.deltaTime * 2.0f;
		}
	}

	Color RandomColor(){
		int n = Random.Range (0, 8);
		Color col = new Color ();
		switch (n) {
		case 0:
			col = Color.red;
			break;
		case 1:
			col = Color.green;
			break;
		case 2:
			col = Color.blue;
			break;
		case 3:
			// Orange
			col = new Color(243.0f / 255.0f, 152.0f / 255.0f, 0.0f / 255.0f);
			break;
		case 4:
			col = Color.white;
			break;
		case 5:
			col = Color.magenta;
			break;
		case 6:
			col = Color.cyan;
			break;
		}

		return col;
	}

	void CreateTriangle(){
		Vector3 g, vg1, vg2, vg3;
		g = (vertices[0].transform.position + vertices[1].transform.position + vertices[2].transform.position) / 3.0f;
		vg1 = g - vertices[0].transform.position;
		vg2 = g - vertices[1].transform.position;
		vg3 = g - vertices[2].transform.position;

		Mesh triMesh = new Mesh ();
		triMesh.name = "EnergyField";
		triMesh.vertices = new Vector3[] {
			vertices[0].transform.position + vg1 / vg1.magnitude * 0.5f,
			vertices[1].transform.position + vg2 / vg2.magnitude * 0.5f,
			vertices[2].transform.position + vg3 / vg3.magnitude * 0.5f,
		};
		triMesh.uv = new Vector2[]{
			new Vector2 (0, 0),
			new Vector2 (0, 1),
			new Vector2 (1, 0),
		};
		triMesh.triangles = new int[]{
			0, 2, 1,
		};
		triMesh.RecalculateNormals ();
		//if (Vector3.Dot (triMesh.normals [0], g) > 0.0f) {
		if(false){
			int[] temp = new int[3];
			temp [0] = triMesh.triangles [0];
			temp [1] = triMesh.triangles [2];
			temp [2] = triMesh.triangles [1];
			triMesh.triangles = temp;
			triMesh.RecalculateNormals ();
		}
		triMesh.RecalculateBounds ();
		triMesh.Optimize ();
		triangle = new GameObject ("Triangle");
		MeshRenderer mr = triangle.AddComponent<MeshRenderer> ();
		mr.material = triangleMaterial;
		mr.material.SetColor("_EmissionColor", color * 0.3f);
		MeshFilter mf = triangle.AddComponent<MeshFilter> ();
		mf.mesh = triMesh;
	}

	void SetEnergySupply(Vector3 from, Vector3 to, Color c){
		supply = (GameObject)Instantiate (energySupply, from, transform.rotation);
		supply.transform.LookAt (to);
		supply.transform.localEulerAngles = supply.transform.localEulerAngles + new Vector3 (90, 0, 0);
		ParticleSystem particle = supply.GetComponent<ParticleSystem> ();
		particle.startLifetime = Vector3.Magnitude (to - from) / particle.startSpeed;
		particle.startColor = c;
	}
}

using UnityEngine;
using System.Collections;

public class CreateIcon : MonoBehaviour {

	private GameObject[] vertices;
	public Material triangleMaterial;
	private Color color;
	public GameObject v1;
	public GameObject v2;
	public GameObject v3;
	private bool icon = false;
	public GameObject energySupply;
	public GameObject energyBall;

	// Use this for initialization
	void Start () {
		Screen.SetResolution (1024, 1024, false);
		color = new Color (200.0f / 255.0f, 95.0f / 255.0f, 10.0f / 255.0f);
		vertices = new GameObject[3];
		vertices [0] = v1;
		vertices [1] = v2;
		vertices [2] = v3;
		for (int i = 0; i < 3; i++) {
			vertices [i].GetComponent<VertexController> ().ChangeColor (color);
			vertices [i].GetComponent<VertexController> ().ParticlePlay ();
		}
		CreateTriangle ();
		//SetEnergySupply ((vertices [0].transform.position + vertices [1].transform.position + vertices [2].transform.position) / 3.0f, energyBall.transform.position, color);
	}
	
	// Update is called once per frame
	void Update () {
		if (!icon && Time.time > 5.0f) {
			icon = false;
			Application.CaptureScreenshot ("icon.png", 4);
		}
	}

	public float CreateTriangle(){
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
		GameObject triangle = new GameObject ("Triangle");
		MeshRenderer mr = triangle.AddComponent<MeshRenderer> ();
		mr.material = triangleMaterial;
		mr.material.SetColor("_EmissionColor", color * 0.3f);
		MeshFilter mf = triangle.AddComponent<MeshFilter> ();
		mf.mesh = triMesh;

		return 0.5f * Mathf.Sqrt(Vector3.SqrMagnitude(triMesh.vertices[1] - triMesh.vertices[0]) * Vector3.SqrMagnitude(triMesh.vertices[2] - triMesh.vertices[0]) - Mathf.Pow(Vector3.Dot(triMesh.vertices[1] - triMesh.vertices[0], triMesh.vertices[2] - triMesh.vertices[0]), 2.0f));
	}

	void SetEnergySupply(Vector3 from, Vector3 to, Color c){
		GameObject supply = (GameObject)Instantiate (energySupply, from, transform.rotation);
		supply.transform.LookAt (to);
		supply.transform.localEulerAngles = supply.transform.localEulerAngles + new Vector3 (90, 0, 0);
		ParticleSystem particle = supply.GetComponent<ParticleSystem> ();
		particle.startLifetime = Vector3.Magnitude (to - from) / particle.startSpeed;
		particle.startColor = c;
	}
}

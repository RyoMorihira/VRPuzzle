using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class VerticesController : MonoBehaviour {

	public GameObject vertex;
	public Material triangleMaterial;

	private Vector3[] verticesMap;
	private GameObject[] vertices;
	private int[] verticesType;
	private Color[] verticesColorType;
	private Queue<GameObject> triangles;

	public static int level = 6;

	void Awake () {
	//void Start () {
		verticesMap = this.GetComponent<MeshFilter> ().mesh.vertices;
		verticesType = new int[verticesMap.Length];
		SetVerticesColorType (level);
		vertices = new GameObject[verticesMap.Length];
		for(int i = 0; i < verticesMap.Length; i++){
			verticesMap[i] *= 10.0f;
			Vector3 v = verticesMap[i];
			GameObject obj = Instantiate(vertex, v, transform.rotation) as GameObject;
			obj.transform.parent = transform;
			verticesType[i] = Random.Range(0, level);
			obj.GetComponent<VertexController>().ChangeColor(verticesColorType[verticesType[i]]);
			vertices[i] = obj;
		}

		//triangles = new Queue<GameObject> ();
	}

	// Use this for initialization
	void Start () {
		triangles = new Queue<GameObject> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void SetVerticesColorType(int n) {
		verticesColorType = new Color[n];
		for(int i = 0; i < n; i++){
			switch(i){
			case 0:
				verticesColorType[i] = Color.red;
				break;
			case 1:
				verticesColorType[i] = Color.green;
				break;
			case 2:
				verticesColorType [i] = Color.blue;
				break;
			case 3:
				// Orange
				verticesColorType[i] = new Color(243.0f / 255.0f, 152.0f / 255.0f, 0.0f / 255.0f);
				break;
			case 4:
				verticesColorType[i] = Color.white;
				break;
			case 5:
				verticesColorType[i] = Color.magenta;
				break;
			case 6:
				verticesColorType [i] = Color.cyan;
				break;
			case 7:
				// Brown
				//verticesColorType[i] = new Color(109.0f / 255.0f, 76.0f / 255.0f, 51.0f);
				// Black
				verticesColorType[i] = Color.black;
				break;
			default:
				verticesColorType[i] = Color.grey;
				break;
			}
		}
	}

	// return area
	public float CreateTriangle(int v1, int v2, int v3){
		Vector3 g, vg1, vg2, vg3;
		g = (verticesMap [v1] + verticesMap [v2] + verticesMap [v3]) / 3.0f;
		vg1 = g - verticesMap [v1];
		vg2 = g - verticesMap [v2];
		vg3 = g - verticesMap [v3];

		Mesh triMesh = new Mesh ();
		triMesh.name = "EnergyField";
		triMesh.vertices = new Vector3[] {
			verticesMap [v1] + vg1 / vg1.magnitude * 0.5f,
			verticesMap [v2] + vg2 / vg2.magnitude * 0.5f,
			verticesMap [v3] + vg3 / vg3.magnitude * 0.5f,
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
		if (Vector3.Dot (triMesh.normals [0], g) > 0.0f) {
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
		mr.material.SetColor("_EmissionColor", verticesColorType[verticesType[v1]] * 0.3f);
		MeshFilter mf = triangle.AddComponent<MeshFilter> ();
		mf.mesh = triMesh;
		triangles.Enqueue (triangle);

		return 0.5f * Mathf.Sqrt(Vector3.SqrMagnitude(triMesh.vertices[1] - triMesh.vertices[0]) * Vector3.SqrMagnitude(triMesh.vertices[2] - triMesh.vertices[0]) - Mathf.Pow(Vector3.Dot(triMesh.vertices[1] - triMesh.vertices[0], triMesh.vertices[2] - triMesh.vertices[0]), 2.0f));
	}

	public int GetVertexNumber(){
		return this.GetComponent<MeshFilter> ().mesh.vertices.Length;
	}

	public void SetEventTrigger(int v, EventTrigger.Entry entry){
		vertices [v].GetComponent<VertexController> ().SetEventTrigger (entry);
	}

	public bool IsVerticesTypeEqual(int v1, int v2, int v3){
		return (verticesType[v1] == verticesType [v2] && verticesType[v2] == verticesType[v3]) ? true : false;
	}

	public void ParticlePlay(int v){
		vertices [v].GetComponent<VertexController> ().ParticlePlay ();
	}

	public void ParticleStop(int v){
		vertices [v].GetComponent<VertexController> ().ParticleStop ();
	}

	public Vector3 GetVertexPosition(int v){
		return vertices [v].transform.position;
	}

	public Color GetVertexColor(int v){
		return vertices [v].GetComponent<Renderer> ().material.color;
	}

	public void DestroyOldestTriangle(){
		Destroy (triangles.Dequeue ());
	}

	public void RandomVertexTypeChange(int v){
		int type = Random.Range (0, level);
		verticesType [v] = type;
		vertices [v].GetComponent<VertexController> ().ChangeColor (verticesColorType [type]);
	}
}

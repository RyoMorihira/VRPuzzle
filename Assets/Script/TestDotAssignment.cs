using UnityEngine;
using System.Collections;

public class TestDotAssignment : MonoBehaviour {

	private Vector3[] ver;
	public GameObject dot;
	public Material dotMaterial;
	private GameObject[] dots;
	private Mesh mesh;
	private GameObject tri;

	// Use this for initialization
	void Start () {
		ver = this.GetComponent<MeshFilter> ().mesh.vertices;
		dots = new GameObject[ver.Length];
		for (int i = 0; i < ver.Length; i++) {
			Vector3 v = (Vector3)ver[i];
			v.x *= 10;
			v.y *= 10;
			v.z *= 10;
			GameObject obj = Instantiate(dot, v, transform.rotation) as GameObject;
			obj.transform.parent = transform;
			dots[i] = obj;
		}

		dots [0].GetComponent<VertexController> ().ChangeColor (new Color (0.0f, 0.0f, 1.0f));

		mesh = new Mesh ();
		mesh.name = "TestMesh";
		mesh.vertices = new Vector3[]{
			ver[11] * 10.0f,
			ver[12] * 10.0f,
			ver[13] * 10.0f,
		};
		mesh.uv = new Vector2[]{
			new Vector2(0, 0),
			new Vector2(0, 1),
			new Vector2(1, 0),
		};
		mesh.triangles = new int[]{
			0, 1, 2,
		};
		//Color[] colors;
		//colors = mesh.colors;
		//colors[0] = Color.cyan;
		//mesh.colors = colors;
		mesh.RecalculateNormals ();
		mesh.RecalculateBounds ();
		mesh.Optimize ();
		tri = new GameObject ("Triangle");
		MeshRenderer meshrender = tri.AddComponent<MeshRenderer> ();
		meshrender.material = dotMaterial;
		MeshFilter meshfilter = tri.AddComponent<MeshFilter> ();
		meshfilter.mesh = mesh;
		MeshCollider collider = tri.AddComponent<MeshCollider> ();
		collider.sharedMesh = mesh;
	}
	
	// Update is called once per frame
	void Update () {

	}
}

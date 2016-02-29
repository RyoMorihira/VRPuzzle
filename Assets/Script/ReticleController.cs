using UnityEngine;
using System.Collections;

public class ReticleController : MonoBehaviour {

	public GameObject circlePrefab;
	private GameObject[] circle;

	public bool gazeComp = false;
	public bool gazing = false;

	public static bool enableGazing = true;

	// Use this for initialization
	void Start () {
		circle = new GameObject[6];
		for(int i = 0; i < 6; i++){
			GameObject obj = Instantiate (circlePrefab);
			obj.transform.parent = transform;
			obj.transform.localPosition = Vector3.zero;
			obj.transform.localEulerAngles = new Vector3 (0.0f, 0.0f, i * 60.0f);
			obj.transform.localScale = new Vector3 (20.0f, 20.0f, 20.0f);
			circle [i] = obj;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (enableGazing && gazing && circle[0].transform.localEulerAngles.z != 300.0f) {
			gazeComp = false;
			circle [0].transform.localEulerAngles += new Vector3 (0.0f, 0.0f, -300.0f) * Time.deltaTime;
			if (circle [0].transform.localEulerAngles.z <= 60.0f) {
				circle [0].transform.localEulerAngles = new Vector3 (0.0f, 0.0f, 60.0f);
				gazeComp = true;
			}
			if (circle [0].transform.localEulerAngles.z <= 120.0f) {
				circle [2].transform.localEulerAngles = circle [0].transform.localEulerAngles;
			}
			if (circle [0].transform.localEulerAngles.z <= 180.0f) {
				circle [3].transform.localEulerAngles = circle [0].transform.localEulerAngles;
			}
			if (circle [0].transform.localEulerAngles.z <= 240.0f) {
				circle [4].transform.localEulerAngles = circle [0].transform.localEulerAngles;
			}
			if (circle [0].transform.localEulerAngles.z <= 300.0f) {
				// gaze complete
				circle [5].transform.localEulerAngles = circle [0].transform.localEulerAngles;
			}
		} else if((!enableGazing || !gazing) && circle[0].transform.localEulerAngles.z != 0.0f){
			gazeComp = false;
			circle [0].transform.localEulerAngles += new Vector3 (0.0f, 0.0f, 300.0f) * Time.deltaTime;
			if (circle [0].transform.localEulerAngles.z >= 120.0f) {
				circle [2].transform.localEulerAngles = new Vector3(0.0f, 0.0f, 120.0f);
			}
			if (circle [0].transform.localEulerAngles.z >= 180.0f) {
				circle [3].transform.localEulerAngles = new Vector3(0.0f, 0.0f, 180.0f);
			}
			if (circle [0].transform.localEulerAngles.z >= 240.0f) {
				circle [4].transform.localEulerAngles = new Vector3(0.0f, 0.0f, 240.0f);
			}
			if (circle [0].transform.localEulerAngles.z >= 300.0f) {
				circle [5].transform.localEulerAngles = new Vector3(0.0f, 0.0f, 300.0f);
			}
			if (circle [0].transform.localEulerAngles.z <= 30.0f) {
				circle [0].transform.localEulerAngles = new Vector3 (0.0f, 0.0f, 0.0f);
			}
		}
	}

	public void SetGazing(bool b){
		gazing = b;
	}
}

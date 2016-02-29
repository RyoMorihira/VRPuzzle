using UnityEngine;
using System.Collections;

public class RandomRotation : MonoBehaviour {

	public float magnitude = 120.0f;
	private Vector3 angle;
	private Vector3 rotation;

	// Use this for initialization
	void Start () {
		angle.x = Random.value;
		angle.y = Random.value;
		angle.z = Random.value;
		angle *= magnitude / Vector3.Distance (Vector3.zero, angle);
		rotation = transform.localEulerAngles;
	}
	
	// Update is called once per frame
	void Update () {
		rotation += angle * Time.deltaTime;
		transform.localEulerAngles = rotation;
	}
}

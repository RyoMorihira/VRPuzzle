using UnityEngine;
using System.Collections;

public class YawRotation : MonoBehaviour {

	public float angle;

	private Vector3 rotation;

	// Use this for initialization
	void Start () {
		rotation = transform.localEulerAngles;
	}
	
	// Update is called once per frame
	void Update () {
		rotation.y += angle * Time.deltaTime;
		transform.localEulerAngles = rotation;
	}
}

using UnityEngine;
using System.Collections;

public class TextFrameController : MonoBehaviour {

	private float iniTime;
	private TextMesh textMesh;
	private float duration = 3.0f;
	private GameObject center;

	private string se;
	private bool contractionSE = false;

	void Awake(){
		textMesh = transform.FindChild ("Text").GetComponent<TextMesh> ();
	}

	// Use this for initialization
	void Start () {
		iniTime = Time.time;
		/*GameObject center = new GameObject ("Center");
		center.transform.parent = transform.parent;
		center.transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
		center.transform.localPosition = new Vector3 (4.0f, -0.125f, 0.0f);
		center.transform.LookAt (Camera.main.transform);
		transform.rotation = center.transform.rotation;
		Destroy (center);
		transform.localEulerAngles += new Vector3 (0.0f, -90.0f, 0.0f);*/
		se = "SE/TextRobot/DM-CGS-30";
		SoundPlayer.SoundPlay (transform, se, 0.1f, false, 0.5f);
		LookAtCamera ();
	}
	
	// Update is called once per frame
	void Update () {
		LookAtCamera ();
		if (Time.time - iniTime >= 1.0f + duration) {
			Destroy (gameObject);
		} else if (Time.time - iniTime >= 0.5f + duration) {
			if (!contractionSE) {
				SoundPlayer.SoundPlay (transform, se, 0.1f, false, 0.5f);
				contractionSE = true;
			}
			SetScale (1.0f - (Time.time - iniTime - 0.5f - duration) * 2.0f);
		} else if (Time.time - iniTime >= 0.5f) {
		} else {
			SetScale ((Time.time - iniTime) * 2.0f);
		}
	}

	void SetScale(float scale){
		transform.localScale = new Vector3 (scale, scale, scale);
	}

	public void SetText(string str){
		textMesh.text = str;
	}

	public void Close(){
		if (Time.time - iniTime < 0.5f) {
			duration = 0.0f;
		} else if (Time.time - iniTime < 3.5f) {
			duration = Time.time - iniTime - 0.5f;
		}
	}

	public void SetFrmaeColor(Color c){
		Renderer frameRenderer = transform.FindChild ("Horizontal").GetComponent<Renderer> ();
		frameRenderer.material.SetColor ("_EmissionColor", c);
	}

	public void SetTextColor(Color c){
		textMesh.color = c;
	}

	private void LookAtCamera(){
		transform.localEulerAngles = new Vector3 (0.0f, 90.0f, 0.0f);
	}
}

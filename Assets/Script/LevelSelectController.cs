using UnityEngine;
using System.Collections;

public class LevelSelectController : MonoBehaviour {

	private Renderer easyBackRenderer;
	private Renderer easyFrontRenderer;
	private Renderer normalBackRenderer;
	private Renderer normalFrontRenderer;
	private Renderer hardBackRenderer;
	private Renderer hardFrontRenderer;
	private Color baseColor;
	private Color baseColorHard;
	private GameObject easy;
	private GameObject normal;
	private GameObject hard;

	private string selectSE;

	public static int mode;

	// Use this for initialization
	void Start () {
		easy = transform.FindChild ("Easy").gameObject;
		easyBackRenderer = easy.transform.FindChild ("Back").GetComponent<Renderer> ();
		easyFrontRenderer = easy.transform.FindChild ("Front").GetComponent<Renderer> ();
		normal = transform.FindChild ("Normal").gameObject;
		normalBackRenderer = normal.transform.FindChild ("Back").GetComponent<Renderer> ();
		normalFrontRenderer = normal.transform.FindChild ("Front").GetComponent<Renderer> ();
		hard = transform.FindChild ("Hard").gameObject;
		hardBackRenderer = hard.transform.FindChild ("Back").GetComponent<Renderer> ();
		hardFrontRenderer = hard.transform.FindChild ("Front").GetComponent<Renderer> ();
		baseColor = Color.cyan;
		baseColorHard = Color.red;
		SetLevel (mode);
		if (PlayerPrefs.GetInt ("ClearLevel", 0) == 0) {
			// clear level 0: easy or not clear, 1: normal, 2: hard
			hard.SetActive (false);
		}

		selectSE = "SE/Start/Menu_Select_00";
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetLevel(int mode){
		if (mode == 0) {
			// Easy Mode
			easyBackRenderer.material.color = baseColor;
			easyFrontRenderer.material.color = baseColor * 0.75f;
			normalBackRenderer.material.color = baseColor * 0.4f;
			normalFrontRenderer.material.color = baseColor * 0.1f;
			hardBackRenderer.material.color = baseColorHard * 0.4f;
			hardFrontRenderer.material.color = baseColorHard * 0.1f;
			VerticesController.level = 5;
			if (LevelSelectController.mode != mode) {
				SoundPlayer.SoundPlay (easy.transform, selectSE, 1.0f, false, 0.85f);
			}
		} else if (mode == 1) {
			// Normal Mode
			easyBackRenderer.material.color = baseColor * 0.4f;
			easyFrontRenderer.material.color = baseColor * 0.1f;
			normalBackRenderer.material.color = baseColor;
			normalFrontRenderer.material.color = baseColor * 0.75f;
			hardBackRenderer.material.color = baseColorHard * 0.4f;
			hardFrontRenderer.material.color = baseColorHard * 0.1f;
			VerticesController.level = 6;
			if (LevelSelectController.mode != mode) {
				SoundPlayer.SoundPlay (normal.transform, selectSE, 1.0f, false, 0.85f);
			}
		} else if (mode == 2) {
			// Hard Mode
			easyBackRenderer.material.color = baseColor * 0.4f;
			easyFrontRenderer.material.color = baseColor * 0.1f;
			normalBackRenderer.material.color = baseColor * 0.4f;
			normalFrontRenderer.material.color = baseColor * 0.1f;
			hardBackRenderer.material.color = baseColorHard;
			hardFrontRenderer.material.color = baseColorHard * 0.75f;
			VerticesController.level = 7;
			if (LevelSelectController.mode != mode) {
				SoundPlayer.SoundPlay (hard.transform, selectSE, 1.0f, false, 0.85f);
			}
		}
		LevelSelectController.mode = mode;
	}
}

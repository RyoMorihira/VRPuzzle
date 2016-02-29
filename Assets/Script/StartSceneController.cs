using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StartSceneController : MonoBehaviour {

	public static bool first = true;

	public GameObject verticesMap;
	public GameObject reticle;
	public GameObject textRobot;
	public string nextScene;

	private bool gazingStart;
	private VerticesController vc;
	private float flashTime = 0.0f;
	private List<int> particles;
	private List<float> extinctionTimes;
	private bool warpOutFinish = false;

	private string bgmSE;
	private SoundPlayer bgmSoundPlayer;

	// Use this for initialization
	void Start () {
		if (first) {
			first = false;
			TextRobotController.coreColor = Color.blue;
			TextRobotController.textColor = new Color (0.0f, 1.0f, 1.0f);
		} else {
			textRobot.GetComponent<TextRobotController> ().WarpOut (1.0f, new Vector3 (0.0f, 300.0f, 0.0f));
		}
		vc = verticesMap.GetComponent<VerticesController> ();
		particles = new List<int> ();
		extinctionTimes = new List<float> ();

		bgmSE = "BGM/Hope - Loop Free";

		PlayerPrefs.SetFloat ("Version", 1.0f);

		// Debug Only
		//PlayerPrefs.SetInt("ClearLevel", 1);
	}
	
	// Update is called once per frame
	void Update () {
		if(!warpOutFinish && !textRobot.GetComponent<TextRobotController>().warpOut){
			warpOutFinish = true;
			bgmSoundPlayer = SoundPlayer.SoundPlay (transform, bgmSE, 0.1f, true, 0.0f);
		}
		if (gazingStart && reticle.GetComponent<ReticleController> ().gazeComp) {
			textRobot.GetComponent<TextRobotController>().Warp(nextScene, 0);
		}
		if (flashTime < Time.time) {
			flashTime = Random.value * 0.4f + 0.1f + Time.time;
			while (true) {
				int p = Random.Range (0, vc.GetVertexNumber());
				if (!particles.Contains (p)) {
					particles.Add (p);
					vc.ParticlePlay (p);
					break;
				}
			}
			extinctionTimes.Add (Random.value * 0.7f + 0.3f + Time.time);
		}
		if (extinctionTimes.Count > 0 && extinctionTimes [0] < Time.time) {
			vc.ParticleStop (particles [0]);
			particles.RemoveAt (0);
			extinctionTimes.RemoveAt (0);
		}
		if (textRobot.GetComponent<TextRobotController> ().warp && bgmSoundPlayer != null) {
			bgmSoundPlayer.SoundStop ();
		}
	}

	public void SetGazingStart(bool b){
		gazingStart = b;
	}
}

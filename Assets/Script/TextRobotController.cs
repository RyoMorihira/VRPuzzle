using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class TextRobotController : MonoBehaviour {

	public GameObject textFramePrefab;

	private GameObject textRobot;
	private GameObject core;
	private ParticleSystem warpAura;
	private GameObject textFrame = null;
	private bool deployTextFrame = false;
	private string str;
	//private bool emergencyEscape = false;
	private int warpType = 0;
	private float warpPreparationTime = 0.0f;
	private Quaternion baseRotation;
	private Vector3 basePosition;
	public static Color coreColor;
	public static Color textColor;
	public bool warp = false;
	private float warpTime;
	private string nextScene;
	private GameObject imageEffect;
	private Renderer imageEffectRenderer;
	private Vector3 warpLocalPosition;
	private Quaternion warpLocalRotation;
	public bool warpOut = false;
	private bool warpPreparation = false;

	private string warpSE;
	private string warpOutSE;
	private SoundPlayer warpSoundPlayer;
	private bool hasWarpOutSEPlayed;
	private string warningSE;
	private SoundPlayer warningSoundPlayer;
	private string winSE;

	void Awake(){
		textRobot = transform.FindChild ("TextRobot").gameObject;
		core = textRobot.transform.FindChild("Core").gameObject;
		warpAura = core.transform.FindChild ("WarpAura").GetComponent<ParticleSystem> ();
		imageEffect = GameObject.FindGameObjectWithTag ("ImageEffect").gameObject;
		imageEffectRenderer = imageEffect.GetComponent<Renderer> ();
		imageEffect.SetActive (false);
	}

	// Use this for initialization
	void Start () {
		ChangeColor(coreColor, textColor);
		warpLocalPosition = new Vector3 (0.0f, 0.0f, 0.4f);
		warpLocalRotation = Quaternion.Euler (0.0f, 0.0f, 0.0f);

		warpSE = "SE/TextRobot/WarpDrive_01";
		warpOutSE = "SE/TextRobot/UL_FS_High_Whoosh_Zap";
		warningSE = "SE/TextRobot/Alarm_Loop_01";
		winSE = "SE/TextRobot/DM-CGS-45";
	}
	
	// Update is called once per frame
	void Update () {
		if (deployTextFrame && textFrame == null) {
			textFrame = (GameObject)Instantiate (textFramePrefab);
			textFrame.transform.parent = transform.FindChild("TextRobot");
			textFrame.transform.localPosition = Vector3.zero;
			textFrame.transform.localEulerAngles = new Vector3 (0.0f, 90.0f, 0.0f);
			TextFrameController tfc = textFrame.GetComponent<TextFrameController> ();
			tfc.SetText (str);
			tfc.SetTextColor (textColor);
			tfc.SetFrmaeColor (coreColor);
			deployTextFrame = false;
		}

		/*if (emergencyEscape) {
			if (Time.time - escapeTime < 0.5f) {
				transform.localRotation =  Quaternion.Slerp (baseRotation, warpLocalRotation, (Time.time - escapeTime) * 2.0f);
				core.transform.localPosition = basePosition + (warpLocalPosition - basePosition) * (Time.time - escapeTime) * 2.0f;
			} else {
				PresentText ("TARGET IS TOO NEAR!\nEMERGENCY ESCAPE");
				WarpOperation ("Start");
				emergencyEscape = false;
			}
		}*/
		if (warpPreparation) {
			if (Time.time - warpPreparationTime < 0.5f) {
				transform.localRotation =  Quaternion.Slerp (baseRotation, warpLocalRotation, 0.5f - Mathf.Cos((Time.time - warpPreparationTime) * 2.0f * Mathf.PI) * 0.5f);
				textRobot.transform.localPosition = Vector3.Slerp(basePosition, warpLocalPosition, 0.5f - Mathf.Cos((Time.time - warpPreparationTime) * 2.0f * Mathf.PI) * 0.5f);
			} else if (Time.time - warpPreparationTime >= 1.5f) {
				warpPreparation = false;
				switch (warpType) {
				case 0:
					// immidiately warp
					WarpOperation(nextScene);
					break;
				case 1:
					// clear warp
					PresentText ("\nCONGRATULATIONS !\n\nALL TAGETS DESTROYED");
					SoundPlayer.SoundPlay (transform, winSE, 0.5f);
					WarpOperation (nextScene, 3.0f);
					break;
				case 2:
					// emergency warp
					PresentText ("\nTARGET IS TOO NEAR !\n\nEMERGENCY ESCAPE");
					WarpOperation (nextScene, 3.0f);
					break;
				}
			}
		}

		if (warp && warpTime < Time.time) {
			if (Time.time - warpTime < 5.0f) {
				if (warpSoundPlayer == null) {
					warpSoundPlayer = SoundPlayer.SoundPlay (transform, warpSE, 0.6f, false, 0.0f);
				}
				if (warpType == 2 && warningSoundPlayer != null) {
					warningSoundPlayer.SoundStop ();
				}
				warpAura.Emit ((int)((Time.time - warpTime) * 5.0f));
				Color imageColor = new Color (coreColor.r, coreColor.g, coreColor.b, (Time.time - warpTime) / 5.0f);
				imageEffectRenderer.material.color = imageColor;
			} else {
				if (warpSoundPlayer == null) {
					SceneManager.LoadScene (nextScene);
				}
			}
		}

		if (warpOut) {
			if (Time.time - warpTime >= 4.5f) {
				warpOut = false;
				ReticleController.enableGazing = true;
				ChangeColor (coreColor, new Color (0.0f, 1.0f, 1.0f));
				imageEffect.SetActive (false);
			} else if (Time.time - warpTime >= 4.0f) {
				ReticleController.enableGazing = true;
				imageEffectRenderer.material.color = new Color (coreColor.r, coreColor.g, coreColor.b, 0.0f);
				//transform.localRotation = Quaternion.Slerp (warpLocalRotation, baseRotation, (Time.time - warpTime - 4.0f) * 2.0f);
				//core.transform.localPosition = warpLocalPosition + (basePosition - warpLocalPosition) * (Time.time - warpTime - 4.0f) * 2.0f;
				transform.localRotation = Quaternion.Slerp (warpLocalRotation, baseRotation, 0.5f - Mathf.Cos((Time.time - warpTime - 4.0f) * 2.0f * Mathf.PI) * 0.5f);
				textRobot.transform.localPosition = Vector3.Slerp (warpLocalPosition, basePosition, 0.5f - Mathf.Cos ((Time.time - warpTime - 4.0f) * 2.0f * Mathf.PI) * 0.5f);
			} else if (Time.time - warpTime < 2.0f) {
				warpAura.Emit (25);
				imageEffectRenderer.material.color = new Color (coreColor.r, coreColor.g, coreColor.b, 1.0f);
			} else {
				if (!hasWarpOutSEPlayed && Time.time - warpTime > 2.0f) {
					SoundPlayer.SoundPlay (transform, warpOutSE, 0.6f, false, 0.0f);
					hasWarpOutSEPlayed = true;
				}
				imageEffectRenderer.material.color = new Color (coreColor.r, coreColor.g, coreColor.b, 1.0f - (Time.time - warpTime - 2.0f) / 4.0f);
			}
		}
	}

	public void PresentText(string str){
		deployTextFrame = true;
		this.str = str;
		if (textFrame != null) {
			textFrame.GetComponent<TextFrameController> ().Close ();
		}
	}

	/*public void Warp(string nextScene, bool emergency){
		warpPreparation = true;
		emergencyEscape = emergency;
		warpPreparationTime = Time.time;
		baseRotation = transform.localRotation;
		basePosition = core.transform.localPosition;
		imageEffect.SetActive (true);
		this.nextScene = nextScene;
		if (emergency) {
			ChangeColor (Color.red, Color.red);
		} else {
			ChangeColor (Color.blue, new Color (0.0f, 1.0f, 1.0f));
		}
	}*/
	// warpType 0: immidiately warp, 1: clear warp, 2: emergency warp
	public void Warp(string nextScene, int warpType){
		warpPreparation = true;
		this.warpType = warpType;
		warpPreparationTime = Time.time;
		baseRotation = transform.localRotation;
		basePosition = textRobot.transform.localPosition;
		imageEffect.SetActive (true);
		this.nextScene = nextScene;
		switch (warpType) {
		case 0:
			ChangeColor (Color.blue, Color.cyan);
			break;
		case 1:
			ChangeColor (Color.blue, Color.cyan);
			PlayerPrefs.SetInt ("ClearLevel", LevelSelectController.mode);
			break;
		case 2:
			ChangeColor (Color.red, Color.red);
			warningSoundPlayer = SoundPlayer.SoundPlay (transform, warningSE, 0.7f, true);
			break;
		}
	}

	void WarpOperation(string nextScene, float after = 0.0f){
		warp = true;
		this.nextScene = nextScene;
		warpTime = Time.time + after;
		warpAura.startColor = coreColor;
		imageEffectRenderer.material.SetColor ("_EmissionColor", coreColor);
		ReticleController.enableGazing = false;
	}

	void ChangeColor(Color c, Color textCol){
		Renderer coreRenderer = core.GetComponent<Renderer> ();
		coreRenderer.material.color = c;
		coreRenderer.material.SetColor ("_EmissionColor", c * 0.6f);
		ParticleSystem particle = core.transform.FindChild ("VertexAura").GetComponent<ParticleSystem> ();
		particle.startColor = c;
		Renderer textRobotRenderer = transform.FindChild ("TextRobot").GetComponent<Renderer> ();
		textRobotRenderer.material.SetColor ("_EmissionColor", c * 0.5f);
		coreColor = c;
		textColor = textCol;
	}

	public void WarpOut(float distance, Vector3 rotation){
		textRobot.transform.localPosition = warpLocalPosition;
		transform.localRotation = warpLocalRotation;
		warpOut = true;
		warpTime = Time.time;
		warpAura.startColor = coreColor;
		ChangeColor (coreColor, new Color (0.0f, 0.0f, 1.0f));
		imageEffectRenderer.material.SetColor ("_EmissionColor", coreColor);
		ReticleController.enableGazing = false;
		baseRotation = Quaternion.Euler (rotation);
		basePosition = new Vector3 (0.0f, 0.0f, distance);
		imageEffect.SetActive (true);
	}
}

using UnityEngine;
using System.Collections;

public class StartButtonController : MonoBehaviour {

	private Renderer rendererBack;
	private TextMesh textMesh;
	private Color color;
	private Color textColor;
	private bool pointerOver = false;

	public GameObject reticle;

	private string se;
	private SoundPlayer seSoundPlayer;

	// Use this for initialization
	void Start () {
		rendererBack = transform.FindChild ("Back").GetComponent<Renderer> ();
		color = rendererBack.material.GetColor("_EmissionColor");
		textMesh = transform.FindChild ("Text").GetComponent<TextMesh> ();
		textColor = textMesh.color;
		se = "SE/Start/DM-CGS-30";
	}
	
	// Update is called once per frame
	void Update () {
		if (pointerOver && reticle.GetComponent<ReticleController>().gazeComp) {
			rendererBack.material.SetColor ("_EmissionColor", color * 0.3f);
			textMesh.color = textColor * 0.5f;
			SoundPlayer.SoundPlay (transform, se, 1.0f, false, 0.85f);
		}
	
	}

	public void PointerOver(bool b){
		pointerOver = b;
	}
}

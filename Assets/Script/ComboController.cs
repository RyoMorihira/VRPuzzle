using UnityEngine;
using System.Collections;

public class ComboController : MonoBehaviour {

	private TextMesh comboMesh;
	private int combo;

	// Use this for initialization
	void Start () {
		comboMesh = transform.FindChild ("ComboNumber").GetComponent<TextMesh> ();
	}
	
	// Update is called once per frame
	void Update () {
		comboMesh.text = combo.ToString ();
	}

	public int GetCombo(){
		return combo;
	}

	public void AddCombo(){
		combo++;
	}

	public void ResetCombo(){
		combo = 0;
	}
}

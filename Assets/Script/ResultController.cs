using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResultController : MonoBehaviour {

	private GameObject result;
	private TextMesh scoreMesh;
	private TextMesh[] topEasyMesh;
	private TextMesh[] topNormalMesh;
	private TextMesh[] topHardMesh;
	private bool hardMode = false;

	public List<int> topEasy;
	public List<int> topNormal;
	public List<int> topHard;

	// Use this for initialization
	void Start () {
		if (PlayerPrefs.GetInt ("ClearLevel", 0) >= 1) {
			result = transform.FindChild ("ResultHard").gameObject;
			transform.FindChild ("ResultNormal").gameObject.SetActive (false);
			hardMode = true;
		} else {
			result = transform.FindChild ("ResultNormal").gameObject;
			transform.FindChild ("ResultHard").gameObject.SetActive (false);
			hardMode = false;
		}
		topEasy = new List<int> ();
		topNormal = new List<int> ();
		if (hardMode) {
			topHard = new List<int> ();
		}
		scoreMesh = result.transform.FindChild ("Score").GetComponent<TextMesh> ();
		topEasyMesh = new TextMesh[5];
		topNormalMesh = new TextMesh[5];
		if (hardMode) {
			topHardMesh = new TextMesh[5];
		}
		scoreMesh.text = ScoreController.score.ToString ();
		for (int i = 0; i < 5; i++) {
			topEasyMesh [i] = result.transform.FindChild ("TopEasy" + (i + 1)).GetComponent<TextMesh> ();
			topEasy.Add (PlayerPrefs.GetInt ("TopEasy" + (i + 1), 0));
			topNormalMesh [i] = result.transform.FindChild ("TopNormal" + (i + 1)).GetComponent<TextMesh> ();
			topNormal.Add (PlayerPrefs.GetInt ("TopNormal" + (i + 1), 0));
			if (hardMode) {
				topHardMesh [i] = result.transform.FindChild ("TopHard" + (i + 1)).GetComponent<TextMesh> ();
				topHard.Add (PlayerPrefs.GetInt ("TopHard" + (i + 1), 0));
			}
		}
		if (LevelSelectController.mode == 0) {
			topEasy.Add (ScoreController.score);
		}else if(LevelSelectController.mode == 1){
			topNormal.Add (ScoreController.score);
		} else if (LevelSelectController.mode == 2) {
			topHard.Add (ScoreController.score);
		}
		topEasy.Sort ();
		topEasy.Reverse ();
		topNormal.Sort ();
		topNormal.Reverse ();
		if (hardMode) {
			topHard.Sort ();
			topHard.Reverse ();
		}
		for (int i = 0; i < 5; i++) {
			topEasyMesh [i].text = topEasy [i].ToString ();
			string str = "TopEasy" + (i + 1);
			PlayerPrefs.SetInt (str, topEasy [i]);
			topNormalMesh [i].text = topNormal [i].ToString ();
			str = "TopNormal" + (i + 1);
			PlayerPrefs.SetInt (str, topNormal [i]);
			if (hardMode) {
				topHardMesh [i].text = topHard [i].ToString ();
				str = "TopHard" + (i + 1);
				PlayerPrefs.SetInt (str, topHard [i]);
			}
		}
		PlayerPrefs.Save ();
		ScoreController.ScoreReset ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

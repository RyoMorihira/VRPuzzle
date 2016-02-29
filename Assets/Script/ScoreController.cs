using UnityEngine;
using System.Collections;

public class ScoreController : MonoBehaviour {

	private TextMesh scoreMesh;
	//private int score = 0;
	private int currentScore = 0;

	public static int score = 0;

	// Use this for initialization
	void Start () {
		scoreMesh = transform.FindChild ("ScoreNumber").GetComponent<TextMesh> ();
		SetScore (currentScore);
	}
	
	// Update is called once per frame
	void Update () {
		if (currentScore < score) {
			currentScore += (int)(2000.0f * Time.deltaTime);
			if (currentScore > score) {
				currentScore = score;
			}
			SetScore (currentScore);
		}
	}

	public void AddScore(int score){
		ScoreController.score += score;
	}

	void SetScore(int num){
		scoreMesh.text = num.ToString();
	}

	public static void ScoreReset(){
		score = 0;
	}
}

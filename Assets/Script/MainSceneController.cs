using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class MainSceneController : MonoBehaviour {

	public GameObject verticesMap;
	public GameObject reticle;
	public GameObject energySupply;
	public GameObject laserMachine;
	public GameObject enemyPrefab1;
	public GameObject enemyPrefab2;
	public GameObject textRobot;
	public GameObject score;
	public GameObject combo;

	private bool vertexSelectEnable;
	public int gazing = -1;
	public List<int> selectVertices;
	private List<List<int>> activeVertices;
	private VerticesController vc;
	private LaserMachineController lmc;
	private float chargingDuration = 10.0f;
	private List<float> chargeCompTimes;
	private List<GameObject> energySupplies;
	private List<GameObject> stopedEnergySupplies;
	private GameObject enemy;
	private float enemyHitPoint = 0.0f;
	private int enemyNumber = 0;
	private TextRobotController trc;
	private bool chargeComplete = false;
	private bool warpOutFinish = false;
	private bool warp = false;
	private ComboController cc;

	private string energyBallShotStarterSE;
	private string energySupplySE;
	private List<SoundPlayer> energySupplySoundPlayers;
	private string bgmSE;
	private SoundPlayer bgmSoundPlayer;

	// Use this for initialization
	void Start () {
		VerticesSetEventTrigger ();

		selectVertices = new List<int> ();
		activeVertices = new List<List<int>> ();
		chargeCompTimes = new List<float> ();
		energySupplies = new List<GameObject> ();
		stopedEnergySupplies = new List<GameObject> ();
		lmc = laserMachine.GetComponent<LaserMachineController> ();
		trc = textRobot.GetComponent<TextRobotController> ();
		cc = combo.GetComponent<ComboController> ();

		vertexSelectEnable = true;
		trc.WarpOut (0.5f, new Vector3 (20.0f, 325.0f, 0.0f));

		energyBallShotStarterSE = "SE/Laser/UL_Starter_11";
		energySupplySE = "SE/Laser/UL_Starter_10";
		energySupplySoundPlayers = new List<SoundPlayer> ();
		//bgmSE = "BGM/Within - Free Loop";
		bgmSE = "BGM/Stars";
	}

	// Update is called once per frame
	void Update () {
		if (!warpOutFinish && !trc.warpOut) {
			warpOutFinish = true;

			lmc.SetEnergyBall ();
			SetEnemy ();
			lmc.FaceEnemy ();

			trc.PresentText ("\n\nTARGET APPROACHING");

			bgmSoundPlayer = SoundPlayer.SoundPlay (transform, bgmSE, 0.15f, true, 0.0f);
		}

		// Select Vertex
		if (gazing != -1 && vertexSelectEnable && reticle.GetComponent<ReticleController> ().gazeComp && !selectVertices.Contains (gazing) && !IsActiveVertex(gazing)) {
			if (selectVertices.Count == 2) {
				if (vc.IsVerticesTypeEqual (selectVertices [0], selectVertices [1], gazing)) {
					vc.ParticlePlay (gazing);
					float suppliedEnergy = vc.CreateTriangle (selectVertices [0], selectVertices [1], gazing);
					lmc.SupplyEnergy (suppliedEnergy, chargingDuration);
					cc.AddCombo ();
					score.GetComponent<ScoreController> ().AddScore ((int)(suppliedEnergy * 100.0f * ((cc.GetCombo() - 1) / 20.0f + 1.0f)));
					selectVertices.Add (gazing);
					List<int> list = new List<int> ();
					for (int i = 0; i < selectVertices.Count; i++) {
						list.Add (selectVertices [i]);
					}
					activeVertices.Add (list);
					chargeCompTimes.Add (Time.time + chargingDuration);
					Vector3 g = (vc.GetVertexPosition (selectVertices [0]) + vc.GetVertexPosition (selectVertices [1]) + vc.GetVertexPosition (gazing)) / 3.0f;
					SetEnergySupply (g, lmc.GetDownsidePosition(), vc.GetVertexColor (gazing));
					energySupplySoundPlayers.Add(SoundPlayer.SoundPlay(g, energySupplySE, suppliedEnergy / 200.0f, true));
					if (lmc.GetEnergy () > enemyHitPoint) {
						vertexSelectEnable = false;
						trc.PresentText ("\n\nFULLY CHARGED SOON");
						//Singleton<SoundPlayer>.instance.playSEOneShot ("EnergyBallShotStarter");
						SoundPlayer.SoundPlay(transform, energyBallShotStarterSE, Mathf.Log(lmc.GetEnergy() / 13.0f) / Mathf.Log(1000.0f / 13.0f));
						chargeComplete = true;
						ChargeCompTimeAllReset (Time.time + 3.0f);
					}
				} else {
					vc.ParticleStop (selectVertices [0]);
					vc.ParticleStop (selectVertices [1]);
				}
				selectVertices.Clear ();
			} else {
				selectVertices.Add (gazing);
				vc.ParticlePlay (gazing);
			}
		}

		// Destroy energy supply items
		if (chargeCompTimes.Count > 0 && chargeCompTimes [0] < Time.time) {
			vc.DestroyOldestTriangle ();
			chargeCompTimes.RemoveAt (0);
			energySupplies [0].GetComponent<ParticleSystem> ().Stop ();
			stopedEnergySupplies.Add (energySupplies [0]);
			energySupplies.RemoveAt (0);
			energySupplySoundPlayers [0].SoundStop ();
			energySupplySoundPlayers.RemoveAt (0);
			if (stopedEnergySupplies.Count < 4) {
				Destroy (stopedEnergySupplies [0]);
				stopedEnergySupplies.RemoveAt (0);
			}
			for (int i = 0; i < activeVertices [0].Count; i++) {
				vc.ParticleStop (activeVertices [0] [i]);
				vc.RandomVertexTypeChange (activeVertices [0] [i]);
			}
			activeVertices.RemoveAt (0);
			if(!chargeComplete && chargeCompTimes.Count == 0){
				cc.ResetCombo();
			}
		}
		if (chargeComplete && chargeCompTimes.Count == 0 && !lmc.IsEnergyBallFired() && lmc.GetEnergy() > 0.0f) {
			while (stopedEnergySupplies.Count > 0 && !stopedEnergySupplies [0].GetComponent<ParticleSystem> ().IsAlive (false)) {
				Destroy (stopedEnergySupplies [0]);
				stopedEnergySupplies.RemoveAt (0);
			}
			lmc.FireEnergyBall ();
		}

		// enemy is too near. Escape!
		if (!chargeComplete && !warp && enemy != null && enemy.GetComponent<EnemyController>().CollisionPeriod() < 12.0f) {
			vertexSelectEnable = false;
			lmc.SetEnergyBallCollider (false);
			trc.Warp ("Start", 2);
			bgmSoundPlayer.SoundStop ();
			warp = true;
		}

		// enemy has been destroyed!
		if (!vertexSelectEnable && enemy == null && !warp) {
			chargeComplete = false;
			if (enemyNumber == 5) {
				// Clear!
				bgmSoundPlayer.SoundStop();
				trc.Warp("START", 1);
				warp = true;
			} else {
				trc.PresentText ("\nTARGET DESTROYED !\n\nNEXT TARGET APPROACHING");
				vertexSelectEnable = true;
				lmc.SetEnergyBall ();
				SetEnemy ();
				lmc.FaceEnemy ();
			}
		}
	}

	void VerticesSetEventTrigger(){
		vc = verticesMap.GetComponent<VerticesController> ();
		for (int i = 0; i < vc.GetVertexNumber (); i++) {
			EventTrigger.Entry entryEnter = new EventTrigger.Entry ();
			entryEnter.eventID = EventTriggerType.PointerEnter;
			int a = i;
			entryEnter.callback.AddListener ( (x) => { this.SetGazingVertex(a); } );
			vc.SetEventTrigger (i, entryEnter);

			EventTrigger.Entry entryExit = new EventTrigger.Entry ();
			entryExit.eventID = EventTriggerType.PointerExit;
			entryExit.callback.AddListener ( (x) => { this.SetGazingVertex(-1); } );
			vc.SetEventTrigger (i, entryExit);

			EventTrigger.Entry reticleEnter = new EventTrigger.Entry ();
			reticleEnter.eventID = EventTriggerType.PointerEnter;
			reticleEnter.callback.AddListener ( (y) => { reticle.GetComponent<ReticleController>().SetGazing(true); } );
			vc.SetEventTrigger (i, reticleEnter);

			EventTrigger.Entry reticleExit = new EventTrigger.Entry ();
			reticleExit.eventID = EventTriggerType.PointerExit;
			reticleExit.callback.AddListener ( (y) => { reticle.GetComponent<ReticleController>().SetGazing(false); } );
			vc.SetEventTrigger (i, reticleExit);
		}
	}

	public void SetGazingVertex(int v){
		gazing = v;
	}

	void SetEnergySupply(Vector3 from, Vector3 to, Color c){
		GameObject supply = (GameObject)Instantiate (energySupply, from, transform.rotation);
		supply.transform.LookAt (to);
		supply.transform.localEulerAngles = supply.transform.localEulerAngles + new Vector3 (90, 0, 0);
		ParticleSystem particle = supply.GetComponent<ParticleSystem> ();
		particle.startLifetime = Vector3.Magnitude (to - from) / particle.startSpeed;
		particle.startColor = c;
		energySupplies.Add (supply);
	}

	bool IsActiveVertex(int v){
		for(int i = 0; i < activeVertices.Count; i++){
			for (int j = 0; j < activeVertices[i].Count; j++) {
				if (activeVertices[i][j] == v)
					return true;
			}
		}
		return false;
	}

	void SetEnemy(){
		Vector3 enemyPosition;
		enemyNumber++;
		enemyPosition = new Vector3 (Random.value - 0.5f, Random.value - 0.5f, Random.value - 0.5f);
		enemyPosition = enemyPosition / enemyPosition.magnitude * 10000.0f;
		if (enemyNumber < 5) {
		//if(false){
			enemy = (GameObject)Instantiate (enemyPrefab2, enemyPosition, transform.rotation);
			enemy.GetComponent<EnemyController> ().SetDuration (100.0f);		// -12s
			//enemy.GetComponent<EnemyController> ().SetDuration (20.0f);
			//enemy.GetComponent<EnemyController> ().SetSize (140.0f);	// scale = 20
			//enemy.GetComponent<EnemyController> ().SetSize (280.0f);	// scale = 40
			enemy.GetComponent<EnemyController> ().SetSize (700.0f);	// scale = 100
			switch(VerticesController.level){
			case 5:
				// Easy Mode
				enemyHitPoint = 270.0f * (1.0f + enemyNumber / 10.0f);
				break;
			case 6:
				// Normal Mode
				enemyHitPoint = 340.0f * (1.0f + enemyNumber / 10.0f);
				break;
			case 7:
				// Hard Mode
				enemyHitPoint = 400.0f * (1.0f + enemyNumber / 10.0f);
				break;
			}
			//enemyHitPoint = 10.0f;
		} else {
			enemy = (GameObject)Instantiate (enemyPrefab1, enemyPosition, transform.rotation);
			enemy.GetComponent<EnemyController> ().SetDuration (160.0f);
			//enemy.GetComponent<EnemyController> ().SetSize(160.0f);		// scale = 80
			//enemy.GetComponent<EnemyController> ().SetSize(400.0f);		// scale = 200
			enemy.GetComponent<EnemyController> ().SetSize(1000.0f);		// scale = 500
			switch(VerticesController.level){
			case 5:
				// Easy Mode
				enemyHitPoint = 700.0f;
				break;
			case 6:
				// Normal Mode
				enemyHitPoint = 850.0f;
				break;
			case 7:
				// Hard Mode
				enemyHitPoint = 1000.0f;
				break;
			}
			//enemyHitPoint = 10.0f;
		}
		lmc.SetEnemy (enemy);
	}

	void ChargeCompTimeAllReset(float time){
		for (int i = 0; i < chargeCompTimes.Count; i++) {
			chargeCompTimes [i] = time;
		}
	}
}

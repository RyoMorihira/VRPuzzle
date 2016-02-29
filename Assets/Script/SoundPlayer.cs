using UnityEngine;
using System.Collections;

public class SoundPlayer : MonoBehaviour {

	private AudioSource audioSource;
	private bool played = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (played && !audioSource.isPlaying) {
			Destroy (gameObject);
		}
	}

	public static SoundPlayer SoundPlay(Transform parent, string seName, float volume = 1.0f, bool loop = false, float stereo = 1.0f){
		GameObject obj = new GameObject ("SoundPlayer");
		obj.transform.parent = parent;
		SoundPlayer soundPlayer = obj.AddComponent<SoundPlayer> ();
		obj.AddComponent<AudioSource> ();
		soundPlayer.Initialize (seName, volume, loop, stereo);
		return soundPlayer;
	}

	public static SoundPlayer SoundPlay(Vector3 position, string seName, float volume = 1.0f, bool loop = false, float stereo = 1.0f){
		GameObject obj = new GameObject ("SoundPlayer");
		obj.transform.position = position;
		SoundPlayer soundPlayer = obj.AddComponent<SoundPlayer> ();
		obj.AddComponent<AudioSource> ();
		soundPlayer.Initialize (seName, volume, loop, stereo);
		return soundPlayer;
	}

	public void Initialize(string seName, float volume, bool loop = false, float stereo = 1.0f){
		audioSource = GetComponent<AudioSource> ();
		audioSource.loop = loop;
		audioSource.spatialBlend = stereo;
		audioSource.volume = volume;
		AudioClip clip = (AudioClip)Resources.Load (seName);
		audioSource.clip = clip;
		audioSource.Play ();
		played = true;
	}

	public void SoundStop(){
		audioSource.Stop ();
	}

	public void SetVolume(float volume){
		audioSource.volume = volume;
	}
}

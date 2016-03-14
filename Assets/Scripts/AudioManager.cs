using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

	AudioSource audioSource;
	
	void Start () {
		audioSource = GetComponents<AudioSource> ()[0];
	}

	public void playMenuClick() {
		audioSource.Play ();
	}
}

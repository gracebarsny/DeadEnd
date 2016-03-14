using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	
	void Update () {
		// Reset game
		if (Input.GetKeyDown (KeyCode.KeypadEnter)) {
			Application.LoadLevel(1);
		}
	}
}

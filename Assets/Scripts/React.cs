using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class React : MonoBehaviour {
	
	public GameObject girlModel;

	Animation anim;

	string currentClip;
	string transitionClip;
	string lastEmotion;
	
	void Start () {
		anim = girlModel.GetComponent<Animation> ();
		lastEmotion = "";
	}
	
	public void showReaction(string emotion) {
		if (emotion != lastEmotion) {
			if (transitionClip != null) {
				anim.Play(transitionClip);
			}
			switch (emotion) {
			case "aggressive":
				currentClip = "Aggressive";
				break;
			case "sad":
				currentClip = "Sad";
				break;
			case "fearful":
				currentClip = "Fearful";
				break;
			case "concerned":
				currentClip = "Concerned";
				break;
			case "surprised":
				currentClip = "Surprised";
				break;
			default:
				currentClip = "Pleasant";
				break;
			}
			anim.PlayQueued(currentClip, QueueMode.CompleteOthers);
			transitionClip = currentClip + " Ending";
			lastEmotion = emotion;
		}
	}
}
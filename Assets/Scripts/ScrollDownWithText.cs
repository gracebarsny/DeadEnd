using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class ScrollDownWithText : MonoBehaviour {

	Scrollbar scrollbar;

	void Start() {
		scrollbar = GetComponent<Scrollbar> ();
		scrollbar.value = 0;
	}

	/*
	 * This is called from the scrollbar's OnValueChanged function 
	 * It has the scrollbar follow the text down as it fills the chatbox
	 * As long as the user isn't manually adjusting it
	 */
	public void scrollDownWithText() {
		if (! Input.GetMouseButton (0)) {
			scrollbar.value = 0;
		}
	}
}

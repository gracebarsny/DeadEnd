using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CommandBank : MonoBehaviour {

	Dropdown commandDropdown;
	public static CommandBank commandBank;

	void Awake() {
		commandBank = this;
		commandDropdown = GetComponent<Dropdown> ();
	}

	public void add(string[] keywords) {
		if (keywords != null) {
			List<Dropdown.OptionData> options = commandDropdown.options;
			for (int i = 0; i < keywords.Length; i++) {
				string currentKeyword = keywords [i];
				if (currentKeyword != "") {					
					Dropdown.OptionData command = new Dropdown.OptionData (currentKeyword);
					if (! options.Contains (command)) {
						options.Add (command);
					}
				}
			}
		}
	}
}

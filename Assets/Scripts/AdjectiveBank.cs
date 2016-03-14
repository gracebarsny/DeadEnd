using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class AdjectiveBank : MonoBehaviour {

	public static AdjectiveBank adjectiveBank;

	Text keywords;
	public Dropdown adjectiveDropdown;

	string[] newKeywords;

	void Awake() {
		adjectiveBank = this;
		keywords = GetComponent<Text> ();
	}

	void Start() {
		keywords.text = "";
	}

	public void addAdjectives(string[] adjectives) {
		// If there are new adjectives
		newKeywords = adjectives;
		if (adjectives != null) {
			List<Dropdown.OptionData> options = adjectiveDropdown.options;
			for (int i = 0; i < adjectives.Length; i++) {
				Dropdown.OptionData adj = new Dropdown.OptionData (adjectives [i]);
				// If the adjective is not an option in the Adjective Dropdown
				if (! options.Contains (adj)) {
					// Add the adjective to the Adjective dropdown
					options.Add (adj);
				}
			}
		}
	}

	public void displayNewKeywords() {
		if (newKeywords != null) {
			string keywordsList = keywords.text;
			for (int i = 0; i < newKeywords.Length; i++) {
				string newKeyword = newKeywords[i];
				if (newKeyword != "" && ! keywordsList.Contains(newKeyword)) {
					if (keywords.text != "") {
						keywords.text += "\n";
					}
					keywords.text += "- " + newKeyword;
				}
			}
			adjectiveBank.clearNewKeywords ();
		}
	}

	public void clearNewKeywords() {
		newKeywords = null;
	}
}

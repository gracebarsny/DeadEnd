/*
 * Attach to "List of Keywords" object under "Word Bank" object 
 * 
 * Displays subject keywords in Word Bank
 * Adds subject keywords to Subject Dropdown
 */
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class WordBank : MonoBehaviour {
	
	public Dropdown subjectDropdown;

	public static WordBank wordBank;

	Text keywords; // Text component in WordBank GameObject
	ArrayList keywordsList;
	string[] newKeywords;
	
	void Awake () {
		wordBank = this;
		keywords = GetComponent<Text> ();

		keywordsList = new ArrayList ();
		newKeywords = null;
	}

	void Start() {
		keywords.text = ""; // Clear what is displayed in the WordBank GameObject
	}

	public void add(string[] keywords) {
		newKeywords = keywords;
		List<Dropdown.OptionData> options = subjectDropdown.options;
		for (int i = 0; i < keywords.Length; i++) {
			string currentKeyword = keywords[i];
			if (currentKeyword != "") {
				string currentKeywordLowercase = currentKeyword.ToLower();
				if (! keywordsList.Contains(currentKeywordLowercase)) {
					// Add to word bank
					keywordsList.Add (currentKeywordLowercase);

					Dropdown.OptionData subject = new Dropdown.OptionData (currentKeyword);
					if (! options.Contains (subject)) {
						options.Add (subject);
					}
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
			wordBank.clearNewKeywords ();
		}
	}

	public void clearNewKeywords() {
		newKeywords = null;
	}

	public string[] NewKeywords {
		get { return newKeywords; }
	}

	public ArrayList KeywordsList {
		get { return keywordsList; }
	}
}

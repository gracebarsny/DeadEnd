using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class NPC : MonoBehaviour {

	TwineParser twineParser;
	React react;

	// Passage Variables
	ArrayList currentPassageInfo;
	string currentPassageTitle;
	string[] responses;
	string[] newSubjects;
	string[] newAdjectives;
	string[] newCommands;
	string emotion;

	string inputToBeChanged;
	string[] hintInfo;

	// Keyword Banks
	WordBank wordBank;
	AdjectiveBank adjectiveBank;
	CommandBank commandBank;

	void Start() {
		twineParser = GetComponent<TwineParser> ();
		wordBank = WordBank.wordBank;
		adjectiveBank = AdjectiveBank.adjectiveBank;
		commandBank = CommandBank.commandBank;
		react = GetComponent<React> ();
	}

	public bool getResponse(string passageTitle) {
		currentPassageTitle = passageTitle;
		if (twineParser.Titles.Contains(passageTitle)) {
			currentPassageInfo = twineParser.getPassageInfo(passageTitle);
			updateInfo ();
			return true;
		}
		return false;
	}

	public bool getResponse(string command, string subject, string adjective, string tone) {

		ArrayList possiblePaths = twineParser.AllPaths;

		// Counts backwards to allow preference for newest paths
		for (int p = possiblePaths.Count - 1; p >= 0; p--) {
			ArrayList path = (ArrayList) possiblePaths[p];
			string c = (string) path[0];
			if (c != command) {
				continue;
			}

			string s = (string) path[1];
			if ( s != subject) {
				continue;
			}

			string a = (string) path[2];
			if (a != adjective) {
				continue;
			}

			string t = (string) path[3];
			if (t != tone) {
				continue;
			}

			string nextPassageTitle = (string) path[4];
			getResponse(nextPassageTitle);
			return true;
		}
		findClosestMatch (possiblePaths, command, subject, adjective, tone);
		return false;
	}

	void findClosestMatch(ArrayList possiblePaths, string command, string subject, string adjective, string tone) {
		for (int p = possiblePaths.Count - 1; p >= 0; p--) {
			ArrayList path = (ArrayList)possiblePaths [p];
			inputToBeChanged = "";
			string c = (string)path [0];
			if (c != command) {
				inputToBeChanged = "command";
				//continue;
			}
			
			string s = (string)path [1];
			if (s != subject) {
				if (inputToBeChanged != "") {
					continue;
				}
				inputToBeChanged = "subject";
			}
			
			string a = (string)path [2];
			if (a != adjective) {
				if (inputToBeChanged != "") {
					continue;
				}
				inputToBeChanged = "adjective";
			}
			
			string t = (string)path [3];
			if (t != tone) {
				if (inputToBeChanged != "") {
					continue;
				}
				inputToBeChanged = "tone";
			}
			
			if (inputToBeChanged != "") {
				getHint (inputToBeChanged);
				return;
			}
		}
	}

	void getHint(string inputToBeChanged) {
		string hint = "";
		switch (inputToBeChanged) {
		case "command":
			hint = "\"Is that supposed to be a question?\"";
			break;
		case "subject":
			hint = "\"Hmm, I don't really want to talk about that...\"";
			break;
		case "adjective":
			hint = "\"Could you be a little more descriptive?\"";
			break;
		case "tone":
			hint = "\"Why are you using that tone with me?\"";
			break;
		}
		hintInfo = new string[2] {inputToBeChanged, hint};
	}

	public string getSuggestedSentence(string command, string subject, string adjective, string tone) {
		ArrayList possiblePaths = twineParser.AllPaths;
		ArrayList suggestedSentence = twineParser.AllSuggestedSentences;

		// Counts backwards to allow preference for newest paths
		for (int p = possiblePaths.Count - 1; p >= 0; p--) {
			ArrayList path = (ArrayList) possiblePaths[p];
			string c = (string) path[0];
			if (c != command) {
				continue;
			}
			
			string s = (string) path[1];
			if ( s != subject) {
				continue;
			}
			
			string a = (string) path[2];
			if (a != adjective) {
				continue;
			}
			
			string t = (string) path[3];
			if (t != tone) {
				continue;
			}

			string suggestion = (string) suggestedSentence[p];

			if (suggestion == "") {
				suggestion = "Suggestion" + p;
			}
			return suggestion.Trim ();
		}
		return "(No suggestion)";

	}

	void updateInfo() {
		responses = (string[]) currentPassageInfo [0];

		newSubjects = (string[]) currentPassageInfo [1];
		wordBank.add (newSubjects);

		newAdjectives = (string[]) currentPassageInfo [2];
		adjectiveBank.addAdjectives (newAdjectives);

		emotion = (string) currentPassageInfo [4];
		if (react != null) {
			react.showReaction (emotion);
		}

		newCommands = (string[]) currentPassageInfo [6];
		commandBank.add (newCommands);
	}

	public string[] HintInfo {
		get { return hintInfo; }
	}

	public string[] Responses {
		get { return responses; }
	}

	public string CurrentPassageTitle {
		get { return currentPassageTitle; }
	}

	public TwineParser TwineParser {
		get { return twineParser; }
	}
}
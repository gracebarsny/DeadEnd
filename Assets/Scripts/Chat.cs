using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Chat : MonoBehaviour {

	public Button submitButton;

	public GameObject chatboxContent;
	Text messageHistory;

	public GameObject nonPlayerCharacter;
	NPC npc;
	string[] npcResponses;

	InputChecker inputChecker;

	string[] defaultMessage;

	//bool finishedDisplaying;

	void Awake() {
		messageHistory = chatboxContent.GetComponent<Text> ();
		npc = nonPlayerCharacter.GetComponent<NPC> ();
		inputChecker = GetComponent<InputChecker> ();
	}

	void Start() {
		// Displays nothing
		messageHistory.text = "";

		getDialogueByPassageTitle ("Root");

		inputChecker.updateInputs ();
		inputChecker.showInputChoices ();

		defaultMessage = new string[1] {"She doesn't seem to be responding to that."};
	}

	public void submitInputs(string command, string subject, string adjective, string tone, string sentence) {
		getResponses (command, subject, adjective, tone, sentence);

		// Display the current input
		string newMessage = "";
		newMessage += ("<size=18>" + command + " <b>" + subject + "</b> ");
		if (adjective != "") {
			newMessage += (adjective + " ");
		}
		newMessage += (tone + "</size>\n<size=25>YOU: " + sentence + "</size>\n");

		// Show message
		messageHistory.text += newMessage;
	}

	public void getDialogueByPassageTitle(string passageTitle) {
		if (npc.getResponse (passageTitle)) {
			npcResponses = npc.Responses;
		}

		StartCoroutine(waitAndDisplay(1.5f));
	}

	void getResponses(string command, string subject, string adjective, string tone, string sentence) {

		string passageTitle;

		// If the NPC has a response for the given input
		if (npc.getResponse (command, subject, adjective, tone)) {
			npcResponses = npc.Responses;
			passageTitle = npc.CurrentPassageTitle;
			//inputChecker.hideNotificationPanel();
			inputChecker.clearInput();
		} else {
			/*string[] hintInfo = npc.HintInfo;
			npcResponses = new string[1] {hintInfo[1]};
			inputChecker.showHint(hintInfo[0]);
			*/
			npcResponses = defaultMessage;
			passageTitle = "NONE";
		}

		StartCoroutine(waitAndDisplay(1.5f));

		storeInputOnWebpage(passageTitle, command, subject, adjective, tone, sentence);
	}

	IEnumerator waitAndDisplay(float waitTime) {
		submitButton.interactable = false;
		//finishedDisplaying = false;
		// For each response
		for (int i = 0; i < npcResponses.Length; i++) {
			// Pause
			yield return new WaitForSeconds(waitTime);
			// Display response
			messageHistory.text += npcResponses[i] + "\n";
			if (npcResponses[i] == "<i>Starting...</i>") {
				yield return new WaitForSeconds(waitTime);
				Application.LoadLevel(2);
			}
		}
		if (npcResponses[0] == "\"Good riddance!\"") {
			yield return new WaitForSeconds(waitTime);
			Application.LoadLevel(3);
		}

		if (npcResponses == defaultMessage) {
			submitButton.interactable = true;
		}

		// Display new keywords
		WordBank.wordBank.displayNewKeywords ();
		AdjectiveBank.adjectiveBank.displayNewKeywords ();
		//finishedDisplaying = true;
	}

	public void add(string text) {
		messageHistory.text += text;
	}

	/*
	 * This function is for playtesting purposes. 
	 * It calls an external JavaScript function on the webpage displaying the build.
	 * Allows for the browser to get the user's input from the game.
	 */
	void storeInputOnWebpage(string passageTitle, string command, string subject, string adjective, string tone, string sentence) {
		string displayString = passageTitle + ": " + command + " " + subject + " " + adjective + " " + tone + " - " + sentence + "\n\n";
		Application.ExternalCall ("AddText", displayString);
	}

	public NPC NPC {
		get { return npc; }
	}
}

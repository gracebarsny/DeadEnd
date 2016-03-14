/*
 * Attached to Canvas
 * 
 * 
 */

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class InputChecker : MonoBehaviour {

	public Toggle suggestionToggle;
	public Button submitButton;

	// Get inputs
	Dropdown[] dropdowns;
	public Dropdown command;
	public Dropdown subject;
	public Dropdown adjective;
	public Dropdown tone;
	public InputField sentence;

	// Get image components
	Image commandImage;
	Image subjectImage;
	Image adjectiveImage;
	Image toneImage;
	Image sentenceImage;

	// Get labels
	public Text commandLabel;
	public Text subjectLabel;
	public Text adjectiveLabel;
	public Text toneLabel;
	Text sentenceLabel;

	// Variables for label text
	string[] labels;
	string commandLabelText;
	string subjectLabelText;
	string adjectiveLabelText;
	string toneLabelText;
	string sentenceLabelText;

	bool commandEntered;
	bool subjectEntered;
	bool adjectiveEntered;
	bool toneEntered;
	bool sentenceEntered;

	string[] inputs;
	string commandInput;
	string subjectInput;
	string adjectiveInput;
	string toneInput;
	string sentenceInput;

	bool suggestionsOn = true;

	// Get feedback components
	public GameObject notifications;
	
	public Text notificationText1;

	public GameObject notificationPanel2;
	Text notificationText2;
	
	bool invalidCombo = false;

	// ArrayList of invalid inputs
	ArrayList invalidInputs;
	ArrayList invalidInputLabels;

	Color invalidInputColor;

	ArrayList newInputs;
	Chat chat;

	NPC npc;
	TwineParser twineParser;

	void Awake() {
		dropdowns = new Dropdown[4] {command, subject, adjective, tone};
		chat = GetComponent<Chat>();
	}

	void Start() {
		//hideNotificationPanel ();
		commandLabelText = commandLabel.text;
		subjectLabelText = subjectLabel.text;
		adjectiveLabelText = adjectiveLabel.text;
		toneLabelText = toneLabel.text;

		labels = new string[4] {commandLabelText, subjectLabelText, adjectiveLabelText, toneLabelText}; 

		sentenceLabelText = sentence.placeholder.GetComponent<Text> ().text;

		commandImage = command.image;
		subjectImage = subject.image;
		adjectiveImage = adjective.image;
		toneImage = tone.image;
		sentenceImage = sentence.image;

		invalidInputs = new ArrayList ();
		invalidInputLabels = new ArrayList ();

		invalidInputColor = new Color (1.0f, 0.83f, 0.83f);

		newInputs = new ArrayList ();

		npc = chat.NPC;
		twineParser = npc.TwineParser;
	}

	void Update() {
		if (submitButton.interactable && Input.GetKeyDown(KeyCode.Return)) {
			checkInput();
		}
	}

	public void checkInput() {
		// Clear previous ArrayList, if there is one
		if (invalidInputs != null || invalidInputLabels != null) {
			invalidInputs.Clear ();
			invalidInputLabels.Clear ();
		}

		updateInputs ();

		// Check inputs
		//string commandInput = command.captionText.text;
		if (commandInput == "") {
			//missingInputs = true;
			invalidInputs.Add (command);
			invalidInputLabels.Add (commandLabelText);
		} else {
			// set new input
			//commandInput = commandInput.ToLower ();
			newInputs.Add (commandInput);

			if (command.image.color != Color.white) {
				commandImage.color = Color.white;
			}
		}

		// If the subject input isn't valid
		if (subjectInput == "") {
			invalidInputs.Add (subject);
			invalidInputLabels.Add (subjectLabelText);
		} else {
			// Set new input
			//subjectInput = subjectInput.ToLower ();
			newInputs.Add (subjectInput);

			if (subject.image.color != Color.white) {
				subjectImage.color = Color.white;
			}
		}

		if (adjective.image.color != Color.white) {
			adjective.image.color = Color.white;
		}

		if (toneInput == "") {
			invalidInputs.Add (tone);
			invalidInputLabels.Add (toneLabelText);
		} else {
			// Set new input
			//toneInput = toneInput.ToLower ();
			newInputs.Add (toneInput);
			if (tone.image.color != Color.white) {
				toneImage.color = Color.white;
			}
		}

		if (! isSentenceValid(sentenceInput)) {
			invalidInputs.Add (sentence);
			invalidInputLabels.Add ("Sentence");
		} else {
			newInputs.Add (sentenceInput);

			if (sentence.image.color != Color.white) {
				sentenceImage.color = Color.white;
			}
		}

		// If all the inputs are valid and the notificationPanel is active
		if (invalidInputs.Count == 0) {
			// Submit all the inputs
			chat.submitInputs(inputs[0], inputs[1], inputs[2], inputs[3], inputs[4]);
		} else {
			//displayFeedback();
		}
	}

	public void clearInput() {
		// Clear values
		command.value = 0;
		subject.value = 0;
		adjective.value = 0;
		tone.value = 0;
		sentence.text = "";

		// Set bools to false
		commandEntered = false;
		subjectEntered = false;
		adjectiveEntered = false;
		toneEntered = false;

		updateInputs ();
	}

	/*
	public void hideNotificationPanel() {
		notifications.SetActive (false);
	}

	void showNotificationPanel(int numOfNotifications) {
		if (numOfNotifications == 2) {
			notificationPanel2.SetActive (true);
		} else if (notificationPanel2.activeSelf) {
			notificationPanel2.SetActive(false);
		}
		notifications.SetActive (true);
	}
	*/

	/*
	public void invalidCombination() {
		invalidCombo = true;
		invalidInputs.Add (command);
		invalidInputLabels.Add (commandLabel);
		invalidInputs.Add (adjective);
		invalidInputLabels.Add (adjectiveLabel);
		invalidInputs.Add (tone);
		invalidInputLabels.Add (toneLabel);
		displayFeedback ();
	}
	*/

	public void showHint(string inputLabel) {
		switch (inputLabel) {
		case "command":
			//turnInputFieldRed(command);
			break;
		case "subject":
			//turnInputFieldRed(subject);
			break;
		case "adjective":
			//turnInputFieldRed(adjective);
			break;
		case "tone":
			//turnInputFieldRed(tone);
			break;
		}
	}

	/*
	void displayFeedback() {
		string feedbackString = "Please enter a";
		int numOfInvalidInputs = invalidInputs.Count;
		for (int i = 0; i < numOfInvalidInputs; i++) {
			if (typeof(Dropdown) == invalidInputs[i].GetType()) {
				Dropdown currentInput = (Dropdown) invalidInputs[i];
				//turnInputFieldRed(currentInput);
			}
			else if (typeof(InputField) == invalidInputs[i].GetType()) {
				InputField currentInput = (InputField) invalidInputs[i];
				//turnInputFieldRed(currentInput);
			}

			// Add the input label to the list
			feedbackString += " " + invalidInputLabels[i];

			// If this is the last one
			if (i == numOfInvalidInputs - 1) {
				// Add a period afterwards
				feedbackString += ".";
			}

			else {
				// If there are more than two invalid inputs
				if (numOfInvalidInputs > 2) {
					// Add a comma and a space afterwards
					feedbackString += ",";
				}
				// If this is the second to last invalid input
				if (i == numOfInvalidInputs - 2) {
					// Add the word "and" with a space after wards
					feedbackString += " and";
				}
			}
		}
		int numOfNotifications = 1;
		notificationText1.text = feedbackString;

		if (invalidCombo) {
			notificationText1.text = "That's not a valid combination.";
		} 
		if (! notifications.activeSelf) {
			showNotificationPanel(numOfNotifications);
		}
	}
	*/

	public void enterInput(string inputLabel) {
		switch (inputLabel) {
		case "command":
			if (command.value != 0) {
				commandEntered = true;
			}
			else {
				submitButton.interactable = false;
				commandEntered = false;
			}
			break;
		case "subject":
			if (subject.value != 0) {
				subjectEntered = true;
			}
			else {
				submitButton.interactable = false;
				subjectEntered = false;
			}
			break;
		case "adjective":
			break;
		case "tone":
			if (tone.value != 0) {
				toneEntered = true;
			}
			else {
				submitButton.interactable = false;
				toneEntered = false;
			}
			break;
		case "sentence":
			if (isSentenceValid(sentence.text)) {
				sentenceEntered = true;
			}
			else {
				sentenceEntered = false;
			}
			break;
		}
		updateInputs ();
		showSuggestion ();
		showInputChoices();
	}

	void showSuggestion() {
		if (commandEntered && subjectEntered && toneEntered) {
			if (suggestionsOn) {
				sentence.text = npc.getSuggestedSentence(inputs[0], inputs[1], inputs[2], inputs[3]);
				sentenceEntered = true;
			}
			if (sentenceEntered) {
				submitButton.interactable = true;
			}
		}
	}

	public void showInputChoices() {
		ArrayList choices = getInputChoices (0, twineParser.AllPaths);

		for (int i = 0; i < choices.Count; i++) {
			ArrayList inputChoices = (ArrayList) choices[i];
			List<Dropdown.OptionData> options = dropdowns[i].options;
			for (int j = 0; j < options.Count; j++) {
				string optionText = options[j].text;
				if (inputChoices.Contains (unboldedText(optionText).ToLower ())) {
					optionText = boldedText(optionText);
				}
				else {
					optionText = unboldedText(optionText);
				}
				options[j].text = optionText;
			}
		}
	}

	// Adds b tags to a string
	string boldedText(string text) {
		// If the text is not already bold
		if (! text.Contains ("<b>")) {
			text = "<b>" + text + "</b>";
		}
		return text;
	}

	// Removes b tags from a string
	string unboldedText(string text) {
		// If the text is bold
		if (text.Contains ("<b>")) {
			text = text.Substring(3, text.Length - 7);
		}
		return text;
	}

	/*
	 * Finds all possible choices with the entered input
	 * Recursive function
	 */
	ArrayList getInputChoices(int inputIndex, ArrayList availablePaths) {

		if (inputIndex == 4) {
			// After all the inputs have been used
			ArrayList commandChoices = new ArrayList();
			ArrayList subjectChoices = new ArrayList();
			ArrayList adjectiveChoices = new ArrayList();
			ArrayList toneChoices = new ArrayList();
			
			ArrayList choices = new ArrayList ();
			choices.Add (commandChoices);
			choices.Add (subjectChoices);
			choices.Add (adjectiveChoices);
			choices.Add (toneChoices);

			Debug.Log (availablePaths.Count);
			for (int p = 0; p < availablePaths.Count; p++) {
				ArrayList path = (ArrayList) availablePaths[p];
				for (int i = 0; i < choices.Count; i++) {
					ArrayList choicesArray = (ArrayList) choices[i];
					if (! choicesArray.Contains (path[i])) {
						choicesArray.Add (path[i]);
					}
				}
			}
			return choices; // Final
		}
		string currentInput = unboldedText(inputs [inputIndex]);
		// If input is blank
		if (currentInput == "") {
			// Don't modify available paths
			return getInputChoices(inputIndex + 1, availablePaths);
		}
		ArrayList newPathsList = new ArrayList ();
		for (int p = 0; p < availablePaths.Count; p++) {
			ArrayList path = (ArrayList) availablePaths[p];
			if (path[inputIndex].Equals(currentInput)) {
				newPathsList.Add (path);
			}
		}
		return getInputChoices (inputIndex + 1, newPathsList);
	}

	bool isSentenceValid(string sentenceInput) {
		sentenceInput = sentenceInput.Trim ();
		// If the sentence field was left empty
		if (sentenceInput == "") {
			submitButton.interactable = false;
			return false;
		}
		return true;
	}

	public void changeSuggestionSettings() {
		suggestionsOn = suggestionToggle.isOn;
		if (suggestionsOn) {
			showSuggestion();
		}
	}

	public void updateInputs() {
		commandInput = command.captionText.text;
		subjectInput = subject.captionText.text;
		adjectiveInput = adjective.captionText.text.ToLower();
		toneInput = tone.captionText.text;
		sentenceInput = sentence.text;

		inputs = new string[5] {
			unboldedText(commandInput).ToLower (),
			unboldedText(subjectInput).ToLower (),
			unboldedText(adjectiveInput).ToLower (),
			unboldedText(toneInput).ToLower (),
			sentenceInput
		};
	}
}
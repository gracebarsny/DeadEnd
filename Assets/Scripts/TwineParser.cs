/*
 * Attached to Non-Player Character
 * 
 * Reads in a twine file 
 * The file MUST follow the instructions specified in the Twine Format Guide (see Twines folder in Assets)
 * 
 * Finds the story
 * Separates the story into passages
 * Provides info about any passage given a passage title
 * 
 * Passage Info includes:
 * - Narration/dialogue
 * - New keywords
 * - New pathways
 * - Character's reaction
 * - Suggested sentences
 * 
 * Contains list of all possible input combinations
 * Contains list of all suggested sentences
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TwineParser : MonoBehaviour {
	
	public TextAsset twineFile;

	string story;
	List<string> titles;
	List<string> passages;

	ArrayList allPaths;
	ArrayList allSuggestedSentences;

	void Start() {
		story = findAndFixStory (twineFile.text);
		titles = findPassageTitles ();
		passages = findPassages ();
		allPaths = new ArrayList ();
		allSuggestedSentences = new ArrayList ();
	}

	/*
	 * Finds all substrings between two strings
	 */
	List<string> findBetween(string text, string startString, string endString) {
		List<string> matched = new List<string>();
		bool exit = false;
		int indexStart = 0;
		int indexEnd = 0;
		while (! exit) {
			indexStart = text.IndexOf(startString);
			indexEnd = text.IndexOf(endString);
			if (indexStart != -1 && indexEnd != -1) { 
				matched.Add(text.Substring(indexStart + startString.Length, indexEnd - indexStart - startString.Length));
				text = text.Substring(indexEnd + endString.Length);
			}
			else {
				exit = true;
			}
		}
		return matched;
	}

	/*
	 * Finds the passage with the title input
	 * Returns ArrayList with passage info
	 * passageInfo[0] = string[] responses (narration/dialogue)
	 * passageInfo[1] = string[] newSubjects
	 * passageInfo[2] = string[] newAdjectives
	 * passageInfo[3] = ArrayList paths
	 * passageInfo[4] = string emotion
	 * passageInfo[5] = string[] suggestedSentences
	 * passageInfo[6] = string[] newCommands
	 */
	public ArrayList getPassageInfo(string passageTitle) {
		ArrayList passageInfo = new ArrayList ();

		int passageIndex = titles.IndexOf (passageTitle);
		string currentPassage = passages [passageIndex];

		// Split into text and code sections
		// Text and Code are separated by "///"
		string[] textAndCode = currentPassage.Split (new string[] { "\n///\n" }, StringSplitOptions.None);

		// Text section contains narration and dialogue
		string textSection = textAndCode [0];

		// Responses can be either narration or dialogue
		// Each response is separated by '~' (see Twine Format Guide for details)
		string[] responses = textSection.Split (new string[] { "\n~\n" }, StringSplitOptions.None);
		//passageInfo.Add (responses);

		// Code section contains new keywords, new adjectives, paths, emotion
		string codeSection = textAndCode [1];

		// Each code snippet is separated by two line breaks (see Twine Format Guide for details)
		// Code snippets in order: new keywords, paths, emotion
		string[] codeSnippet = codeSection.Split (new string[] { "\n\n" }, StringSplitOptions.None);

		// Get new subjects (Keywords 0)
		string[] keywords = codeSnippet[0].Split ('\n');
		string[] newSubjects = getList (keywords[0]);

		// Get new adjectives (Keywords 1) and new commands (Keywords 2), if any
		string[] newAdjectives = null;
		string[] newCommands = null;
		if (keywords.Length > 1) {
			newAdjectives = getList (keywords [1]);
		} 
		if (keywords.Length > 2) {
			newCommands = getList (keywords[2]);
		}

		// Get branching paths
		string pathsParagraph = codeSnippet [1];
		ArrayList separatedPathsAndSuggestions = separatePathsAndSuggestions (pathsParagraph);
		ArrayList paths = (ArrayList) separatedPathsAndSuggestions [0];
		List<string> suggestedSentences = (List<string>) separatedPathsAndSuggestions [1];

		// Get emotion
		string emotion = codeSnippet [2].Split (';')[1];

		passageInfo.Add (responses);
		passageInfo.Add (newSubjects);
		passageInfo.Add (newAdjectives);
		passageInfo.Add (paths);
		passageInfo.Add (emotion);
		passageInfo.Add (suggestedSentences);
		passageInfo.Add (newCommands);

		return passageInfo;
	}

	// Gets list of new keywords between parentheses
	string[] getList(string line) {
		return findBetween (line, "(", ")") [0].Split (new string[] { ", " }, StringSplitOptions.None);
	}

	/*
	 * Finds paths and suggestions within pathsParagraph of passage
	 * Adds suggestions to allSuggestedSentences
	 * Returns ArrayList with paths and suggestions
	 * separatedPathsAndSuggestions[0] = ArrayList paths
	 * separatedPathsAndSuggestions[1] = List<string> suggestions
	 */
	ArrayList separatePathsAndSuggestions (string pathsParagraph) {
		List<string> pathLines = findBetween (pathsParagraph, "[[", "]]");
		ArrayList paths = null;
		if (pathLines[0] != "") {
			paths = formatPaths (pathLines);
		}

		List<string> suggestions = null;
		if (paths != null) {
			suggestions = new List<string>();
			string[] pathsWithSuggestions = pathsParagraph.Split ('\n');
			foreach (string line in pathsWithSuggestions) {
				int position = line.LastIndexOf (']');
				// Gets all text after the ] on each line
				string suggestedSentence = line.Substring (position + 1);
				suggestions.Add (suggestedSentence);
				allSuggestedSentences.Add (suggestedSentence);
			}
		}
		ArrayList separatedPathsAndSuggestions = new ArrayList ();
		separatedPathsAndSuggestions.Add (paths);
		separatedPathsAndSuggestions.Add (suggestions);

		return separatedPathsAndSuggestions;
	}
	
	/*
	 * Returns an ArrayList of ArrayList of strings
	 * Each ArrayList has the following format:
	 * [command, subject, adjective, tone, nextPassageTitle]
	 */
	ArrayList formatPaths(List<string> pathLines) {
		ArrayList formattedPaths = new ArrayList ();

		for (int i = 0; i < pathLines.Count; i++) {
			string currentPath = pathLines[i];

			string[] inputs = currentPath.Split('-');

			string[] tonesAndTitle = inputs[3].Split('|');

			string[] tones = tonesAndTitle[0].Split ('/');

			// For each tone
			for (int t = 0; t < tones.Length; t++) {
				ArrayList pathInfo = new ArrayList();
				pathInfo.Add (inputs[0]); // command
				pathInfo.Add (inputs[1]); // subject
				pathInfo.Add (inputs[2]); // adjective
				pathInfo.Add (tones[t]); // tone
				pathInfo.Add (tonesAndTitle[1]); // title

				formattedPaths.Add (pathInfo);
				allPaths.Add (pathInfo);
			}
				//Debug.Log ("currentPath: " + pathInfo[0] + " " + pathInfo[1] + " " + pathInfo[2] + " " + pathInfo[3] + " " + pathInfo[4]);

		}
		return formattedPaths;
	}

	/*
	 * Adds any new suggested sentences to allSuggestedSentences
	 */
	void addSuggestedSentences(List<string> suggestedSentences) {
		if (suggestedSentences != null) {
			for (int i = 0; i < suggestedSentences.Count; i++) {
				allSuggestedSentences.Add (suggestedSentences [i]);
			}
		}
	}

	/*
	 * Finds story within the tw-storydata tags in the HTML file
	 * Replaces &quot; with "
	 * Adds bold tags
	 * Replaces &#x27; and &#39; with '
	 */
	string findAndFixStory(string fullText) {
		string startString = "\n</script>";
		string endString = "</tw-storyd";
		string story = findBetween (fullText, startString, endString)[0];
		
		// Replace quotes
		story = story.Replace ("&quot;", "\"");
		
		// Replace < and >
		story = story.Replace ("&lt;", "<").Replace ("&gt;", ">");
		
		// Replace apostrophes
		story = story.Replace ("&#x27;", "\'");
		story = story.Replace ("&#39;", "\'");
		
		return story;
	}

	/*
	 * Finds passage titles within the story
	 * Titles are specified as an attribute to the name property within a tw-passagedata tag
	 * These titles are used to find the next passage
	 */
	List<string> findPassageTitles() {
		string startString = "e=\"";
		string endString = "\" ta";
		return findBetween (story, startString, endString);
	}

	/*
	 * Finds passages within the story
	 * Titles are specified as an attribute to the name property within a tw-passagedata tag
	 * These titles are used to find the next passage
	 */
	List<string> findPassages() {
		string startString = "\">";
		string endString = "</t";
		return findBetween (story, startString, endString);
	}

	public ArrayList AllPaths {
		get { return allPaths; }
	}

	public ArrayList AllSuggestedSentences {
		get { return allSuggestedSentences; }	
	}

	public List<string> Titles {
		get { return titles; }
	}
}
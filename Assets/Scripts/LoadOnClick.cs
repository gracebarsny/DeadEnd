/*
 * Attached to buttons on StartScreen, Credits, and the Bedroom pop-up menu
 * 
 * Code directly from Unity Tutorial (beside menu click): 
 * Creating a Scene Selection Menu
 * https://unity3d.com/learn/tutorials/modules/beginner/live-training-archive/creating-a-scene-menu
 * 
 * Unity Documentation on LoadLevel:
 * http://docs.unity3d.com/ScriptReference/Application.LoadLevel.html
 */

using UnityEngine;
using System.Collections;

public class LoadOnClick : MonoBehaviour
{
	/*
    AudioSource menuClick;

    void Awake()
    {
        menuClick = GetComponent<AudioSource>();
    }
    */

    public void LoadScene(int level)
    {
        //menuClick.Play();
        Application.LoadLevel(level);
    }
}
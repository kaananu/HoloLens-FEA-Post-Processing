using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadModelButton : MonoBehaviour {

    public GameObject WelcomeText;
    public GameObject ModelCreationCube;
    public GameObject Managers;
    public GameObject LoadButton;

	// Use this for initialization
	void OnSelect()
    {

        Managers.SendMessage("LoadModel");

        Destroy(WelcomeText);
        ModelCreationCube.SetActive(false);
        LoadButton.SetActive(false);
    }
}

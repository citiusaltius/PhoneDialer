﻿using UnityEngine;
using System.Collections;

public class ButtonAboutBehaviourScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnClick()
    {
        Assets.Scripts.Scenes.SceneSwitcher.SwitchScene(Assets.Scripts.Scenes.SceneSwitcher.SceneNames.SceneAbout);
    }
}
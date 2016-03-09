using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class BackScreenBehaviourScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnClick()
    {
        Assets.Scripts.Scenes.SceneSwitcher.BackScene();
    }
}

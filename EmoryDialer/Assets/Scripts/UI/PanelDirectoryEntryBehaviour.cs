using UnityEngine;
using UnityEngine.UI;

public class PanelDirectoryEntryBehaviour : MonoBehaviour {

    public Button ButtonDirectoryEntry;
    public Text Label;
    public Text Sublabel;

	// Use this for initialization
	void Start () {
	
	}

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClick()
    {
        UnityEngine.Debug.Log("button clicked");
    }
}

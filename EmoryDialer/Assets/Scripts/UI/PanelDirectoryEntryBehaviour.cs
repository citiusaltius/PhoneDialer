using UnityEngine;
using UnityEngine.UI;

public class PanelDirectoryEntryBehaviour : MonoBehaviour {

    public Button ButtonDirectoryEntry;
    public Text Label;
    public Text Sublabel;
    public Text Extension;
    public Text FullNumber;
    public Assets.Scripts.Data.Model.DirectoryEntry ApplicableEntry;

	// Use this for initialization
	void Start () {
	    
	}

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClick()
    {
        Debug.Log("button clicked");
        Assets.Scripts.Data.DataStore.EntryToCall = ApplicableEntry;
        Assets.Scripts.Scenes.SceneSwitcher.SwitchScene(Assets.Scripts.Scenes.SceneSwitcher.SceneNames.ScenePhoneDialer);
    }

    public void AddContent(GameObject EncapsulatingObject, Assets.Scripts.Data.Model.DirectoryEntry NewInformation, RectTransform Parent)
    {
        EncapsulatingObject.transform.SetParent(Parent);
        EncapsulatingObject.transform.SetAsLastSibling();
        EncapsulatingObject.transform.localScale = new Vector3(1, 1, 1);
        Label.text = NewInformation.Label;
        Sublabel.text = NewInformation.Sublabel;
        Extension.text = NewInformation.Extension;
        FullNumber.text = NewInformation.FullNumber;
        ApplicableEntry = NewInformation;
    }
}

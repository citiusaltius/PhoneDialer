using UnityEngine;
using UnityEngine.UI;

public class ScrollViewDirectoryBehaviour : MonoBehaviour {

    public ScrollRect Scroller;
    public RectTransform Content;
    public GameObject EntryTemplateButton;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnDirectoryLocationDropdownValueChanged()
    {
        UnityEngine.Debug.Log("location changed.");
        for(int i = 0; i < 5; i++)
        {
            LoadDirectoryEntries();
        }
    }

    public void LoadDirectoryEntries()
    {
        GameObject NewEntry = Instantiate(EntryTemplateButton) as GameObject;

        PanelDirectoryEntryBehaviour InstantiatedEntry = NewEntry.GetComponent<PanelDirectoryEntryBehaviour>();
        NewEntry.transform.SetParent(Content);
        NewEntry.transform.SetAsLastSibling();
        NewEntry.transform.localScale = new Vector3(1, 1, 1);
        InstantiatedEntry.Label.text = "new entry";
        InstantiatedEntry.Sublabel.text = "new sublabel";

    }
}

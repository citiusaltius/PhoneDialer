using UnityEngine;
using UnityEngine.UI;

public class ScrollViewDirectoryBehaviour : MonoBehaviour {

    public ScrollRect Scroller;
    public RectTransform Content;
    public GameObject EntryTemplateButton;

	// Use this for initialization
	void Start () {
        LoadDataFromDefaultData();
	}
	
    void LoadDataFromDefaultData()
    {
        string GradyData = Assets.Scripts.Data.DirectoryData.DefaultData.GradyData;
        System.IO.StringReader Reader = new System.IO.StringReader(GradyData);
        string ReadLine = null;
        char[] Delimiters = new char[] { '\t' };
        do
        {

            ReadLine = Reader.ReadLine();
            if(ReadLine == null) { break; }
            string[] SplitLine = ReadLine.Split(Delimiters);
            Assets.Scripts.Data.Model.DirectoryEntry AnotherEntry = new Assets.Scripts.Data.Model.DirectoryEntry();
            AnotherEntry.Label = SplitLine[0];
            AnotherEntry.Category = SplitLine[1];
            AnotherEntry.Sublabel = SplitLine[2];
            AnotherEntry.Extension = SplitLine[3];
            AnotherEntry.FullNumber = SplitLine[4];
            LoadDirectoryEntry(AnotherEntry);
        } while (ReadLine != null);
    }

	// Update is called once per frame
	void Update () {
	
	}

    public void OnDirectoryLocationDropdownValueChanged()
    {
        UnityEngine.Debug.Log("location changed.");
        DestroyChildren(Content);
        for(int i = 0; i < 5; i++)
        {
            Assets.Scripts.Data.Model.DirectoryEntry NewInformation = new Assets.Scripts.Data.Model.DirectoryEntry();
            NewInformation.Label = "new information";
            NewInformation.Sublabel = "- " + System.DateTime.Now;
            NewInformation.Extension = "51234";
            NewInformation.FullNumber = "14046161000";
            LoadDirectoryEntry(NewInformation);
        }
    }

    public void LoadDirectoryEntry(Assets.Scripts.Data.Model.DirectoryEntry NewInformation)
    {
        GameObject NewEntry = Instantiate(EntryTemplateButton) as GameObject;
        PanelDirectoryEntryBehaviour InstantiatedEntry = NewEntry.GetComponent<PanelDirectoryEntryBehaviour>();
        InstantiatedEntry.AddContent(NewEntry, NewInformation, Content);
    }

    public void DestroyChildren(Transform transform)
    {
        for (int i = transform.childCount - 1; i >= 0; --i)
        {
            GameObject.Destroy(transform.GetChild(i).gameObject);
        }
        transform.DetachChildren();
    }
}

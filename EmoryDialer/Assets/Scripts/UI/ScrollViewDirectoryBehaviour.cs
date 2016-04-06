using Assets.Scripts.UI;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ScrollViewDirectoryBehaviour : MonoBehaviour {

    public Dropdown LocationDropdown;
    public ScrollRect Scroller;
    public RectTransform Content;
    public GameObject EntryTemplateButton;

	// Use this for initialization
	void Start () {
        // LoadDataFromDefaultData();
	}
	
    void LoadDataFromDefaultData(string StringData)
    {

        List<string> Lines = new List<string>(StringData.Split(new char[] { '\n' }));
        
        char[] Delimiters = new char[] { '\t' };
        foreach(string ReadLine in Lines)
        {
            if(ReadLine == null) { continue; }
            string[] SplitLine = ReadLine.Split(Delimiters, System.StringSplitOptions.None);
            if (SplitLine.Length < 5)
            {
                Debug.Log(SplitLine.Length + ":\t" + ReadLine);
                continue;
            }

            Assets.Scripts.Data.Model.DirectoryEntry AnotherEntry = new Assets.Scripts.Data.Model.DirectoryEntry();
            AnotherEntry.Label = SplitLine[0];
            AnotherEntry.Category = SplitLine[1];
            AnotherEntry.Sublabel = SplitLine[2];
            AnotherEntry.Extension = SplitLine[3];
            AnotherEntry.FullNumber = SplitLine[4];
            LoadDirectoryEntry(AnotherEntry);
        } 
        Debug.Log(Content.childCount + " entries from " + Lines.Count + " lines");
        Debug.Log(StringData );
    }

	// Update is called once per frame
	void Update () {
	
	}

    public void OnDirectoryLocationDropdownValueChanged()
    {
        UnityEngine.Debug.Log("location changed.");
        DestroyChildren(Content);

        DirectoryLocationDropdownOptionData CurrentOption = (DirectoryLocationDropdownOptionData ) LocationDropdown.options[LocationDropdown.value];
        UnityEngine.Debug.Log("From scrollview - " + CurrentOption.LocationUniqueIdentifier);

        switch(CurrentOption.LocationUniqueIdentifier)
        {
            case "Grady.identifier":
                LoadDataFromDefaultData(Assets.Scripts.Data.DirectoryData.DefaultData.GradyData);
                break;

            default:
                for (int i = 0; i < 5; i++)
                {
                    Assets.Scripts.Data.Model.DirectoryEntry NewInformation = new Assets.Scripts.Data.Model.DirectoryEntry();
                    NewInformation.Label = "new information";
                    NewInformation.Sublabel = "- " + System.DateTime.Now;
                    NewInformation.Extension = "51234";
                    NewInformation.FullNumber = "14046161000";
                    LoadDirectoryEntry(NewInformation);
                }
                break;
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

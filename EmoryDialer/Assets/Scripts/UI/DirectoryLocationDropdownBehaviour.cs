using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DirectoryLocationDropdownBehaviour : MonoBehaviour {

    public Dropdown DirectoryLocationDropdown;
    
	// Use this for initialization
	void Start () {
        PopulateOptions();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void PopulateOptions()
    {
        DirectoryLocationDropdown.ClearOptions();
        for(int i = 0; i < 10; i++)
        {
            Assets.Scripts.UI.DirectoryLocationDropdownOptionData Temp = new Assets.Scripts.UI.DirectoryLocationDropdownOptionData();
            Temp.text = "test option " + i + "\n" + "subtext";
            Temp.LocationUniqueIdentifier = "value " + i;
            DirectoryLocationDropdown.options.Add(Temp);
        }
        
    }

    public void OnValueChanged()
    {
        Debug.Log("changed location to " +     ( (Assets.Scripts.UI.DirectoryLocationDropdownOptionData) 
            (DirectoryLocationDropdown.options)[DirectoryLocationDropdown.value]).LocationUniqueIdentifier
            );
    }
}

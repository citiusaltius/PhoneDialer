using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

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
        List<string> Locations = new List<string>() { "Grady", "Emory", "VA" };
        Dropdown.OptionData BaseOption = new Dropdown.OptionData("---please select---");
        DirectoryLocationDropdown.options.Add(BaseOption);
        for (int i = 0; i < Locations.Count; i++)
        {
            Assets.Scripts.UI.DirectoryLocationDropdownOptionData Temp = new Assets.Scripts.UI.DirectoryLocationDropdownOptionData();
            Temp.text = Locations[i];
            Temp.LocationUniqueIdentifier = Locations[i] + ".identifier";
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

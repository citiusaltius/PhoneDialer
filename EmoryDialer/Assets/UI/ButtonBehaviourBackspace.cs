using UnityEngine;
using UnityEngine.UI;

public class ButtonBehaviourBackspace : MonoBehaviour {
    public InputField PhoneNumber;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnClick()
    {
        if(PhoneNumber.text.Length > 0)
        {
            PhoneNumber.text = PhoneNumber.text.Substring(0, PhoneNumber.text.Length - 1);
        }
    }
}

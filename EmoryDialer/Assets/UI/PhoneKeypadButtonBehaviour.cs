using UnityEngine;
using UnityEngine.UI;

public class PhoneKeypadButtonBehaviour : MonoBehaviour {

    public Text ButtonText;
    public InputField PhoneNumber;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnClick()
    {
        if(PhoneNumber.text.Length < 5)
        {
            PhoneNumber.text += ButtonText.text;
        }
        else
        {
                
        }
    }
}

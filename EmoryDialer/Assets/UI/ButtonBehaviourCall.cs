using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class ButtonBehaviourCall : MonoBehaviour {

    public InputField PhoneNumber;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnClick()
    {
        StringBuilder PhoneNumberBuilder = new StringBuilder();
        string PhoneNumberText = PhoneNumber.text;
        switch(PhoneNumberText.Length)
        {
            case 4:
                PhoneNumberBuilder.Append("1404646");
                PhoneNumberBuilder.Append(PhoneNumberText);
                break;

            case 5:
                switch(PhoneNumberText[0])
                {
                    case '5':
                        PhoneNumberBuilder.Append("1404616");
                        break;

                    case '6':

                        break;

                    case '7':

                        break;

                    case '8':
                        PhoneNumberBuilder.Append("1404778");
                        break;

                    default:

                        break;
                }
                PhoneNumberBuilder.Append(PhoneNumberText.Substring(1));
                break;


        }


        Application.OpenURL("tel://" + PhoneNumberBuilder.ToString());
    }

}

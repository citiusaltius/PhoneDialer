using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class ButtonBehaviourCall : MonoBehaviour {

    public InputField PhoneNumber;
    public Text TextDetails;
	// Use this for initialization
	void Start () {
	    if(Assets.Scripts.Data.DataStore.EntryToCall != null)
        {
            if (string.IsNullOrEmpty(Assets.Scripts.Data.DataStore.EntryToCall.Extension) == false)
            {
                PhoneNumber.text = Assets.Scripts.Data.DataStore.EntryToCall.Extension;
            }
            else if (string.IsNullOrEmpty(Assets.Scripts.Data.DataStore.EntryToCall.FullNumber) == false)
            {
                PhoneNumber.text = Assets.Scripts.Data.DataStore.EntryToCall.FullNumber;
            }
            
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnClick()
    {
        Handheld.Vibrate();
        StringBuilder PhoneNumberBuilder = new StringBuilder();
        string PhoneNumberText = PhoneNumber.text;
        bool IsValidCall = false;
        switch(PhoneNumberText.Length)
        {
            case 4:
                PhoneNumberBuilder.Append("1404646");
                PhoneNumberBuilder.Append(PhoneNumberText);
                IsValidCall = true;
                break;

            case 5:
                switch(PhoneNumberText[0])
                {
                    case '1':
                        PhoneNumberBuilder.Append("1404251");
                        IsValidCall = true;
                        break;

                    case '2':
                        PhoneNumberBuilder.Append("1404712");
                        IsValidCall = true;
                        break;

                    case '4':
                        PhoneNumberBuilder.Append("1404489");
                        IsValidCall = true;
                        break;

                    case '5':
                        PhoneNumberBuilder.Append("1404616");
                        IsValidCall = true;
                        break;

                    case '6':
                        PhoneNumberBuilder.Append("1404686");
                        IsValidCall = true;
                        break;

                    case '7':
                        PhoneNumberBuilder.Append("1404727");
                        IsValidCall = true;
                        break;

                    case '8':
                        PhoneNumberBuilder.Append("1404778");
                        IsValidCall = true;
                        break;

                    default:
                        TextDetails.text = "invalid prefix " + PhoneNumberText[0];
                        break;
                }

                if(IsValidCall == true)
                {
                    PhoneNumberBuilder.Append(PhoneNumberText.Substring(1));
                }

                break;

            default:
                TextDetails.text = "invalid call\n must have 4-5 digits";
                IsValidCall = false;
                break;

        }

        if (IsValidCall == true)
        {
            Application.OpenURL("tel://" + PhoneNumberBuilder.ToString());
        }

    }

}

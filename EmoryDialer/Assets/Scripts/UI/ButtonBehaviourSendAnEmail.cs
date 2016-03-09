using UnityEngine;
using System.Collections;

public class ButtonBehaviourSendAnEmail : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnClick()
    {
        string email = "me@aiwong.com";
        string subject = MyEscapeURL("Re: Grady / Emory / Atlanta VA dialer");
        string body = MyEscapeURL(" ");
        Application.OpenURL("mailto:" + email + "?subject=" + subject + "&body=" + body);
    }

    string MyEscapeURL(string url)
    {
        return WWW.EscapeURL(url).Replace("+", "%20");
    }
}

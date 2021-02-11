using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AddPopup : MonoBehaviour
{
    [SerializeField] Toggle toggle;
    [SerializeField] TMP_InputField text;
    [SerializeField] GameObject warning;

    public void SaveNewChallenge()
    {
        if (text.text == "")
        {
            NoTextWarning();
            return;
        }
        var chal = new Challenge(text.text, toggle.isOn, false, true);
        FindObjectOfType<ChallengeList>().AddNewChallenge(chal);
        ResetData();
        GetComponent<Popup>().ClosePopup();
    }

    void NoTextWarning()
    {
        warning.SetActive(true);
        warning.transform.localScale = Vector3.zero;
        LeanTween.scale(warning, Vector3.one, 0.2f).setEaseInExpo();
    }

    void ResetData()
    {
        toggle.isOn = true;
        text.text = "";
        warning.SetActive(false);
    }
}

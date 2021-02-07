using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EditPopup : MonoBehaviour
{
    [SerializeField] Toggle toggle;
    [SerializeField] TMP_InputField text;
    [SerializeField] GameObject warning;

    ListItem item;
    Challenge chal;

    public void SaveEdit()
    {
        if (text.text == "")
        {
            NoTextWarning();
            return;
        }
        var challenge = new Challenge(text.text, toggle.isOn, chal.isStared, true);
        item.ChangeAfterEdit(challenge);
        ResetData();
        GetComponent<Popup>().ClosePopup();
    }

    void NoTextWarning()
    {
        warning.SetActive(true);
        warning.transform.localScale = Vector3.zero;
        LeanTween.scale(warning, Vector3.one, 0.2f).setEaseInExpo();
    }

    public void ResetData()
    {
        toggle.isOn = true;
        text.text = "";
        warning.SetActive(false);
    }

    public void InitializeEdit(Challenge challenge, ListItem listItem)
    {
        text.text = challenge.name;
        toggle.isOn = challenge.coronaFriendly;
        GetComponent<Popup>().OpenPopup();
        chal = challenge;
        item = listItem;
    }
}

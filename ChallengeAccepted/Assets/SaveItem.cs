using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;


public class SaveItem : MonoBehaviour, IPointerDownHandler
{
    TextMeshProUGUI text;
    SaveSystem ss;

    public SaveItemData data;

    public void Initiate(SaveItemData myData)
    {
        text = GetComponent<TextMeshProUGUI>();
        ss = FindObjectOfType<SaveSystem>();
        data = myData;
        text.text = data.nameToShow;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("was selected");
        text.color = Color.white;
        ss.SelectThisKey(data.key);
    }

    public void Deselect()
    {
        text.color = Color.black;
    }


}


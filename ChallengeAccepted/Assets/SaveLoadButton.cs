using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;



public class SaveLoadButton : MonoBehaviour, IPointerDownHandler
{
    TextMeshProUGUI tmp;


    public void ChangeButton(string text, bool anim = true)
    {
        var tmp = GetComponent<TextMeshProUGUI>();
        if (tmp.text != text && anim)
        {
            transform.localScale = Vector3.zero;
            LeanTween.scale(gameObject, Vector3.one, 0.3f).setEaseOutElastic();
        }
        tmp.text = text;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        FindObjectOfType<SaveSystem>().LoadOrSave();
    }

    public void Hide()
    {
        LeanTween.scale(gameObject, Vector3.zero, 0.1f);
    }
    public void Show()
    {
        LeanTween.scale(gameObject, Vector3.one, 0.1f).setEaseOutExpo();
    }





}

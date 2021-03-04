using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsSave : MonoBehaviour
{
    SaveSystem ss;

    private void Start()
    {
        ss = FindObjectOfType<SaveSystem>();
        transform.localScale = Vector3.zero;
    }

    public void CallDelete()
    {
        ss.ReceiveAnswer(true);
    }

    public void CallCancel()
    {
        ss.ReceiveAnswer(false);
    }

    public void Show()
    {
        LeanTween.scale(gameObject, Vector3.one, 0.3f).setEaseInExpo();
    }

    public void Hide()
    {
        LeanTween.scale(gameObject, Vector3.zero, 0.1f).setEaseOutExpo();
    }
}

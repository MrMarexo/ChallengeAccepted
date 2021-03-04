using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MessageScript : MonoBehaviour
{
    TextMeshProUGUI tmp;
    IEnumerator coroutine;

    void Start()
    {
        transform.localScale = Vector3.zero;
        tmp = GetComponent<TextMeshProUGUI>();
        coroutine = WaitAndDisappear();
    }

    public void Appear(string text, bool timedDisappear = true)
    {
        if (transform.localScale == Vector3.zero)
        {
            StopCoroutine(coroutine);
            transform.localScale = Vector3.zero;
            LeanTween.scale(gameObject, Vector3.one, 0.3f).setEaseOutExpo();
            if (timedDisappear)
            {
                StartCoroutine(coroutine);
            }
        }
        tmp.text = text;
    }

    public void StopWaitAndDisappear()
    {
        StopCoroutine(coroutine);
        Disappear();
    }

    public void Disappear()
    {
        LeanTween.scale(gameObject, Vector3.zero, 0.1f).setEaseInExpo();
        coroutine = WaitAndDisappear();
    }

    IEnumerator WaitAndDisappear()
    {
        yield return new WaitForSecondsRealtime(3);
        Disappear();
    }


}

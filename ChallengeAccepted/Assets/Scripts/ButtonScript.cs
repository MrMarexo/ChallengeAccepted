using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{
    //Color32 red = new Color32(239, 35, 60, 255);
    //Color32 white = new Color32(237, 242, 244, 255);

    //TextMeshProUGUI tm;
    //Color32 otherColor;
    //Color32 curColor;

    float tweenTime = 0.4f;


    //void Start()
    //{
    //    tm = GetComponent<TextMeshProUGUI>();
    //    curColor = tm.color;
    //    if (curColor.r == red.r && curColor.g == red.g && curColor.b == red.b)
    //    {
    //        otherColor = white;
    //    }
    //    else
    //    {
    //        otherColor = red;
    //    }
    //}
        
    public void Click()
    {
        LeanTween.scale(gameObject, new Vector3(0.9f, 0.9f, 0.9f), tweenTime).setEasePunch();
        //LeanTween.value(gameObject, 0.1f, 1f, tweenTime).setEasePunch().setOnUpdate((value) =>
        //{
        //    tm.color = Color.Lerp(curColor, otherColor, value);
        //}).setOnComplete(() =>
        //{
        //    tm.color = curColor;
        //});
    }
}

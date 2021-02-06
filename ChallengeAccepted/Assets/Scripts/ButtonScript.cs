using UnityEngine;
using UnityEngine.EventSystems;


public class ButtonScript : MonoBehaviour, IPointerDownHandler
{


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

    public void OnPointerDown(PointerEventData eventData)
    {
        Click();

    }

    void Click()
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


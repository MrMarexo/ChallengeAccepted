using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class PopupManager : MonoBehaviour
{
    GameObject[] popups;

    [SerializeField] GameObject listPopup;
    [SerializeField] GameObject mask;


     float openSpeed = 0.4f;
     float closeSpeed = 0.2f;


    //GameObject mask;

    void Start()
    {
        mask.SetActive(false);
        popups = GameObject.FindGameObjectsWithTag("popup");
        foreach (GameObject popup in popups)
        {
            popup.SetActive(false);
        }
    }

    public void ClosePopup()
    {
        var canvas = EventSystem.current.currentSelectedGameObject.transform.GetComponentInParent<Canvas>().gameObject;
        var panel = canvas.GetComponentInChildren<Image>().gameObject;
        LeanTween.scale(panel, new Vector3(0, 0, 0), closeSpeed).setEaseInSine().setOnComplete(() => Finish(canvas));
        ToggleMask(false);
    }

    void Finish(GameObject canvas)
    {
        canvas.SetActive(false);
    }


    public void OpenListPopup()
    {
        var panel = listPopup.GetComponentInChildren<Image>().gameObject;
        panel.transform.localScale = new Vector2(0, 0);
        listPopup.SetActive(true);
        ToggleMask(true);
        LeanTween.scale(panel, new Vector3(1, 1, 1), openSpeed).setEaseOutBounce();
    }

    void ToggleMask(bool tf)
    {
        var image = mask.GetComponent<Image>();
        var originalAlpha = image.color.a;
        Debug.Log(originalAlpha);
        var tempColor = image.color;
        if (tf)
        {
           tempColor.a = 0f;
            image.color = tempColor;
            mask.SetActive(true);
            LeanTween.alpha(image.rectTransform, originalAlpha, openSpeed).setEaseOutBounce();
        } else
        {
            LeanTween.alpha(image.rectTransform, 0f, closeSpeed).setOnComplete(() => ToggleCleanup(originalAlpha, tempColor, image));
        }
    }

    void ToggleCleanup(float origAlpha, Color tempColor, Image image)
    {
        mask.SetActive(false);
        tempColor.a = origAlpha;
        image.color = tempColor;
    }

    

}

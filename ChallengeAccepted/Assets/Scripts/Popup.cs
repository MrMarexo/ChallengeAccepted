using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    [SerializeField] GameObject mask;

    float openSpeed = 0.6f;
    float closeSpeed = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        //mask.SetActive(false);
        //if (gameObject.activeInHierarchy)
        //{
        //    gameObject.SetActive(false);

        //}
    }

    public void ClosePopup()
    {
        var panel = gameObject.GetComponentInChildren<Image>().gameObject;
        LeanTween.scale(panel, new Vector3(0, 0, 0), closeSpeed).setEaseInExpo()
            .setOnComplete(() =>
            {
                gameObject.SetActive(false);
            });
        ToggleMask(false);
    }

    public void OpenPopup()
    {
        var panel = gameObject.GetComponentInChildren<Image>().gameObject;
        panel.transform.localScale = new Vector2(0, 0);
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);

        }
        ToggleMask(true);
        LeanTween.scale(panel, new Vector3(1, 1, 1), openSpeed).setEaseOutExpo();
    }

    void ToggleMask(bool tf)
    {
        var image = mask.GetComponent<Image>();
        var originalAlpha = image.color.a;
        var tempColor = image.color;
        if (tf)
        {
            tempColor.a = 0f;
            image.color = tempColor;
            mask.SetActive(true);
            LeanTween.alpha(image.rectTransform, originalAlpha, openSpeed).setEaseOutExpo();
        }
        else
        {
            LeanTween.alpha(image.rectTransform, 0f, closeSpeed).setOnComplete(() =>
            {
                mask.SetActive(false);
                tempColor.a = originalAlpha;
                image.color = tempColor;
            });
        }
    }
}

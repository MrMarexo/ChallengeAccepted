using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tween : MonoBehaviour
{
    public void OnClickButton()
    {
        Debug.Log(gameObject.name);
        LeanTween.scale(gameObject, new Vector3(0, 0, 0), 0.5f);
    }
}

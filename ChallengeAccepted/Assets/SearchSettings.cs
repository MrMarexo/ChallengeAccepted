using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SearchSettings : MonoBehaviour
{
    [SerializeField] Toggle socialToggle;
    [SerializeField] Toggle repeatToggle;
    [SerializeField] Toggle playerRepeatToggle;
    [SerializeField] Toggle repeatInTurnToggle;

    public bool SocialToggle
    {
        get => socialToggle.isOn;
        set => socialToggle.isOn = value;
    }
    public bool RepeatToggle
    {
        get => repeatToggle.isOn;
        set => repeatToggle.isOn = value;
    }
    public bool PlayerRepeatToggle
    {
        get => playerRepeatToggle.isOn;
        set => playerRepeatToggle.isOn = value;
    }
    public bool RepeatInTurnToggle
    {
        get => repeatInTurnToggle.isOn;
        set => repeatInTurnToggle.isOn = value;
    }
}

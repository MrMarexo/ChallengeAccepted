using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class StaticScripts
{
    public static void SetAlphaTo(float alpha, Image image)
    {
        var newColor = image.color;
        newColor.a = alpha;
        image.color = newColor;
    }
}

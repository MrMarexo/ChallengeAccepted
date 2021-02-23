using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public struct MyQuote
{
    public string text;
    public string author;

    public MyQuote(string text, string author)
    {
        this.text = text;
        this.author = author;
    }

}

public class Quote : MonoBehaviour
{
    [SerializeField] GameObject quoteGo;

    MyQuote randomQuote;

    public bool quoteActive;

    List<MyQuote> quoteList = new List<MyQuote>()
    {
        new MyQuote("If you're going to try, go all the way. There is no other feeling like that. You will be alone with the gods, and the nights will flame with fire. You will ride life straight to perfect laughter. It's the only good fight there is.", "Charles Bukowski"),
        new MyQuote("Whether you think you can or you can't, either way, you are right.", "Henry Ford")
    };




    private void Awake()
    {
        randomQuote = quoteList[Random.Range(0, quoteList.Count)];
    }

    void Start()
    {
        var tms = quoteGo.GetComponentsInChildren<TextMeshProUGUI>();
        tms[0].text = "\"" + randomQuote.text + "\"";
        tms[1].text = randomQuote.author;
        quoteGo.transform.localScale = Vector3.zero;
        LeanTween.scale(quoteGo, Vector3.one, 0.4f).setEaseOutBounce();
        quoteActive = true;
    }

    public void Leave()
    {
        LeanTween.moveX(quoteGo, quoteGo.transform.position.x + Screen.width, 0.3f).setEaseInExpo();
        quoteActive = false;
    }

    public void Return()
    {
        LeanTween.moveX(quoteGo, quoteGo.transform.position.x - Screen.width, 0.3f).setEaseInExpo();
        quoteActive = true;
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChallengeList : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] Transform contextTransform;

    List<string> textList = new List<string>();



    void Start()
    {
        for (int i = 1; i <= 100; ++i)
        {
            textList.Add(i.ToString());
        }

        foreach (string item in textList)
        {
            var newText = Instantiate(prefab, contextTransform);
            newText.GetComponent<TextMeshProUGUI>().text = item;
        }
    }

    public void Generate()
    {
        int playerCount = GetComponent<PlayerList>().GetPlayerCount();
        var results = GameObject.FindGameObjectsWithTag("result");

        for (int i = 0; i < playerCount; ++i)
        {
            var resIndex = Random.Range(0, textList.Count);
            results[i].GetComponent<TextMeshProUGUI>().text = textList[resIndex];
        }
    }

   


}

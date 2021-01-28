using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChallengeList : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] Transform contextTransform; 
    
    List<string> textList = new List<string>{ "1", "2", "3", "4", "5", "6", "7", "8" };


    void Start()
    {
        foreach(string item in textList)
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

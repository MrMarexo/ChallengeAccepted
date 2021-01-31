using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerList : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] GameObject prefab2;

    [SerializeField] Transform listTransform;

    [SerializeField] GameObject plus;
    [SerializeField] GameObject minus;


    int playerCount = 1;

    private void Start()
    {
        CheckSigns();
    }


    public void AddPlayer()
    {
        int listChildrenCount = listTransform.childCount;
        var curPrefab = prefab;
        if (listChildrenCount % 2 == 0) curPrefab = prefab2;
        var newPlayer = Instantiate(curPrefab, listTransform);
        var nameChild = newPlayer.transform.GetChild(0);
        nameChild.localScale = Vector3.zero;
        var playerChildrenArr = newPlayer.transform.GetComponentsInChildren<TextMeshProUGUI>();
        foreach (TextMeshProUGUI child in playerChildrenArr)
        {
            if (child.gameObject.name == "Placeholder")
            {
                child.text = "Player " + (playerCount + 1).ToString();
            }
        }
        newPlayer.transform.SetSiblingIndex(listChildrenCount - 1);
        LeanTween.scale(nameChild.gameObject, Vector3.one, 0.2f).setEaseOutElastic();
        ++playerCount;
        CheckSigns();

    }

    public void RemovePlayer()
    {
        int listChildrenCount = listTransform.childCount;
        var itemToRemove = listTransform.GetChild(listChildrenCount - 2);
        foreach (Transform child in itemToRemove)
        {
            LeanTween.scale(child.gameObject, Vector3.zero, 0.2f).setEaseInElastic().setOnComplete(() =>
            {
                Destroy(itemToRemove.gameObject);
            });
        }
        
        --playerCount;
        CheckSigns();

    }

    void CheckSigns()
    {
        if (playerCount <= 1) minus.SetActive(false);
        else minus.SetActive(true);

        if (playerCount >= 4) plus.SetActive(false);
        else plus.SetActive(true);
    }

    public int GetPlayerCount()
    {
        return playerCount;
    }

}
